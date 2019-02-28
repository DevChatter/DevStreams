using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.TwitchHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevChatter.DevStreams.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IChannelAggregateService _channelAggregateService;
        private readonly ITwitchService _twitchService;

        public IndexModel(IChannelAggregateService channelAggregateService, ITwitchService twitchService)
        {
            _channelAggregateService = channelAggregateService;
            _twitchService = twitchService;
        }
        public async Task<IActionResult> OnGetLuckyAsync()
        {
            var liveChannels = await _twitchService.GetLiveChannels();
            var result = new Result();

            if (liveChannels.Count == 0)
            {
                result.Error = "No live channels available right now, try again later!";
            }
            else
            {
                result.ChannelName = liveChannels.PickOneRandomElement();
            }

            return new JsonResult(result);
        }
    }

    public class Result
    {
        public string ChannelName { get; set; }
        public string Error { get; set; }
    }
}
