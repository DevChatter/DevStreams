using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.DevStreams.Web.Pages.Streams
{
    public class CreateModel : PageModel
    {
        private static readonly LocalTimePattern TimePattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm");
        public static readonly ZonedDateTimePattern ZonedDateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm o<G>", null);

        private readonly IClock clock = SystemClock.Instance;
        public string TimeZoneId { get; set; }
        public string LocalTime { get; set; } = "14:00";
        public IEnumerable<SelectListItem> TimeZoneIds;
        public List<ZonedDateTime> SuggestedSessions = new List<ZonedDateTime>();
        public void OnGet()
        {
            IDateTimeZoneProvider dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
            var tzdbIds = dateTimeZoneProvider.Ids;

            // TODO: Use better names than the ID itself.
            TimeZoneIds = tzdbIds.Select(x => new SelectListItem(x, x));

            var localTime = new LocalTime(14,0,0,0);
            string formatted = LocalTimePattern.ExtendedIso.Format(localTime);

            var parsed = LocalTimePattern.ExtendedIso.Parse(formatted).Value;
        }

        public void OnPost()
        {
            var dayOfWeek = IsoDayOfWeek.Monday;
            var parseResult = TimePattern.Parse(LocalTime);
            var localTime = parseResult.Value;

            var zone = DateTimeZoneProviders.Tzdb["America/New_York"];
            //var version = DateTimeZoneProviders.Tzdb.VersionId;
            var zonedClock = clock.InZone(zone);

            LocalDate today = zonedClock.GetCurrentDate();
            LocalDate next = today.With(DateAdjusters.Next(dayOfWeek));

            SuggestedSessions = new List<ZonedDateTime>();
            for (int i = 0; i < 52; i++)
            {
                LocalDateTime nextLocalDateTime = next + localTime;

                SuggestedSessions.Add(nextLocalDateTime.InZoneLeniently(zone));
                next = next.PlusWeeks(1);
            }
        }
    }
}