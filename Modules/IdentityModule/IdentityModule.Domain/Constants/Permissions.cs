using System.ComponentModel;

namespace IdentityModule.Domain.Constants
{
    /// <summary>
    /// Provides a centralized definition of application permissions, organized by functional areas.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class contains nested static classes that group permission
    /// constants by domain, such as <see cref="Users"/>, <see cref="Roles"/>, <see cref="RoleClaims"/>, and <see
    /// cref="AuditTrails"/>. Each nested class defines string constants representing specific permissions within that
    /// domain. These constants are typically used for access control checks throughout the application.  Permissions
    /// are defined as strings in the format "Permissions.[Domain].[Action]" to ensure consistency and clarity. For
    /// example, <c>Permissions.Users.View</c> represents the permission to view user-related data.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Provides a collection of constants representing permission keys for user-related operations.
        /// </summary>
        /// <remarks>These constants define the permission keys used to control access to various user
        /// management functionalities,  such as viewing, creating, editing, and deleting users. They can be used in
        /// authorization checks to enforce  role-based or permission-based access control within the
        /// application.</remarks>
        [DisplayName("Users"), Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
            public const string CanApprove = "Permissions.Users.CanApproveRegistrations";
            public const string CanCreateChat = "Permissions.Users.CanCreateChat";
            public const string CanSendMessage = "Permissions.Users.CanSendMessage";
        }

        /// <summary>
        /// Provides a collection of constants representing permission keys for role-related operations.
        /// </summary>
        /// <remarks>These constants define the permissions required to perform specific actions related
        /// to roles, such as viewing, creating, editing, deleting, and searching roles.  They can be used to enforce
        /// access control in applications by associating these keys with authorization policies.</remarks>
        [DisplayName("Roles"), Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
            public const string CanAddRolesToBeManaged = "Permissions.Roles.CanAddRolesToBeManaged";
        }

        /// <summary>
        /// Provides a collection of constants representing permission strings for managing role claims.
        /// </summary>
        /// <remarks>These constants define the specific permissions required to perform various
        /// operations on role claims,  such as viewing, creating, editing, deleting, and searching. They can be used to
        /// enforce access control  in applications that implement role-based authorization.</remarks>
        [DisplayName("Role Claims"), Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        /// <summary>
        /// Provides a collection of permission constants related to audit trail operations.
        /// </summary>
        /// <remarks>This class defines string constants representing specific permissions for managing
        /// audit trails,  such as viewing, exporting, and searching audit trail data. These constants can be used to
        /// enforce  or check user permissions within an application.</remarks>
        [DisplayName("Audit Trails"), Description("Audit Trails Permissions")]
        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
    }
}
