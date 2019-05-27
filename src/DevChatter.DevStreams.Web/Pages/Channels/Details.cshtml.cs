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

        public string LogoUrl => (!string.IsNullOrWhiteSpace(Channel?.ImageUrl))
                ? Channel?.ImageUrl
                : $"https://via.placeholder.com/150/404041/FFFFFF/?text={Channel.Name}";

        public IActionResult OnGet(int id)
        {
            var channel = _channelAggregateService.GetAggregate(id);
            Channel = channel.ToChannelViewModel();

            return Page();
        }
    }
}