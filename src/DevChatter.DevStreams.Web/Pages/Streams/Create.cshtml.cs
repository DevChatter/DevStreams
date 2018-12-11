using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevChatter.DevStreams.Web.Pages.Streams
{
    public class CreateModel : PageModel
    {
        public int LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations;
        public void OnGet()
        {
            var locations = new List<Location>
            {
                new Location(1, "Country 1"),
                new Location(2, "Country 2"),
                new Location(3, "Country 3"),
            };
            Locations = locations.ToSelectListItems();
        }
    }
}