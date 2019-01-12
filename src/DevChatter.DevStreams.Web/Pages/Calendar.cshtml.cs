using DevChatter.DevStreams.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Pages
{
    public class CalendarModel : PageModel
    {
        private readonly IStreamSessionService _streamSessionService;

        public CalendarModel(IStreamSessionService streamSessionService)
        {
            _streamSessionService = streamSessionService;
        }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public string CountryCode { get; set; } = "GB";

        public void OnGet()
        {
            Countries = TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name)
                .Select(x => new SelectListItem(x.Value, x.Key, x.Key == CountryCode));
        }
    }
}