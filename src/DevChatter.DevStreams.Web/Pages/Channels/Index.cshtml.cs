using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class IndexModel : PageModel
    {
        private readonly IChannelSearchService _channelSearch;

        public IndexModel(IChannelSearchService channelSearch)
        {
            _channelSearch = channelSearch;
        }

        public List<ChannelSearchModel> Channels { get; set; }

        public void OnGet()
        {
            Channels = _channelSearch.Find()
                .Select(x => x.ToChannelSearchModel())
                .ToList();
        }
    }
}