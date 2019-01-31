using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace DevChatter.DevStreams.Web.Services
{
    public class StreamSessionService : IStreamSessionService
    {
        private readonly ApplicationDbContext _context;

        public StreamSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<StreamSession>> Get(string timeZoneId, DateTime localDateTime,
            IEnumerable<int> includedTagIds)
        {
            DateTimeZone zone = DateTimeZoneProviders.Tzdb[timeZoneId];
            LocalDate localDate = LocalDate.FromDateTime(localDateTime);

            (Instant dayStart, Instant dayEnd) = ResolveDayRange(localDate, zone);

            List<StreamSession> sessions = await _context.StreamSessions
                .Include(x => x.ScheduledStream)
                .ThenInclude(x => x.Channel)
                .ThenInclude(x => x.Tags)
                .Where(x => x.UtcEndTime > dayStart && x.UtcStartTime < dayEnd 
                        && includedTagIds.All(t => x.ScheduledStream.Channel.Tags.Any(ct => ct.TagId == t)))
                .ToListAsync();

            return sessions.ToList();
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