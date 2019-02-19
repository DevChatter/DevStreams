using Dapper;
using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper.Services
{
    public class DapperSessionLookup : IStreamSessionService
    {
        private readonly DatabaseSettings _dbSettings;

        public DapperSessionLookup(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public async Task<List<EventResult>> Get(string timeZoneId, DateTime localDateTime, IEnumerable<int> includedTagIds)
        {

            DateTimeZone zone = DateTimeZoneProviders.Tzdb[timeZoneId];
            LocalDate localDate = LocalDate.FromDateTime(localDateTime);

            (Instant dayStart, Instant dayEnd) = ResolveDayRange(localDate, zone);


            const string sql =
                @"SELECT top 1 c.*, ss.Id--, t.*
                FROM [Channels] c
                INNER JOIN [ScheduledStream] ss on ss.ChannelId = c.Id
                --INNER JOIN [ChannelTag] ct on ct.ChannelId = c.Id
                --INNER JOIN [Tags] t on t.Id = ct.TagId
                WHERE ss.UtcEndTime > @dayStart 
                    AND ss.UtcStartTime < @dayEnd";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                List<EventResult> events = (await connection.QueryAsync<Channel, StreamSession, EventResult>(
                        sql, (channel, session) =>
                        {
                            var eventResult = new EventResult
                            {
                                Channel = channel,
                                StreamSession = session
                            };
                            return eventResult;
                        },
                        new { dayStart, dayEnd }, splitOn: "ChannelId,TagId"))
                    .ToList();
                return events;
            }
        }

        private static (Instant start, Instant end) ResolveDayRange(LocalDate input,
            DateTimeZone zone)
        {
            Instant dayStart = input.AtStartOfDayInZone(zone).ToInstant();
            Instant dayEnd = input.PlusDays(1).AtStartOfDayInZone(zone).ToInstant();
            return (dayStart, dayEnd);
        }

    }
}
