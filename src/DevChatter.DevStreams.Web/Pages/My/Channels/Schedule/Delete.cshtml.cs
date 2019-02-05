using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.ScheduledStreams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ScheduledStreamViewModel ScheduledStream { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.ScheduledStream.Include(x => x.Channel).FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            ScheduledStream = model.ToViewModel(model.Channel);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelToDelete = await _context.ScheduledStream
                .Include(s => s.Channel)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (modelToDelete != null)
            {
                _context.ScheduledStream.Remove(modelToDelete);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", 
                    new { channelId = modelToDelete.Channel.Id });
            }

            return NotFound();
        }
    }
}
