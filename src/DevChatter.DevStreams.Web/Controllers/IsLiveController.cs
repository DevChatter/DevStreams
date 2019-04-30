using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class IsLiveController : Controller
    {
        private readonly ITwitchStreamService _twitchService;
        private readonly ICrudRepository _crudRepository;
        private readonly IChannelAggregateService _channelAggregateService;

        public IsLiveController(ICrudRepository crudRepository,
            IChannelAggregateService channelAggregateService,
            ITwitchStreamService twitchService)
        {
            _crudRepository = crudRepository;
            _channelAggregateService = channelAggregateService;
            _twitchService = twitchService;
        }


        /// <summary>
        /// Get all live streams info.
        /// </summary>
        /// <returns>TwitchName, isLive = True, started_At, View Count</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Channel> channels = await _channelAggregateService.GetAllAggregate();
            List<string> twitchIds = channels.Select(x => x?.Twitch?.TwitchId).ToList();
            twitchIds.RemoveAll(string.IsNullOrWhiteSpace);
            var liveTwitchData = (await _twitchService.GetChannelLiveStates(twitchIds));

            var sortedLiveData = liveTwitchData.OrderByDescending(o => o.ViewerCount).ToList();

            var liveTwitchIds = sortedLiveData
                .Where(x => x.IsLive)
                .Select(x => x.TwitchId)
                .ToList();

            var liveChannelSorted = sortedLiveData
                .OrderByDescending(x => x.ViewerCount)
                .Select(x => channels.Where(y => y?.Twitch?.TwitchId == x?.TwitchId).Where(v => x.IsLive).Select(z => z?.Twitch?.TwitchName))
                .Where(x => x.Any())
                .ToList();

            var timeDifference = sortedLiveData
                .Where(x => liveTwitchIds.Contains(x.TwitchId))
                .Select(x => (DateTime.UtcNow - x?.StartedAt.ToUniversalTime()));

            var viewerCount = sortedLiveData
                .Where(x => liveTwitchIds.Contains(x.TwitchId))
                .Select(x => x.ViewerCount)
                .ToList();

            var responseObject = new
            {
                Channel = liveChannelSorted,
                viewCount = viewerCount,
                timeOnline = timeDifference
            };

            return Ok(responseObject);
        }

        [HttpGet, Route("{twitchId}")]
        public async Task<IActionResult> Get(string twitchId)
        {
            var liveState = await _twitchService.IsLive(twitchId);
            return Ok(liveState.IsLive);
        }
    }
}