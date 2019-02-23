using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    public class IndexModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public IndexModel(ICrudRepository repo)
        {
            _repo = repo;
        }

        public IList<ChannelIndexModel> Channels { get;set; }

        public async Task OnGetAsync()
        {
            List<Channel> models = await _repo.GetAll<Channel>();
            // TODO: Pull in Scheduled Streams
            Channels = models.Select(model => model.ToChannelIndexModel()).ToList();
        }
    }
}
