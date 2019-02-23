using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public IndexModel(ICrudRepository repo)
        {
            _repo = repo;
        }

        public IList<Tag> Tag { get;set; }

        public async Task OnGetAsync()
        {
            Tag = await _repo.GetAll<Tag>();
        }
    }
}
