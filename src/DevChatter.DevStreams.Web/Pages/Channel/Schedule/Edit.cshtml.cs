using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;

namespace DevChatter.DevStreams.Web.Pages.Streams.Schedule
{
    public class EditModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public EditModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ScheduledStream).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduledStreamExists(ScheduledStream.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ScheduledStreamExists(int id)
        {
            return _context.ScheduledStream.Any(e => e.Id == id);
        }
    }
}
