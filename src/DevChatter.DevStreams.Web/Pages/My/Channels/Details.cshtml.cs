using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    public class DetailsModel : PageModel
    {
        private readonly ICrudRepository _repo;
        private readonly IChannelAggregateService _channelAggregateService;

        public DetailsModel(ICrudRepository repo, IChannelAggregateService channelAggregateService)
        {
            _repo = repo;
            _channelAggregateService = channelAggregateService;
        }

        public ChannelViewModel Channel { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = _channelAggregateService.GetAggregate(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            Channel = model.ToChannelViewModel();
            return Page();
        }
    }
}
