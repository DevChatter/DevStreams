using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
