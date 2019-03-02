using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    [ChannelAuthorize("id")]
    public class EditModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public EditModel(ICrudRepository repo)
        {
            _repo = repo;
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
                bool exists = await _repo.Exists<Channel>(id.Value);
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
