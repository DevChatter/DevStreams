using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel;
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

        [HttpGet]
        public async Task<IList<EventViewModel>> Get(string timeZoneId, DateTime localDateTime)
        {

            IList<StreamSession> sessions = 
                await _streamSessionService.Get(timeZoneId, localDateTime);

            return sessions.Select(ToViewModel).ToList();
        }

        private static EventViewModel ToViewModel(StreamSession x)
        {
            return new EventViewModel
            {
                Id = x.Id,
                ScheduledStreamId = x.ScheduledStream.Id,
                ChannelId = x.ScheduledStream.Channel.Id,
                ChannelName = x.ScheduledStream.Channel.Name,
                Uri = x.ScheduledStream.Channel.Uri,
                UtcStartTime = x.UtcStartTime.ToDateTimeUtc(),
                UtcEndTime = x.UtcEndTime.ToDateTimeUtc()
            };
        }
    }
}