using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
using System.Globalization;
using System.Threading.Tasks;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class CreateModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public CreateModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
            _clock = SystemClock.Instance;
        }

        public static readonly ZonedDateTimePattern ZonedDateTimePattern = 
            ZonedDateTimePattern.CreateWithInvariantCulture("HH:mm o<G>", null);

        private readonly IClock _clock;

        [BindProperty]
        public CreateStreamTimeViewModel StreamTime { get; set; } = new CreateStreamTimeViewModel
        {
            DayOfWeek = IsoDayOfWeek.Monday,
            LocalStartTime = "14:00",
            LocalEndTime = "16:00",
        };

        public string TimeZoneName { get; set; }

        public async Task OnGetAsync(int channelId)
        {
            var channel = await _context.Channels.FindAsync(channelId);
            
            TimeZoneName = TZNames.GetNamesForTimeZone(channel.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic;
        }

        public async Task<IActionResult> OnPostAsync(int channelId)
        {
            ScheduledStream stream = StreamTime.ToModel();

            var channel = await _context.Channels
                .Include(x => x.ScheduledStreams)
                .SingleAsync(x => x.Id == channelId);

            channel.ScheduledStreams.Add(stream);

            var zone = DateTimeZoneProviders.Tzdb[channel.TimeZoneId];
            var version = DateTimeZoneProviders.Tzdb.VersionId;
            ZonedClock zonedClock = _clock.InZone(zone);

            LocalDate today = zonedClock.GetCurrentDate();
            LocalDate next = today.With(DateAdjusters.Next(stream.DayOfWeek));

            
            for (int i = 0; i < 52; i++)
            {
                LocalDateTime nextLocalStartDateTime = next + stream.LocalStartTime;
                LocalDateTime nextLocalEndDateTime = next + stream.LocalEndTime;

                var streamSession = new StreamSession
                {
                    TzdbVersionId = version,
                    UtcStartTime = nextLocalStartDateTime.InZoneLeniently(zone).ToInstant(),
                    UtcEndTime = nextLocalEndDateTime.InZoneLeniently(zone).ToInstant(),
                };

                stream.Sessions.Add(streamSession);

                next = next.PlusWeeks(1);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");

        }
    }
}