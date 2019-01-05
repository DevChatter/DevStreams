using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class DeleteModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public DeleteModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ScheduledStream ScheduledStream { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream = await _context.ScheduledStream
                    .Include(x => x.Channel)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (ScheduledStream == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream = await _context.ScheduledStream
                .Include(s => s.Channel)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (ScheduledStream != null)
            {
                _context.ScheduledStream.Remove(ScheduledStream);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", 
                    new { channelId = ScheduledStream.Channel.Id });
            }

            return NotFound();
        }
    }
}
