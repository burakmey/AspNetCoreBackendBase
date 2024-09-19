using AspNetCoreBackendBase.Application.Enums;
using AspNetCoreBackendBase.Application.Extensions;

namespace Application.Attributes
{
    /// <summary>
    /// Specifies authorization details for a method, including the action type, definition, and route.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method,  // This attribute can only be applied to methods.
        Inherited = false,        // This attribute is not inherited by derived classes.
        AllowMultiple = false     // Only one instance of this attribute can be applied to a method. Cant be doublicated on related action methods
    )]
    public class AuthorizeDefinitionAttribute(ActionType actionType, string definition, string route) : Attribute
    {
        /// <summary>
        /// Gets or sets the action type for authorization (e.g., <see cref="ActionType.Read"/>, <see cref="ActionType.Write"/>).
        /// </summary>
        /// <value>
        /// An <see cref="AspNetCoreBackendBase.Application.Enums.ActionType"/> representing the type of action required for authorization.
        /// </value>
        /// 
        public ActionType ActionType { get; set; } = actionType;

        /// <summary>
        /// Gets or sets the definition of the action for authorization purposes.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the description or definition of the action that needs authorization.
        /// </value>
        public string Definition { get; set; } = definition;

        /// <summary>
        /// Gets or sets the route name for authorization.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> representing the route name associated with the authorization action.
        /// The route name is processed to remove the "Controller" suffix if it exists.
        /// </value>
        public string Route { get; set; } = route.RemoveControllerSuffix();
    }
}
