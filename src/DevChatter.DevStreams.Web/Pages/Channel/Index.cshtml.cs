using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Streams
{
    public class IndexModel : PageModel
    {
        private readonly DevChatter.DevStreams.Web.Data.ApplicationDbContext _context;

        public IndexModel(DevChatter.DevStreams.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Channel> Channel { get;set; }

        public async Task OnGetAsync()
        {
            Channel = await _context.Channels.ToListAsync();
        }
    }
}
