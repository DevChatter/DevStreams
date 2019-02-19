using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class EditModel : PageModel
    {
        private readonly IScheduledStreamService _streamService;
        private readonly ICrudRepository _crudRepository;

        public EditModel(IScheduledStreamService streamService, ICrudRepository crudRepository)
        {
            _streamService = streamService;
            _crudRepository = crudRepository;
        }

        [BindProperty]
        public ScheduledStreamEditModel ViewModel { get; set; }

        public int ChannelId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream stream = await _crudRepository.Get<ScheduledStream>(id.Value);

            if (stream == null)
            {
                return NotFound();
            }

            ViewModel = stream.ToEditViewModel();
            ChannelId = stream.ChannelId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ScheduledStream model = await _crudRepository.Get<ScheduledStream>(ViewModel.Id);

            model.ApplyEditChanges(ViewModel);

            int updateCount = await _streamService.Update(model);

            if (updateCount == 0)
            {
                return NotFound();
            }

            return RedirectToPage("./Details", new { id = ViewModel.Id });
        }
    }
}
