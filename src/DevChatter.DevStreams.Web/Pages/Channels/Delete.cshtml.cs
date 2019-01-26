using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class DeleteModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public DeleteModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ChannelViewModel Channel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context
                        .Channels
                        .Include(x => x.ScheduledStreams)
                        .Include(x => x.Tags)
                        .ThenInclude(x => x.Tag)
                        .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            Channel = model.ToChannelViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Channels.FindAsync(id);

            if (model != null)
            {
                _context.Channels.Remove(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
