using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    public class DeleteModel : PageModel
    {
        private readonly ICrudRepository _repo;
        private readonly IChannelAggregateService _channelAggregateService;

        public DeleteModel(ICrudRepository repo, IChannelAggregateService channelAggregateService)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            if (await _repo.Exists<Channel>(id.Value))
            {
                int countDeleted = await _channelAggregateService.Delete(id.Value);
                if (countDeleted == 0)
                {
                    return NotFound();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
