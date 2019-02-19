using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class DeleteModel : PageModel
    {
        private readonly IScheduledStreamService _streamService;
        private readonly ICrudRepository _crudRepository;

        public DeleteModel(IScheduledStreamService streamService, ICrudRepository crudRepository)
        {
            _streamService = streamService;
            _crudRepository = crudRepository;
        }

        [BindProperty]
        public ScheduledStreamViewModel ScheduledStream { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream model = await _crudRepository.Get<ScheduledStream>(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            ScheduledStream = model.ToViewModel();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int deleteCount = await _streamService.Delete(id.Value);

            if (deleteCount == 0)
            {
                return NotFound();
            }

            return RedirectToPage("./Index",
                new { channelId = ScheduledStream.ChannelId });
        }
    }
}
