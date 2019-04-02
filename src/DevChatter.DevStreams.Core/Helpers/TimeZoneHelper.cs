using NodaTime;
using System;

namespace DevChatter.DevStreams.Core.Helpers
{
    public static class TimeZoneHelper
    {
        public static LocalTime ConvertLocalTimeToDifferentTimeZone(LocalTime fromTime, string fromZoneId, string toZoneId)
        {
            var localDateTime = LocalDateTime.FromDateTime(DateTime.Today
                .AddHours(fromTime.Hour)
                .AddMinutes(fromTime.Minute));
            var fromZone = DateTimeZoneProviders.Tzdb[fromZoneId];
            var fromZoned = localDateTime.InZoneLeniently(fromZone);

            var toZone = DateTimeZoneProviders.Tzdb[toZoneId];
            var toZoned = fromZoned.WithZone(toZone);
            var toLocal = toZoned.LocalDateTime;

            return new LocalTime(toLocal.Hour, toLocal.Minute);
        }
    }
}
