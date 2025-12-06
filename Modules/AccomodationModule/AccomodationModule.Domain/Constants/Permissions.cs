using System.ComponentModel;

namespace AccomodationModule.Domain.Constants
{
    /// <summary>
    /// Provides a centralized definition of permission constants for various application features.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class contains nested static classes that group permission
    /// constants  by feature or domain, such as "Vacation Host", "Vacation", "Lodging", "Golf Courses", and
    /// "Destination".  Each nested class defines specific permissions (e.g., View, Create, Edit, Delete, Search) as
    /// string constants. These constants can be used to enforce access control and authorization throughout the
    /// application.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Provides a set of constants representing permission keys for managing vacation host operations.
        /// </summary>
        /// <remarks>This class defines permission keys that can be used to control access to various
        /// vacation host-related actions,  such as viewing, creating, editing, deleting, and searching. These constants
        /// are typically used in authorization  checks to ensure that users have the appropriate permissions for
        /// specific operations.</remarks>
        [DisplayName("Vacation Host"), Description("Vacation Host Permissions")]
        public static class VacationHost
        {
            public const string View = "Permissions.VacationHost.View";
            public const string Create = "Permissions.VacationHost.Create";
            public const string Edit = "Permissions.VacationHost.Edit";
            public const string Delete = "Permissions.VacationHost.Delete";
            public const string Search = "Permissions.VacationHost.Search";
            public const string Image = "Permissions.VacationHost.Image";
        }

        /// <summary>
        /// Provides constants representing permission keys for vacation-related operations.
        /// </summary>
        /// <remarks>This class defines string constants that can be used to check or assign permissions 
        /// for viewing, creating, editing, deleting, and searching vacation-related data.</remarks>
        [DisplayName("Vacation"), Description("Vacation Permissions")]
        public static class Vacation
        {
            public const string View = "Permissions.Vacation.View";
            public const string Create = "Permissions.Vacation.Create";
            public const string Edit = "Permissions.Vacation.Edit";
            public const string Delete = "Permissions.Vacation.Delete";
            public const string Search = "Permissions.Vacation.Search";
        }

        /// <summary>
        /// Provides constants representing permission keys for managing reviews.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions related to
        /// reviews,  such as viewing, creating, editing, deleting, and searching reviews. These constants can be  used
        /// to enforce or check permissions within an application.</remarks>
        [DisplayName("Reviews"), Description("Reviews Permissions")]
        public static class Reviews
        {
            public const string View = "Permissions.Reviews.View";
            public const string Create = "Permissions.Reviews.Create";
            public const string Edit = "Permissions.Reviews.Edit";
            public const string Delete = "Permissions.Reviews.Delete";
            public const string Search = "Permissions.Reviews.Search";
        }

        /// <summary>
        /// Provides a set of constants representing permission keys for lodging-related operations.
        /// </summary>
        /// <remarks>This class defines permission keys that can be used to control access to
        /// lodging-related functionality. Each constant represents a specific permission, such as viewing, creating,
        /// editing, deleting, or searching lodging records. These keys are typically used in authorization checks to
        /// ensure that users have the appropriate permissions for the requested operation.</remarks>
        [DisplayName("Lodging"), Description("Lodging Permissions")]
        public static class Lodging
        {
            public const string View = "Permissions.Lodging.View";
            public const string Create = "Permissions.Lodging.Create";
            public const string Edit = "Permissions.Lodging.Edit";
            public const string Delete = "Permissions.Lodging.Delete";
            public const string Search = "Permissions.Lodging.Search";
        }

        /// <summary>
        /// Provides a set of constants representing permission keys for lodging type operations.
        /// </summary>
        /// <remarks>These constants are used to define and check permissions for various actions related
        /// to lodging types, such as viewing, creating, editing, deleting, and searching. Each constant represents a
        /// specific permission key that can be used in access control mechanisms.</remarks>
        [DisplayName("Lodging Types"), Description("Lodging Type Permissions")]
        public static class LodgingTypes
        {
            public const string View = "Permissions.LodgingTypes.View";
            public const string Create = "Permissions.LodgingTypes.Create";
            public const string Edit = "Permissions.LodgingTypes.Edit";
            public const string Delete = "Permissions.LodgingTypes.Delete";
            public const string Search = "Permissions.LodgingTypes.Search";
        }

        [DisplayName("Restaurants"), Description("Restaurant Permissions")]
        public static class Restaurants
        {
            public const string View = "Permissions.Restaurants.View";
            public const string Create = "Permissions.Restaurants.Create";
            public const string Edit = "Permissions.Restaurants.Edit";
            public const string Delete = "Permissions.Restaurants.Delete";
            public const string Search = "Permissions.Restaurants.Search";
        }

