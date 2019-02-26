using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class ChannelsController : Controller
    {
        private readonly IChannelAggregateService _channelService;
        private readonly ICrudRepository _crudRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ChannelsController(IChannelAggregateService channelService,
            ICrudRepository crudRepository, UserManager<IdentityUser> userManager)
        {
            _channelService = channelService;
            _crudRepository = crudRepository;
            _userManager = userManager;
        }

        [HttpGet, Route("{id}")]
        public ChannelEditModel Get(int id)
        {
            if (id <= 0)
            {
                return new ChannelEditModel();
            }

            var channel = _channelService.GetAggregate(id);

            var editModel = channel?.ToChannelEditModel();

            return editModel;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChannelEditModel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (IsNewChannel(channel))
                {
                    Channel model = new Channel();
                    model.ApplyEditChanges(channel);

                    // Pull into Authorize Attribute
                    string userId = (await _userManager.GetUserAsync(User)).Id;

                    await _channelService.Create(model, userId);
                }
                else
                {
                    Channel model = _channelService.GetAggregate(channel.Id);
                    model.ApplyEditChanges(channel);
                    await _channelService.Update(model);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ChannelExists(channel.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok();

            bool IsNewChannel(ChannelEditModel c) => c.Id <= 0;
        }

        private async Task<bool> ChannelExists(int id)
        {
            return await _crudRepository.Exists<Channel>(id);
        }
    }
}