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
        private readonly ITwitchService _twitchService;
        private readonly ICrudRepository _crudRepository;

        public IsLiveController(ICrudRepository crudRepository,
            ITwitchService twitchService)
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
            // TODO: Do this better. Extract and remove duplication.
            List<Channel> channels = await _crudRepository.GetAll<Channel>();
            List<string> channelNames = channels.Select(x => x.Name).ToList();
            var liveChannels = await _twitchService.GetLiveChannels(channelNames);
            return Ok(liveChannels);
        }

        [HttpGet, Route("{twitchId}")]
        public async Task<IActionResult> Get(int twitchId)
        {
            // TODO: Do this better. Extract and remove duplication.
            bool isLive = await _twitchService.IsLive(twitchId);
            return Ok(isLive);
        }
    }
}