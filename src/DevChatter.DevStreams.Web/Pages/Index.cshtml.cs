using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
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
        private readonly ITwitchStreamService _twitchService;

        public IndexModel(ICrudRepository repo, ITwitchStreamService twitchService)
        {
            _repo = repo;
            _twitchService = twitchService;
        }

        public List<ChannelIndexModel> NewlyAddedChannels { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // TODO: Don't pull the data like this....
            List<Channel> channels = await _repo.GetAll<Channel>();
            //List<string> twitchIds = channels.Select(x => x?.Twitch?.TwitchId)
            //    .Where(x => !string.IsNullOrWhiteSpace(x))
            //    .ToList();
            //var liveTwitchIds = (await _twitchService.GetChannelLiveStates(twitchIds))
            //    .Where(x => x.IsLive)
            //    .Select(x => x.TwitchId)
            //    .ToList();

            //LiveChannels = channels
            //    .Where(x => liveTwitchIds.Contains(x?.Twitch?.TwitchId))
            //    .Select(x => x.ToChannelIndexModel())
            //    .ToList();

            NewlyAddedChannels = channels
                .Take(5)
                .Select(x => x.ToChannelIndexModel())
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetLuckyAsync()
        {
            List<Channel> channels = await _repo.GetAllChannelInfo();

            List<string> twitchIds = channels.Select(x => x?.Twitch?.TwitchId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var liveTwitchId = (await _twitchService.GetChannelLiveStates(twitchIds))
                .Where(x => x.IsLive)
                .Select(x => x.TwitchId)
                .ToList().PickOneRandomElement();

            var liveChannel = channels
                .Where(x => x?.Twitch?.TwitchId == liveTwitchId)
                .Select(x => x?.Name);

            var result = new Result(); ;

            if (liveChannel.Any())
            {
                result.ChannelName = liveChannel.First();
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
