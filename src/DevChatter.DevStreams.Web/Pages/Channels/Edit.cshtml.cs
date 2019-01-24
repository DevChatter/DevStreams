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

        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> TimeZones { get; set; }

        [BindProperty]
        public ChannelEditModel Channel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Channels.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            Channel = model.ToChannelEditModel();

            Countries = TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name)
                .Select(x => 
                    new SelectListItem(x.Value, x.Key, x.Key == model.CountryCode));

            TimeZones = TimeZonesData.GetForCountry(model.CountryCode, null)
                .Select(x => 
                    new SelectListItem(x.Value, x.Key, x.Key == model.TimeZoneId));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Channel model = await _context.Channels
                                    .Include(c => c.Tags)
                                    .FirstOrDefaultAsync(c => c.Id == Channel.Id);

            model.ApplyEditChanges(Channel);

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

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.Id == id);
        }
    }
}
