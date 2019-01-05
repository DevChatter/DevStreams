using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class CreateModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public CreateModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Countries = TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name)
                .Select(x => new SelectListItem(x.Value, x.Key));

            return Page();
        }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [BindProperty]
        public Channel Channel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Channels.Add(Channel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}