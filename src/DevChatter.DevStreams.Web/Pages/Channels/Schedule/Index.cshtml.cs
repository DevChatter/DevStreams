using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Channels.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public IndexModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Channel Channel { get;set; }

        public async Task OnGetAsync(int channelId)
        {
            Channel = await _context.Channels
                .Include(x => x.ScheduledStreams)
                .SingleAsync(x => x.Id == channelId);
        }
    }
}
