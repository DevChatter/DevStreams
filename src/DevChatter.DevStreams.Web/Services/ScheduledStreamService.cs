using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using NodaTime;
using NodaTime.Extensions;

namespace DevChatter.DevStreams.Web.Services
{
    public class ScheduledStreamService : IScheduledStreamService
    {
        private readonly IClock _clock;

        public ScheduledStreamService(IClock clock)
        {
            _clock = clock;
        }

        public void AddScheduledStreamToChannel(Channel channel, ScheduledStream stream)
        {
            channel.ScheduledStreams.Add(stream);

            var timeZone = DateTimeZoneProviders.Tzdb[channel.TimeZoneId];
            CreateStreamSessions(stream, timeZone);
        }

        private void CreateStreamSessions(ScheduledStream stream, DateTimeZone timeZone)
        {
            ZonedClock zonedClock = _clock.InZone(timeZone);

            LocalDate nextOfDay = zonedClock.GetCurrentDate()
                .With(DateAdjusters.Next(stream.DayOfWeek));

            for (int i = 0; i < 52; i++)
            {
                LocalDateTime nextLocalStartDateTime = nextOfDay + stream.LocalStartTime;
                LocalDateTime nextLocalEndDateTime = nextOfDay + stream.LocalEndTime;

                var streamSession = new StreamSession
                {
                    TzdbVersionId = DateTimeZoneProviders.Tzdb.VersionId,
                    UtcStartTime = nextLocalStartDateTime
                        .InZoneLeniently(timeZone)
                        .ToInstant(),
                    UtcEndTime = nextLocalEndDateTime.InZoneLeniently(timeZone).ToInstant(),
                };

                stream.Sessions.Add(streamSession);

                nextOfDay = nextOfDay.PlusWeeks(1);
            }
        }
    }
}