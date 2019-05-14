using DevChatter.DevStreams.Core;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.Events;
using DevChatter.DevStreams.Web.Data.ViewModel.Tags;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var includedTagIds = requestModel.SelectedTags.Select(t => t.Id).ToList();
            List<EventResult> events = await _streamSessionService
                .Get(requestModel.SelectedTimeZone, requestModel.SelectedDate, includedTagIds);

            return events
                .Select(x => x.StreamSession.ToViewModel(x.Channel))
                .ToList();
        }
    }

    public class EventsRequestModel
    {
        public string SelectedTimeZone { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<TagViewModel> SelectedTags { get; set; }
    }
}