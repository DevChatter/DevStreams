using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeZoneNames;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class TimeZonesController : Controller
    {
        [HttpGet]
        public IDictionary<string, string> Get(string countryCode, DateTimeOffset? threshold)
        {
            var languageCode = CultureInfo.CurrentUICulture.Name;

            if (countryCode != null)
            {
                return GetTimeZonesForCountry(countryCode, threshold, languageCode);
            }

            return TZNames.GetCountryNames(languageCode)
                .SelectMany(x => GetTimeZonesForCountry(x.Key, threshold, languageCode)
                    .Select(y => new { CountryCode = x.Key, Country = x.Value, TimeZoneId = y.Key, TimeZoneName = y.Value }))
                .GroupBy(x => x.TimeZoneId)
                .ToDictionary(x => x.Key, x => $"{x.First().Country} - {x.First().TimeZoneName}");
        }

        private static IDictionary<string, string> GetTimeZonesForCountry(string countryCode, DateTimeOffset? threshold, string languageCode)
        {
            return threshold == null
                ? TZNames.GetTimeZonesForCountry(countryCode, languageCode)
                : TZNames.GetTimeZonesForCountry(countryCode, languageCode, threshold.Value);
        }
    }
}