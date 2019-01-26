using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Web.Controllers;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ChannelId { get; set; }

        public string Title { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                Title = "Edit";
                ChannelId = id.Value;
                bool exists = await _context.Channels.AnyAsync(m => m.Id == id);
                if (!exists)
                {
                    return NotFound();
                }
            }
            else
            {
                Title = "Create";
                ChannelId = -1;
            }

            Countries = TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name)
                .Select(x => new SelectListItem(x.Value, x.Key));

            return Page();
        }
    }
}
