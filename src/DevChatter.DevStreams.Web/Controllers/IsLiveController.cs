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

        public IsLiveController(ICrudRepository crudRepository,
            ITwitchStreamService twitchService)
        {
            _crudRepository = crudRepository;
            _twitchService = twitchService;
        }


        /// <summary>
        /// Get all live streams.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Channel> channels = await _crudRepository.GetAll<Channel>();
            List<string> twitchIds = channels.Select(x => x.Twitch.TwitchId).ToList();
            var liveTwitchIds = (await _twitchService.GetChannelLiveStates(twitchIds))
                .Where(x => x.IsLive)
                .Select(x => x.TwitchId)
                .ToList();
            var liveChannelNames = channels.Where(c => liveTwitchIds.Contains(c.Twitch.TwitchId)).Select(c => c.Name);
            return Ok(liveChannelNames);
        }

        [HttpGet, Route("{twitchId}")]
        public async Task<IActionResult> Get(string twitchId)
        {
            // TODO: Do this better. Extract and remove duplication.
            bool isLive = await _twitchService.IsLive(twitchId);
            return Ok(isLive);
        }
    }
}