using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel.LiveChannels;
using NodaTime;

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
        /// Get info about all currently live channels.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Channel> channels = (await _channelAggregateService.GetAllAggregates())
                .Where(c => c.Twitch != null).ToList();
            List<string> twitchIds = channels.Select(x => x?.Twitch?.TwitchId).ToList();
            twitchIds.RemoveAll(string.IsNullOrWhiteSpace);
            List<ChannelLiveState> channelLiveStates = (await _twitchService.GetChannelLiveStates(twitchIds));
            var liveTwitchData = channelLiveStates
                .Where(x => x.IsLive)
                .OrderByDescending(x => x.ViewerCount)
                .Select(x => x.ToViewModel(channels.Single(c => c.Twitch.TwitchId == x.TwitchId)))
                .ToList();

            return Ok(liveTwitchData);
        }

        [HttpGet, Route("{twitchId}")]
        public async Task<IActionResult> Get(string twitchId)
        {
            var liveState = await _twitchService.IsLive(twitchId);
            return Ok(liveState.IsLive);
        }
    }
}