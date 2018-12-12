using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        [HttpGet]
        public IDictionary<string, string> Get()
        {
            return TZNames.GetCountryNames(CultureInfo.CurrentUICulture.Name);
        }
    }
}