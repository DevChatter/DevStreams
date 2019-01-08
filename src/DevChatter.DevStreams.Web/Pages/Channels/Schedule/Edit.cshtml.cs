using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EditScheduledStream ViewModel { get; set; }

        public int ChannelId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream scheduledStream = await _context.ScheduledStream
                .Include(x => x.Channel) // Required for "Back to List" link
                .FirstOrDefaultAsync(m => m.Id == id);

            if (scheduledStream == null)
            {
                return NotFound();
            }

            ViewModel = scheduledStream.ToEditViewModel();
            ChannelId = scheduledStream.Channel.Id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ScheduledStream model = await _context.ScheduledStream
                .Include(x => x.Sessions) // Required for updating them.
                .SingleOrDefaultAsync(x => x.Id == ViewModel.Id);

            model.ApplyEditChanges(ViewModel);

            // TODO: Update the Sessions

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduledStreamExists(ViewModel.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Details", new { id = ViewModel.Id });

        }

        private bool ScheduledStreamExists(int id)
        {
            return _context.ScheduledStream.Any(e => e.Id == id);
        }
    }
}
