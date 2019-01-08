using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public ScheduledStream ScheduledStream { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ScheduledStream = await _context.ScheduledStream.Include(x => x.Channel).FirstOrDefaultAsync(m => m.Id == id);
            if (ScheduledStream == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
