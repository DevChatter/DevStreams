using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class ChannelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChannelsController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet, Route("{id}")]
        public async Task<ChannelEditModel> Get(int id)
        {
            if (id <= 0)
            {
                return null; // TODO: Return an InvalidIdResult
            }

            var model = await _context.Channels
                        .Include(c => c.Tags)
                        .ThenInclude(t => t.Tag)
                        .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return null; // TODO: Return a NotFoundResult
            }

            var editModel = model.ToChannelEditModel();

            return editModel;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChannelEditModel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Channel model = await _context.Channels
                                    .Include(c => c.Tags)
                                    .FirstOrDefaultAsync(c => c.Id == channel.Id);

            model.ApplyEditChanges(channel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(channel.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");

            bool ChannelExists(int id)
            {
                return _context.Channels.Any(e => e.Id == id);
            }
        }
    }
}