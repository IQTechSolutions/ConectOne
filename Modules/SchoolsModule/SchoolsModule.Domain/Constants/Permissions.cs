using System.ComponentModel;

namespace SchoolsModule.Domain.Constants
{
    /// <summary>
    /// Represents a collection of predefined permission constants organized by domain.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class provides a structured way to define and manage permission
    /// strings  for various application domains, such as learners, age groups, school grades, and more. Each nested 
    /// static class corresponds to a specific domain and contains constants representing specific actions  (e.g.,
    /// Create, Edit, Delete, View) that can be performed within that domain.  These permission strings are typically
    /// used in role-based access control (RBAC) systems to enforce  authorization rules. For example, a user with the
    /// "Permissions.Learner.Create" permission would be  allowed to create learner records.  The class is designed to
    /// be extensible, allowing developers to add new permission categories or actions  as needed.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Provides a set of constants representing permissions related to learner operations.
        /// </summary>
        /// <remarks>These permissions are used to define and enforce access control for learner-related
        /// actions such as creating, editing, deleting, and viewing learners. Each constant represents a specific
        /// permission string that can be used in authorization checks.</remarks>
        [DisplayName("Learner"), Description("Learner Permissions")]
        public static class LearnerPermissions
        {
            public const string Create = "Permissions.Learner.Create";
            public const string Edit = "Permissions.Learner.Edit";
            public const string Delete = "Permissions.Learner.Delete";
            public const string View = "Permissions.Learner.View";
            public const string SendMessage = "Permissions.Learner.SendMessage";
            public const string CreateChat = "Permissions.Learner.CreateChat";
            public const string AddParent = "Permissions.Learner.AddParent";
        }

        /// <summary>
        /// Defines a set of permission constants related to parent operations.
        /// </summary>
        /// <remarks>This class provides string constants representing specific permissions for
        /// parent-related actions. These constants can be used to enforce or check access control in the
        /// application.</remarks>
        [DisplayName("Parent"), Description("Parent Permissions")]
        public static class ParentPermissions
        {
            public const string Create = "Permissions.Parent.Create";
            public const string Edit = "Permissions.Parent.Edit";
            public const string Delete = "Permissions.Parent.Delete";
            public const string Export = "Permissions.Parent.Export";
            public const string View = "Permissions.Parent.View";
            public const string AddLeaner = "Permissions.Learner.AddLeaner";
        }

        /// <summary>
        /// Defines a set of permission constants for managing teacher-related operations.
        /// </summary>
        /// <remarks>These permissions are used to control access to specific teacher-related actions,
        /// such as creating, editing, deleting, and viewing teacher records.  Each permission is represented as a
        /// string constant that can be used in authorization checks.</remarks>
        [DisplayName("Teacher"), Description("Teacher Permissions")]
        public static class TeacherPermissions
        {
            public const string Create = "Permissions.Teacher.Create";
            public const string Edit = "Permissions.Teacher.Edit";
            public const string Delete = "Permissions.Teacher.Delete";
            public const string View = "Permissions.Teacher.View";
        }

        /// <summary>
        /// Defines a set of constants representing permissions for event-related operations.
        /// </summary>
        /// <remarks>These permissions are used to control access to event-related functionality within
        /// the application. Each constant represents a specific permission that can be assigned to users or
        /// roles.</remarks>
        [DisplayName("Event"), Description("Event Permissions")]
        public static class EventPermissions
        {
            public const string Create = "Permissions.Event.Create";
            public const string Edit = "Permissions.Event.Edit";
            public const string Delete = "Permissions.Event.Delete";
            public const string View = "Permissions.Event.View";
            public const string Export = "Permissions.Event.Export";
            public const string SendNotification = "Permissions.Event.SendNotification";
            public const string CreateMessage = "Permissions.Event.CreateMessage";
        }

        /// <summary>
        /// Defines a set of constants representing permissions related to activities.
        /// </summary>
        /// <remarks>These permissions are used to control access to various activity-related operations,
        /// such as creating, editing, deleting, and viewing activities. Each constant represents a specific permission
        /// string that can be used in authorization checks.</remarks>
        [DisplayName("Activity"), Description("Activity Permissions")]
        public static class ActivityPermissions
        {
            public const string Create = "Permissions.Activity.Create";
            public const string Edit = "Permissions.Activity.Edit";
            public const string Delete = "Permissions.Activity.Delete";
            public const string View = "Permissions.Activity.View";
            public const string Search = "Permissions.Activity.CanLinkGroups";
            public const string LinkGroups = "Permissions.Activity.LinkGroups";
            public const string LinkTeams = "Permissions.Activity.LinkTeams";
            public const string LinkTeamMembers = "Permissions.Activity.LinkTeamMembers";
            public const string Export = "Permissions.Activity.Export";
        }

        /// <summary>
        /// Defines permission constants for managing activity categories.
        /// </summary>
        /// <remarks>These constants represent specific permissions that can be used to control access to 
        /// create, edit, delete, and view activity categories within the application. They are  typically used in
        /// role-based access control (RBAC) systems to enforce security policies.</remarks>
        [DisplayName("ActivityCategory"), Description("Activity Category Permissions")]
        public static class ActivityCategoryPermissions
        {
            public const string Create = "Permissions.ActivityCategory.Create";
            public const string Edit = "Permissions.ActivityCategory.Edit";
            public const string Delete = "Permissions.ActivityCategory.Delete";
            public const string View = "Permissions.ActivityCategory.View";
            public const string Search = "Permissions.ActivityCategory.CanLinkGroups";
        }

        /// <summary>
        /// Defines a set of permissions related to activity team operations.
        /// </summary>
        /// <remarks>This class provides constants representing specific permissions for creating,
        /// editing, deleting,  and viewing activity team data. These constants can be used to enforce or check access
        /// control  within the application.</remarks>
        [DisplayName("ActivityTeam"), Description("Activity Team Permissions")]
        public static class ActivityTeamPermissions
        {
            public const string Create = "Permissions.ActivityTeam.Create";
            public const string Edit = "Permissions.ActivityTeam.Edit";
            public const string Delete = "Permissions.ActivityTeam.Delete";
            public const string View = "Permissions.ActivityTeam.View";
        }

        /// <summary>
        /// Provides a collection of constants representing permissions for system data operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions for creating,
        /// editing, deleting,  and viewing system data. These constants can be used to enforce or check access control
        /// in applications  that implement role-based or permission-based security.</remarks>
        [DisplayName("System Data"), Description("System Data Permissions")]
        public static class SystemDataPermissions
        {
            public const string Create = "Permissions.SystemData.Create";
            public const string Edit = "Permissions.SystemData.Edit";
            public const string Delete = "Permissions.SystemData.Delete";
            public const string View = "Permissions.SystemData.View";
        }
    }
}
