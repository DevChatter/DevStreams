using System.Collections.Generic;
using System.Globalization;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class EditModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public EditModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [BindProperty]
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

            Countries = TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name)
                .Select(x => new SelectListItem(x.Value, x.Key));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Channel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(Channel.Id))
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

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.Id == id);
        }
    }
}
