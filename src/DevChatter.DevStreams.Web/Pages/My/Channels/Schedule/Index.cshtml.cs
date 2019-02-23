using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly ICrudRepository _crudRepository;
        private readonly IScheduledStreamService _scheduledStreamService;

        public IndexModel(ICrudRepository crudRepository, IScheduledStreamService scheduledStreamService)
        {
            _crudRepository = crudRepository;
            _scheduledStreamService = scheduledStreamService;
        }

        public List<ScheduledStreamViewModel> ScheduledStreams { get; set; }
        public Channel Channel { get; set; }

        public async Task<IActionResult> OnGetAsync(int channelId)
        {
            if (channelId <= 0)
            {
                return NotFound();
            }

            var streams = await _scheduledStreamService.GetChannelSchedule(channelId);

            Channel = await _crudRepository.Get<Channel>(channelId);

            if (Channel == null)
            {
                return NotFound();
            }

            ScheduledStreams = streams.Select(x => x.ToViewModel()).ToList();

            return Page();
        }
    }
}
