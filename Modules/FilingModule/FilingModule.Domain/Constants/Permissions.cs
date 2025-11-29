using System.ComponentModel;

namespace FilingModule.Application.Constants
{
    /// <summary>
    /// Provides a centralized definition of permission constants for various application features.
    /// </summary>
    /// <remarks>This class contains nested static classes that group related permissions by feature or
    /// module. Each permission is represented as a constant string, which can be used for authorization checks
    /// throughout the application.</remarks>
    public class Permissions
    {
        [DisplayName("Gallery")]
        [Description("Gallery Permissions")]
        public static class Gallery
        {
            public const string View = "Permissions.Gallery.View";
            public const string Create = "Permissions.Gallery.Create";
            public const string Edit = "Permissions.Gallery.Edit";
            public const string Delete = "Permissions.Gallery.Delete";
            public const string Search = "Permissions.Gallery.Search";
        }
    }
}
