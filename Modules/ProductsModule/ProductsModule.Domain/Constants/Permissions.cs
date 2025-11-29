using System.ComponentModel;

namespace ProductsModule.Domain.Constants;

/// <summary>
/// Provides a centralized definition of permission constants for product related features.
/// </summary>
public class Permissions
{
    /// <summary>
    /// Provides a collection of permission constants for managing product categories.
    /// </summary>
    /// <remarks>This static class defines string constants representing various permissions related to
    /// product category operations. These permissions can be used to enforce access control in applications.</remarks>
    [DisplayName("Product Categories"), Description("Product Category Permissions")]
    public static class ProductCategories
    {
        public const string View = "Permissions.ProductCategories.View";
        public const string Create = "Permissions.ProductCategories.Create";
        public const string Edit = "Permissions.ProductCategories.Edit";
        public const string Delete = "Permissions.ProductCategories.Delete";
        public const string Search = "Permissions.ProductCategories.Search";
    }

    /// <summary>
    /// Provides a collection of constants representing permission names for product-related operations.
    /// </summary>
    /// <remarks>This class defines string constants that represent specific permissions for managing
    /// products.  These permissions can be used to enforce access control in applications, such as determining  whether
    /// a user is authorized to view, create, edit, delete, or search for products.</remarks>
    [DisplayName("Products"), Description("Product Permissions")]
    public static class Products
    {
        public const string View = "Permissions.Products.View";
        public const string Create = "Permissions.Products.Create";
        public const string Edit = "Permissions.Products.Edit";
        public const string Delete = "Permissions.Products.Delete";
        public const string Search = "Permissions.Products.Search";
    }

    /// <summary>
    /// Provides a collection of constants representing permission keys for service-related operations.
    /// </summary>
    /// <remarks>These constants define the permission keys used to control access to various service-related
    /// actions,  such as viewing, creating, editing, deleting, and searching services. They can be used in
    /// authorization  checks to ensure that users have the appropriate permissions for specific operations.</remarks>
    [DisplayName("Services"), Description("Services Permissions")]
    public static class Services
    {
        public const string View = "Permissions.Services.View";
        public const string Create = "Permissions.Services.Create";
        public const string Edit = "Permissions.Services.Edit";
        public const string Delete = "Permissions.Services.Delete";
        public const string Search = "Permissions.Services.Search";
    }
}
