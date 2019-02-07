using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.My.Channels
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ChannelIndexModel> Channels { get;set; }

        public async Task OnGetAsync()
        {
            List<Channel> models = await _context.Channels
                .Include(x => x.ScheduledStreams)
                .ToListAsync();
            Channels = models.Select(model => model.ToChannelIndexModel()).ToList();
        }
    }
}
