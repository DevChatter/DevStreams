using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class EditModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public EditModel(ICrudRepository repo)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _repo.Update(Tag)) // TODO: Exception Handling and Logging
            {
                return RedirectToPage("./Index");
            }

            return NotFound();
        }
    }
}
