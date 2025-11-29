using System.ComponentModel;

namespace BusinessModule.Domain.Constants
{
    /// <summary>
    /// Represents a centralized collection of permission constants used for access control in various application
    /// domains.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class provides nested static classes, each defining a set of
    /// string constants that represent specific permissions for different functional areas of the application. These
    /// permissions are typically used in role-based access control (RBAC) systems to determine whether a user or role
    /// is authorized to perform specific operations, such as creating, editing, deleting, viewing, or searching
    /// resources.  Each nested class corresponds to a distinct domain, such as advertisements, business listings, or
    /// affiliates, and contains constants that represent the permissions required for operations within that domain.
    /// For example, the <see cref="AdvertisementPermissions"/> class defines permissions for managing advertisements,
    /// while the <see cref="BusinessListingPermissions"/> class defines permissions for managing business listings. 
    /// These constants are intended to be used as keys in access control checks, ensuring that the application enforces
    /// the appropriate level of security for each operation.</remarks>
    public class Permissions
    {
        /// <summary>
        /// Defines a set of permissions related to advertisement operations.
        /// </summary>
        /// <remarks>This class provides constants representing specific permissions for creating,
        /// editing, deleting,  viewing, and searching advertisements. These permissions are typically used in access
        /// control  mechanisms to determine whether a user or role is authorized to perform the corresponding
        /// actions.</remarks>
        [DisplayName("Advertisement"), Description("Advertisement Permissions")]
        public static class AdvertisementPermissions
        {
            public const string Create = "Permissions.Advertisement.Create";
            public const string Edit = "Permissions.Advertisement.Edit";
            public const string Delete = "Permissions.Advertisement.Delete";
            public const string View = "Permissions.Advertisement.View";
            public const string Search = "Permissions.Advertisement.Search";
        }

        /// <summary>
        /// Defines a set of permissions related to advertisement review operations.
        /// </summary>
        /// <remarks>This class provides constants representing specific permissions for creating,
        /// editing, deleting,  viewing, and searching advertisement reviews. These permissions are typically used to
        /// enforce  access control in systems that manage advertisement reviews.</remarks>
        [DisplayName("AdvertisementReview"), Description("Advertisement Review Permissions")]
        public static class AdvertisementReviewPermissions
        {
            public const string Create = "Permissions.AdvertisementReview.Create";
            public const string Edit = "Permissions.AdvertisementReview.Edit";
            public const string Delete = "Permissions.AdvertisementReview.Delete";
            public const string View = "Permissions.AdvertisementReview.View";
            public const string Search = "Permissions.AdvertisementReview.Search";
        }

        /// <summary>
        /// Defines a set of permissions related to advertisement tiers.
        /// </summary>
        /// <remarks>This static class provides constants representing specific permissions for managing
        /// advertisement tiers.  These permissions can be used to control access to operations such as creating,
        /// editing, deleting, viewing,  and searching advertisement tiers within the application.</remarks>
        [DisplayName("AdvertisementTier"), Description("Advertisement Tier Permissions")]
        public static class AdvertisementTierPermissions
        {
            public const string Create = "Permissions.AdvertisementTier.Create";
            public const string Edit = "Permissions.AdvertisementTier.Edit";
            public const string Delete = "Permissions.AdvertisementTier.Delete";
            public const string View = "Permissions.AdvertisementTier.View";
            public const string Search = "Permissions.AdvertisementTier.Search";
        }

        /// <summary>
        /// Defines a set of permissions related to managing business listings.
        /// </summary>
        /// <remarks>This class provides constants representing specific permissions for creating,
        /// editing, deleting,  viewing, and searching business listings. These permissions are typically used in access
        /// control  scenarios to determine whether a user or role has the necessary rights to perform a given
        /// operation.</remarks>
        [DisplayName("BusinessListing"), Description("Business Listing Permissions")]
        public static class BusinessListingPermissions
        {
            public const string Create = "Permissions.BusinessListing.Create";
            public const string Edit = "Permissions.BusinessListing.Edit";
            public const string Delete = "Permissions.BusinessListing.Delete";
            public const string View = "Permissions.BusinessListing.View";
            public const string Search = "Permissions.BusinessListing.Search";
        }

        /// <summary>
        /// Provides a collection of constants representing permission keys for operations  within the Business Tier.
        /// These keys are used to define and enforce access control  for creating, editing, deleting, viewing, and
        /// searching business tier resources.
        /// </summary>
        /// <remarks>Each constant in this class represents a specific permission key that can be used  in
        /// role-based access control (RBAC) systems or similar authorization mechanisms.  These keys are typically used
        /// to check whether a user or role has the necessary  permissions to perform a specific operation.</remarks>
        [DisplayName("BusinessTier"), Description("Business Tier Permissions")]
        public static class BusinessTierPermissions
        {
            public const string Create = "Permissions.BusinessTier.Create";
            public const string Edit = "Permissions.BusinessTier.Edit";
            public const string Delete = "Permissions.BusinessTier.Delete";
            public const string View = "Permissions.BusinessTier.View";
            public const string Search = "Permissions.BusinessTier.Search";
        }

        /// <summary>
        /// Provides a collection of permission constants for managing business reviews.
        /// </summary>
        /// <remarks>These permissions represent the actions that can be performed on business reviews,
        /// such as creating, editing, deleting, viewing, and searching. Each permission is defined as a string constant
        /// and can be used for access control in the application.</remarks>
        [DisplayName("BusinessReview"), Description("Business Review Permissions")]
        public static class BusinessReviewPermissions
        {
            public const string Create = "Permissions.BusinessReview.Create";
            public const string Edit = "Permissions.BusinessReview.Edit";
            public const string Delete = "Permissions.BusinessReview.Delete";
            public const string View = "Permissions.BusinessReview.View";
            public const string Search = "Permissions.BusinessReview.Search";
        }

        /// <summary>
        /// Provides a collection of permission constants related to business category operations.
        /// </summary>
        /// <remarks>This class defines string constants representing specific permissions for creating,
        /// editing, deleting,  viewing, and searching business categories. These constants can be used to enforce or
        /// check access  control in applications.</remarks>
        [DisplayName("BusinessCategory"), Description("Business Category Permissions")]
        public static class BusinessCategoryPermissions
        {
            public const string Create = "Permissions.BusinessCategory.Create";
            public const string Edit = "Permissions.BusinessCategory.Edit";
            public const string Delete = "Permissions.BusinessCategory.Delete";
            public const string View = "Permissions.BusinessCategory.View";
            public const string Search = "Permissions.BusinessCategory.Search";
            public const string SubCategory = "Permissions.BusinessCategory.SubCategory";
        }

        /// <summary>
        /// Provides a collection of constants representing the permissions available for affiliate-related operations.
        /// </summary>
        /// <remarks>These permissions are used to define and enforce access control for various affiliate
        /// management actions. Each constant represents a specific operation that can be performed within the affiliate
        /// domain.</remarks>
        [DisplayName("Affiliate"), Description("Affiliate Permissions")]
        public static class AffiliatePermissions
        {
            public const string Create = "Permissions.Affiliate.Create";
            public const string Edit = "Permissions.Affiliate.Edit";
            public const string Delete = "Permissions.Affiliate.Delete";
            public const string View = "Permissions.Affiliate.View";
            public const string Search = "Permissions.Affiliate.Search";
        }
    }
}
