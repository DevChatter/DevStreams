using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public IndexModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ScheduledStreamViewModel> ScheduledStreams { get; set; }
        public string ChannelName { get; set; }

        public async Task OnGetAsync(int channelId)
        {
            var channel = await _context.Channels
                .Include(x => x.ScheduledStreams)
                .SingleAsync(x => x.Id == channelId);

            ChannelName = channel.Name;

            ScheduledStreams = channel.ToScheduledStreamViewModels();
        }
    }
}
