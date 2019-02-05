using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.DevStreams.Web.Pages.Channels
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<ChannelSearchModel> Channels { get; set; }

        public void OnGet()
        {
            Channels = _db.Channels
                .Include(x => x.Tags)
                .ThenInclude(t => t.Tag)
                .Select(x => x.ToChannelSearchModel())
                .ToList();
        }
    }
}