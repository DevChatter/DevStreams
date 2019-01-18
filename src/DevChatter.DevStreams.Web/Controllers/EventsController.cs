using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Data.ViewModel.Events;
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

            return sessions
                .Select(x => x.ToViewModel())
                .ToList();
        }
    }
}