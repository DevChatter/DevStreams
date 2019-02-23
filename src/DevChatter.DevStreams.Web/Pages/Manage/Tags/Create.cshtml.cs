using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Pages.Manage.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ICrudRepository _repo;

        public CreateModel(ICrudRepository repo)
        {
            _repo = repo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.Insert(Tag);

            return RedirectToPage("./Index");
        }
    }
}