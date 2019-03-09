using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICrudRepository _repo;
        private readonly ITwitchService _twitchService;

        public IndexModel(ICrudRepository repo, ITwitchService twitchService)
        {
            _repo = repo;
            _twitchService = twitchService;
        }

        public async Task<IActionResult> OnGetLuckyAsync()
        {
            List<Channel> channels = await _repo.GetAll<Channel>();
            List<string> channelNames = channels.Select(x => x.Name).ToList();
            var liveChannels = await _twitchService.GetLiveChannels(channelNames);
            var result = new Result();

            if (liveChannels.Any())
            {
                result.ChannelName = liveChannels.PickOneRandomElement();
            }
            else
            {
                result.Error = "No live channels available right now, try again later!";
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
