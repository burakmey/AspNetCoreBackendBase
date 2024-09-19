using Application.Attributes;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace AspNetCoreBackendBase.Application.Utils
{
    /// <summary>
    /// Provides utility methods for working with assemblies.
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Retrieves the route names from the controllers in the loaded assembly.
        /// </summary>
        /// <remarks>
        /// The route names are extracted from controllers that inherit from <see cref="ControllerBase"/>.
        /// These are typically API controllers in ASP.NET Core applications.
        /// </remarks>
        /// <returns>
        /// A list of <see langword="string"/> containing route names from the assembly's API controllers.
        /// </returns>
        public static List<string> GetRoutesWithAssembly()
        {
            // Determines the current environment and sets the appropriate assembly path.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var assemblyPath = environment == "Development"
                ? Path.Combine(Directory.GetCurrentDirectory(), "../Presentation/AspNetCoreBackendBase.API/bin/Debug/net8.0/AspNetCoreBackendBase.API.dll")
                : Directory.GetCurrentDirectory();

            // Checks if the assembly file exists at the given path.
            if (!File.Exists(assemblyPath))
                return [];

            // Loads the assembly.
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            List<string> routes = [];

            // Retrieves all controller types from the assembly.
            var controllers = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            // Iterates over each controller to extract and process route names.
            foreach (var controller in controllers)
                // Removes the "Controller" suffix for a cleaner route name.
                routes.Add(controller.Name.RemoveControllerSuffix());

            return routes;
        }

        /// <summary>
        /// Retrieves the menu definitions, including routes and action codes, from the controllers in the loaded assembly.
        /// </summary>
        /// <returns>A list of <see cref="Menu"/> objects, each containing route and associated action codes.</returns>
        public static List<Menu> GetMenusWithAssembly()
        {
            // Determines the current environment and sets the appropriate assembly path.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var assemblyPath = environment == "Development"
                ? Path.Combine(Directory.GetCurrentDirectory(), "../Presentation/AspNetCoreBackendBase.API/bin/Debug/net8.0/AspNetCoreBackendBase.API.dll")
                : Directory.GetCurrentDirectory();

            // Checks if the assembly file exists at the given path.
            if (!File.Exists(assemblyPath))
                return [];

            // Loads the assembly.
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            List<Menu> menus = [];

            // Retrieves all controller types from the assembly.
            var controllers = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            // Iterates over each controller to find and process actions with the AuthorizeDefinitionAttribute.
            foreach (var controller in controllers)
            {
                // Retrieves methods that are decorated with the AuthorizeDefinitionAttribute.
                var actions = controller.GetMethods().Where(method => method.IsDefined(typeof(AuthorizeDefinitionAttribute), false));

                foreach (var action in actions)
                {
                    // Retrieves the AuthorizeDefinitionAttribute from the method.
                    var authorizeAttribute = action.GetCustomAttribute<AuthorizeDefinitionAttribute>();
                    if (authorizeAttribute == null)
                        continue;

                    // Retrieves or creates a menu based on the route defined in the AuthorizeDefinitionAttribute.
                    var menu = menus.FirstOrDefault(m => m.Route == authorizeAttribute.Route) ?? new Menu { Route = authorizeAttribute.Route };
                    if (!menus.Contains(menu))
                        menus.Add(menu);

                    // Retrieves the HTTP method type for the action, defaulting to GET if none is specified.
                    var httpAttribute = action.GetCustomAttributes<HttpMethodAttribute>(false).FirstOrDefault();
                    var httpType = httpAttribute?.HttpMethods.First() ?? Microsoft.AspNetCore.Http.HttpMethods.Get;

                    // Constructs the action code using the HTTP method, action type, and definition from the attribute.
                    string code = $"{httpType}.{authorizeAttribute.ActionType}.{authorizeAttribute.Definition}";
                    // Adds the action code to the menu.
                    menu.Codes.Add(code); 
                }
            }
            return menus;
        }
    }
}
