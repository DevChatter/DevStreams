using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public DeleteModel(ICrudRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Required: SimpleCRUD Fails to delete Tag by Id only.
            Tag = await _repo.Get<Tag>(id.Value);

            if (Tag != null)
            {
                await _repo.Delete(Tag);
            }

            return RedirectToPage("./Index");
        }
    }
}
