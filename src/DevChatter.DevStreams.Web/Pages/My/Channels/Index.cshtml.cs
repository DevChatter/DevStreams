using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    public class IndexModel : PageModel
    {
        private readonly IChannelAggregateService _channelAggregateService;

        public IndexModel(IChannelAggregateService channelAggregateService)
        {
            _channelAggregateService = channelAggregateService;
        }

        public IList<ChannelIndexModel> Channels { get;set; }

        public IActionResult OnGetAsync()
        {
            List<Channel> models = _channelAggregateService.GetAll();
            Channels = models.Select(model => model.ToChannelIndexModel()).ToList();

            return Page();
        }
    }
}
