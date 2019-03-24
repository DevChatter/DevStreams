using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel.Events;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class DetailsModel : PageModel
    {
        private readonly IChannelAggregateService _channelAggregateService;
        private readonly ICrudRepository _repo;

        public DetailsModel(IChannelAggregateService channelAggregateService,
            ICrudRepository repo)
        {
            _channelAggregateService = channelAggregateService;
            _repo = repo;
        }

        public ChannelViewModel Channel { get; set; }
        public List<ScheduledStreamViewModel> ScheduledStreams { get; set; }
        public IEnumerable<StreamSession> StreamSessions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var channel = _channelAggregateService.GetAggregate(id);
            Channel = channel.ToChannelViewModel();

            ScheduledStreams = channel.ScheduledStreams
                .Select(x => x.ToViewModel())
                .ToList();

            StreamSessions = (await _repo
                .GetAll<StreamSession>("ChannelId = @id", new { id })).Take(5); // TODO: Limit closer to DB

            return Page();
        }
    }
}