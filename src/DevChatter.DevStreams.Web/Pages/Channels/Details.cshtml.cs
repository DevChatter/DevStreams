using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class DetailsModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public DetailsModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Channel Channel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Channel = await _context.Channels.FirstOrDefaultAsync(m => m.Id == id);

            if (Channel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