        /// <summary>
        /// Provides permission constants for managing golf courses.
        /// </summary>
        /// <remarks>This class defines string constants representing various permissions related to golf
        /// course operations. These permissions can be used to control access to specific functionalities, such as
        /// viewing, creating, editing, deleting, or searching golf courses.</remarks>
        [DisplayName("Golf Courses"), Description("Golf Courses Permissions")]
        public static class GolfCourse
        {
            public const string View = "Permissions.GolfCourse.View";
            public const string Create = "Permissions.GolfCourse.Create";
            public const string Edit = "Permissions.GolfCourse.Edit";
            public const string Delete = "Permissions.GolfCourse.Delete";
            public const string Search = "Permissions.GolfCourse.Search";
        }

        /// <summary>
        /// Provides constants representing permission keys for destination-related operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions for viewing,
        /// creating,  editing, deleting, and searching destinations. These constants can be used to enforce or check 
        /// access control in applications.</remarks>
        [DisplayName("Destination"), Description("Destination Permissions")]
        public static class Destination
        {
            public const string View = "Permissions.Destination.View";
            public const string Create = "Permissions.Destination.Create";
            public const string Edit = "Permissions.Destination.Edit";
            public const string Delete = "Permissions.Destination.Delete";
            public const string Search = "Permissions.Destination.Search";
        }

        

        /// <summary>
        /// Provides constants representing permission keys for destination-related operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions for viewing,
        /// creating,  editing, deleting, and searching destinations. These constants can be used to enforce or check 
        /// access control in applications.</remarks>
        [DisplayName("Contact"), Description("Contact Permissions")]
        public static class Contact
        {
            public const string View = "Permissions.Contact.View";
            public const string Create = "Permissions.Contact.Create";
            public const string Edit = "Permissions.Contact.Edit";
            public const string Delete = "Permissions.Contact.Delete";
            public const string Search = "Permissions.Contact.Search";
        }

        /// <summary>
        /// Provides constants representing permission keys for destination-related operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions for viewing,
        /// creating,  editing, deleting, and searching destinations. These constants can be used to enforce or check 
        /// access control in applications.</remarks>
        [DisplayName("Booking"), Description("Booking Permissions")]
        public static class Booking
        {
            public const string View = "Permissions.Booking.View";
            public const string Create = "Permissions.Booking.Create";
            public const string Edit = "Permissions.Booking.Edit";
            public const string Delete = "Permissions.Booking.Delete";
            public const string Search = "Permissions.Booking.Search";
        }

        /// <summary>
        /// Provides constants representing permission strings for extension-related operations.
        /// </summary>
        /// <remarks>These constants can be used to define or check permissions for various
        /// extension-related actions,  such as viewing, creating, editing, deleting, or searching extensions.</remarks>
        [DisplayName("Extensions"), Description("Extension Permissions")]
        public static class Extensions
        {
            public const string View = "Permissions.Extensions.View";
            public const string Create = "Permissions.Extensions.Create";
            public const string Edit = "Permissions.Extensions.Edit";
            public const string Delete = "Permissions.Extensions.Delete";
            public const string Search = "Permissions.Extensions.Search";
        }

        /// <summary>
        /// Provides permission constants for managing and interacting with tour guide schedules.
        /// </summary>
        /// <remarks>This class defines string constants representing specific permissions related to tour
        /// guide schedules. These permissions can be used to control access to viewing and searching tour guide
        /// schedules.</remarks>
        [DisplayName("Tour Guid Schedule"), Description("Tour Guid Schedule Permissions")]
        public static class TourGuidSchedule
        {
            public const string View = "Permissions.TourGuidSchedule.View";
            public const string Search = "Permissions.TourGuidSchedule.Search";
        }

        /// <summary>
        /// Provides a set of permission constants for managing template-related operations.
        /// </summary>
        /// <remarks>Use these constants to represent specific access rights when authorizing actions such
        /// as viewing, creating, editing, deleting, or searching templates. These values are typically used in
        /// role-based access control systems to check or assign permissions for users.</remarks>
        [DisplayName("Templates"), Description("Template Permissions")]
        public static class Templates
        {
            public const string View = "Permissions.Templates.View";
            public const string Create = "Permissions.Templates.Create";
            public const string Edit = "Permissions.Templates.Edit";
            public const string Delete = "Permissions.Templates.Delete";
            public const string Search = "Permissions.Templates.Search";
        }

        /// <summary>
        /// Provides constant permission names for operations related to application settings.
        /// </summary>
        /// <remarks>Use the constants defined in this class to reference specific permissions when
        /// implementing authorization checks for viewing, creating, editing, deleting, or searching settings. This
        /// class is intended to centralize permission strings to ensure consistency across the application.</remarks>
        [DisplayName("Settings"), Description("Settings Permissions")]
        public static class Settings
        {
            public const string View = "Permissions.Settings.View";
            public const string Create = "Permissions.Settings.Create";
            public const string Edit = "Permissions.Settings.Edit";
            public const string Delete = "Permissions.Settings.Delete";
            public const string Search = "Permissions.Settings.Search";
        }
    }
}
