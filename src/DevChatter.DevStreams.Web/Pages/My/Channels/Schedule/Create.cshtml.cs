using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NodaTime;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduledStreamService _scheduledStreamService;

        public CreateModel(ApplicationDbContext context, IScheduledStreamService scheduledStreamService)
        {
            _context = context;
            _scheduledStreamService = scheduledStreamService;
        }

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

            _scheduledStreamService.AddScheduledStreamToChannel(channel, stream);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}