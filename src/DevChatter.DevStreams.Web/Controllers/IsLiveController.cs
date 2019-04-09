using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Get all live streams.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<TwitchChannel> channels = await _crudRepository.GetAll<TwitchChannel>();
            List<string> twitchIds = channels.Select(x => x.TwitchId).ToList();
            var liveTwitchIds = (await _twitchService.GetChannelLiveStates(twitchIds))
                .Where(x => x.IsLive)
                .Select(x => x.TwitchId)
                .ToList();
            var liveChannelNames = channels.Where(c => liveTwitchIds.Contains(c.TwitchId)).Select(c => c.TwitchName);
            return Ok(liveChannelNames);
        }

        [HttpGet, Route("{twitchId}")]
        public async Task<IActionResult> Get(string twitchId)
        {
            var liveState = await _twitchService.IsLive(twitchId);
            return Ok(liveState.IsLive);
        }
    }
}