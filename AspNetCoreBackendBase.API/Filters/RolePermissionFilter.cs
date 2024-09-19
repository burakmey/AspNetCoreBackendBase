using Application.Attributes;
using AspNetCoreBackendBase.Application.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Extensions;

namespace AspNetCoreBackendBase.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Retrieve the user identity name from the HTTP context.
            var userName = context.HttpContext.User.Identity?.Name;

            // If the user is authenticated, process the authorization check.
            if (string.IsNullOrEmpty(userName))
            {
                await next();
                return;
            }

            // If the descriptor is null, proceed to the next action.
            if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
            {
                await next();
                return;
            }

            // Retrieve the AuthorizeDefinitionAttribute from the method.
            var attribute = descriptor.MethodInfo.GetCustomAttribute<AuthorizeDefinitionAttribute>();

            // If the attribute is not present, proceed to the next action.
            if (attribute == null)
            {
                await next();
                return;
            }

            // Retrieve the controller name (route) from the descriptor.
            string route = descriptor.ControllerName.RemoveControllerSuffix();

            // Retrieve the HTTP method attribute from the method.
            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute<HttpMethodAttribute>();
            var httpMethod = httpAttribute?.HttpMethods.First() ?? HttpMethods.Get;

            // Construct the code string based on HTTP method, action type, and definition.
            var code = $"{httpMethod}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

            // Check if the user has the required role for the endpoint.
            var hasRole = await _userService.HasRolePermissionForEndpointAsync(userName, code, route);

            // If the user has the required role, proceed to the next action. Otherwise, return unauthorized response.
            if (hasRole)
                await next();
            else
                context.Result = new UnauthorizedObjectResult(new BaseResponse<object> { IsSuccessful = false, Message = "Unauthorized" });
        }


        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var name = context.HttpContext.User.Identity?.Name;
        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //        var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

        //        if (attribute != null)
        //        {
        //            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

        //            var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

        //            var hasRole = await _userService.HasRolePermissionForEndpointAsync(name, code);

        //            if (hasRole)
        //                await next();
        //            else
        //                context.Result = new UnauthorizedObjectResult(new BaseResponse<object>() { IsSuccessful = false, Message = "Unauthorized" });
        //        }
        //        else
        //            await next();
        //    }
        //    else
        //        await next();
        //}
    }
}
