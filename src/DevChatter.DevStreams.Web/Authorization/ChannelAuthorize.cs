using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace DevChatter.DevStreams.Web.Authorization
{
    public class ChannelAuthorize : Attribute, IPageFilter, IActionFilter
    {
        private readonly string _parameter;
        private readonly Func<IServiceProvider, IChannelPermissionsService> _permissionsFactory;

        public ChannelAuthorize(string parameter)
            : this(parameter,
                provider => (IChannelPermissionsService)provider.GetService(typeof(IChannelPermissionsService)))
        {
        }

        public ChannelAuthorize(string parameter,
            Func<IServiceProvider, IChannelPermissionsService> permissionsFactory)
        {
            _parameter = parameter;
            _permissionsFactory = permissionsFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsAuthorized(context.HttpContext, context.ActionArguments))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Result = new JsonResult(nameof(HttpStatusCode.Unauthorized));
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (IsAuthorized(context.HttpContext, context.HandlerArguments))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Result = new JsonResult(nameof(HttpStatusCode.Unauthorized));

        }

        private bool IsAuthorized(HttpContext httpContext, 
            IDictionary<string, object> arguments)
        {
            var authorization = _permissionsFactory.Invoke(httpContext.RequestServices);

            string userId = GetUserId(httpContext);

            object value = arguments[_parameter];

            switch (value)
            {
                case Channel channel when channel.Id == 0 || authorization.CanAccessChannel(userId, channel.Id):
                case int id when authorization.CanAccessChannel(userId, id):
                case null:
                    return true;
            }

            return false;
        }

        private static string GetUserId(HttpContext httpContext)
        {
            var userId = httpContext.User.Claims.FirstOrDefault(x
                => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}