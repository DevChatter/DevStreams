using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
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

        public IActionResult OnGet(int id)
        {
            var channel = _channelAggregateService.GetAggregate(id);
            Channel = channel.ToChannelViewModel();

            return Page();
        }
    }
}