using System.ComponentModel;

namespace BloggingModule.Domain.Constants
{
    /// <summary>
    /// Represents a collection of permission constants used for access control in blog-related operations.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class contains nested static classes that define string
    /// constants representing specific permissions for various blog functionalities, such as managing blogs and blog
    /// categories. These constants are typically used in authorization checks to determine whether a user has the
    /// necessary permissions to perform specific actions. Each nested class groups permissions by functionality, making
    /// it easier to manage and organize access control in applications.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Defines a set of constants representing permission names for blog-related operations.
        /// </summary>
        /// <remarks>These permissions are used to control access to various blog functionalities, such as
        /// creating, editing, deleting, viewing, and searching blogs.  Each constant represents a specific permission
        /// identifier that can be used in authorization checks.</remarks>
        [DisplayName("Blog"), Description("Blog Permissions")]
        public static class BlogPermissions
        {
            public const string Create = "Permissions.Blog.Create";
            public const string Edit = "Permissions.Blog.Edit";
            public const string Delete = "Permissions.Blog.Delete";
            public const string View = "Permissions.Blog.View";
            public const string Search = "Permissions.Blog.Search";
        }

        /// <summary>
        /// Defines a set of permission constants for managing blog categories.
        /// </summary>
        /// <remarks>These permissions represent the actions that can be performed on blog categories,
        /// such as creating, editing, deleting, viewing, and searching. Each permission is represented as a string
        /// constant, which can be used for authorization checks in the application.</remarks>
        [DisplayName("BlogCategory"), Description("Blog Category Permissions")]
        public static class BlogCategoryPermissions
        {
            public const string Create = "Permissions.BlogCategory.Create";
            public const string Edit = "Permissions.BlogCategory.Edit";
            public const string Delete = "Permissions.BlogCategory.Delete";
            public const string View = "Permissions.BlogCategory.View";
            public const string Search = "Permissions.BlogCategory.Search";
        }
    }
}
