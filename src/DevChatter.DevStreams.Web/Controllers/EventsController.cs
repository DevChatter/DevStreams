using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.Events;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;

namespace DevChatter.DevStreams.Web.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IStreamSessionService _streamSessionService;

        public EventsController(IStreamSessionService streamSessionService)
        {
            _streamSessionService = streamSessionService;
        }

        [HttpPost]
        public async Task<IList<EventViewModel>> Post([FromBody] EventsRequestModel requestModel)
        {
            var includedTagIds = requestModel.IncludedTags.Select(t => t.Id);
            IList<StreamSession> sessions = 
                await _streamSessionService.Get(requestModel.SelectedTimeZone, requestModel.SelectedDate, includedTagIds);

            return sessions
                .Select(x => x.ToViewModel())
                .ToList();
        }
    }

    public class EventsRequestModel
    {
        public string SelectedTimeZone { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<TagViewModel> IncludedTags { get; set; }
    }
}