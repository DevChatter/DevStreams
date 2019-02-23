using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Globalization;
using System.Threading.Tasks;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class CreateModel : PageModel
    {
        private readonly ICrudRepository _crudRepository;
        private readonly IScheduledStreamService _scheduledStreamService;

        public CreateModel(ICrudRepository crudRepository, IScheduledStreamService scheduledStreamService)
        {
            _crudRepository = crudRepository;
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
            Channel channel = await _crudRepository.Get<Channel>(channelId);

            TimeZoneName = TZNames.GetNamesForTimeZone(channel.TimeZoneId, CultureInfo.CurrentUICulture.Name).Generic;
        }

        public async Task<IActionResult> OnPostAsync(int channelId)
        {
            ScheduledStream stream = StreamTime.ToModel();

            Channel channel = await _crudRepository.Get<Channel>(channelId);

            stream.ChannelId = channelId;
            stream.TimeZoneId = channel.TimeZoneId;

            int? id = await _scheduledStreamService.AddScheduledStreamToChannel(stream);

            return RedirectToPage("./Index", new { channelId });
        }
    }
}