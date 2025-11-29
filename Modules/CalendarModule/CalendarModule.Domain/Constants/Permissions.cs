using System.ComponentModel;

namespace CalendarModule.Domain.Constants
{
    /// <summary>
    /// Represents a collection of predefined permission constants organized by category.
    /// </summary>
    /// <remarks>This class provides a structured way to define and access permission strings used for
    /// authorization purposes. Each nested static class groups related permissions, making it easier to manage and
    /// reference them in the application.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Provides a set of constants representing permissions related to calendar operations.
        /// </summary>
        /// <remarks>These permissions are used to define access control for creating, editing, and
        /// deleting calendar entries. Each constant represents a specific permission string that can be used in
        /// authorization checks.</remarks>
        [DisplayName("Calendar"), Description("Calendar Permissions")]
        public static class CalendarPermissions
        {
            public const string Create = "Permissions.Calendar.Create";
            public const string Edit = "Permissions.Calendar.Edit";
            public const string Delete = "Permissions.Calendar.Delete";
            public const string View = "Permissions.Calendar.View";
            public const string Search = "Permissions.Calendar.Search";
        }
    }
}
