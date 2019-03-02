using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Authorization
{
    public class ChannelPermissionsHandler : AuthorizationHandler<ChannelOwnerRequirement>
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IChannelPermissionsService _permissionsService;

        public ChannelPermissionsHandler(IActionContextAccessor actionContextAccessor, IChannelPermissionsService permissionsService)
        {
            _actionContextAccessor = actionContextAccessor;
            _permissionsService = permissionsService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            ChannelOwnerRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var id = _actionContextAccessor.ActionContext.RouteData.Values["id"] as int?;

            if (id is null || _permissionsService.CanAccessChannel(userId, id.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}