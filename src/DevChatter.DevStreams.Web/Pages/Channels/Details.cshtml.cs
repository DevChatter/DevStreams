using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class DetailsModel : PageModel
    {
        private readonly IChannelAggregateService _channelAggregateService;

        public DetailsModel(IChannelAggregateService channelAggregateService)
        {
            _channelAggregateService = channelAggregateService;
        }

        public ChannelViewModel Channel { get; set; }
        public List<ScheduledStreamViewModel> ScheduledStreams { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var channel = _channelAggregateService.GetAggregate(id);
            Channel = channel.ToChannelViewModel();

            return Page();
        }
    }
}