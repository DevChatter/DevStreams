using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Tag> Tag { get;set; }

        public async Task OnGetAsync()
        {
            Tag = await _context.Tags.ToListAsync();
        }
    }
}
