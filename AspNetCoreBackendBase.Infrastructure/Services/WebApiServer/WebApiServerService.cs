using Application.Attributes;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AspNetCoreBackendBase.Infrastructure.Services.WebApiServer
{
    public class WebApiServerService : IWebApiServerService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            // Get the assembly of the provided type.
            Assembly? assembly = Assembly.GetAssembly(type);
            if (assembly == null)
                throw new Exception("Assembly not found!");

            // Find all controller types in the assembly.
            var controllers = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            List<Menu> menus = [];

            foreach (var controller in controllers)
            {
                // Find methods with the AuthorizeDefinitionAttribute.
                var actions = controller.GetMethods().Where(method => method.IsDefined(typeof(AuthorizeDefinitionAttribute), false));

                foreach (var action in actions)
                {
                    // Get the AuthorizeDefinitionAttribute from the method.
                    var authorizeAttribute = action.GetCustomAttribute<AuthorizeDefinitionAttribute>();
                    if (authorizeAttribute == null)
                        continue;

                    // Get or create the menu.
                    var menu = menus.FirstOrDefault(m => m.Route == authorizeAttribute.Route) ?? new Menu { Route = authorizeAttribute.Route };
                    Console.WriteLine(menu.Route);
                    if (!menus.Contains(menu))
                        menus.Add(menu);

                    // Create the action.
                    var httpAttribute = action.GetCustomAttributes<HttpMethodAttribute>(false).FirstOrDefault();
                    var httpType = httpAttribute?.HttpMethods.First() ?? HttpMethods.Get;

                    string code = $"{httpType}.{authorizeAttribute.ActionType}.{authorizeAttribute.Definition}";
                    menu.Codes.Add(code);
                }
            }
            return menus;
        }
    }
}
