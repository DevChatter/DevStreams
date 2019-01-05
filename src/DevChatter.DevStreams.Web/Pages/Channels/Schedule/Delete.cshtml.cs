using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;

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

            ScheduledStream = await _context.ScheduledStream.FindAsync(id);

            if (ScheduledStream != null)
            {
                _context.ScheduledStream.Remove(ScheduledStream);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
