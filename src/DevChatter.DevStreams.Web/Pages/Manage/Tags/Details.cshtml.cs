using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public DetailsModel(ICrudRepository repo)
        {
            _repo = repo;
        }

        public Tag Tag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tag = await _repo.Get<Tag>(id.Value);

            if (Tag == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
