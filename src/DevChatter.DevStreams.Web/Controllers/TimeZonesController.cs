using DevChatter.DevStreams.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class TimeZonesController : Controller
    {
        [HttpGet]
        public IDictionary<string, string> Get(string countryCode, DateTimeOffset? threshold)
        {
            return TimeZonesData.GetForCountry(countryCode, threshold);
        }

    }
}