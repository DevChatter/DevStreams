using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels.Schedule
{
    public class DetailsModel : PageModel
    {
        private readonly ICrudRepository _crudRepository;

        public DetailsModel(ICrudRepository crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public ScheduledStreamViewModel ScheduledStream { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _crudRepository.Get<ScheduledStream>(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            ScheduledStream = model.ToViewModel();

            return Page();
        }
    }
}
