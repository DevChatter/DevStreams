using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Pages.Streams;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevChatter.DevStreams.Web.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Location> src)
        {
            return src.Select(loc => new SelectListItem(loc.Name, loc.Id.ToString()));
        }
    }
}
