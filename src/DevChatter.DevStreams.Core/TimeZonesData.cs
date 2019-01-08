using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeZoneNames;

namespace DevChatter.DevStreams.Core
{
    public static class TimeZonesData
    {
        public static IDictionary<string, string> GetForCountry(string countryCode, DateTimeOffset? threshold)
        {
            var languageCode = CultureInfo.CurrentUICulture.Name;

            if (countryCode != null)
            {
                return GetTimeZonesForCountryAndLanguage(countryCode, threshold, languageCode);
            }

            return TZNames.GetCountryNames(languageCode)
                .SelectMany(x => GetTimeZonesForCountryAndLanguage(x.Key, threshold, languageCode)
                    .Select(y => new { CountryCode = x.Key, Country = x.Value, TimeZoneId = y.Key, TimeZoneName = y.Value }))
                .GroupBy(x => x.TimeZoneId)
                .ToDictionary(x => x.Key, x => $"{x.First().Country} - {x.First().TimeZoneName}");

        }

        private static IDictionary<string, string> GetTimeZonesForCountryAndLanguage(string countryCode, DateTimeOffset? threshold, string languageCode)
        {
            return threshold == null
                ? TZNames.GetTimeZonesForCountry(countryCode, languageCode)
                : TZNames.GetTimeZonesForCountry(countryCode, languageCode, threshold.Value);
        }

    }
}