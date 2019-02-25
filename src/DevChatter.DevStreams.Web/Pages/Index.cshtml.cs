using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.DevStreams.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IChannelAggregateService _channelAggregateService;

        public IndexModel(IChannelAggregateService channelAggregateService)
        {
            _channelAggregateService = channelAggregateService;
        }
        public async Task<IActionResult> OnGetLuckyAsync()
        {
            var channel = await _channelAggregateService.LuckySearch();
            return new JsonResult(channel);
        }
    }
}
