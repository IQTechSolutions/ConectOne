using System.ComponentModel;

namespace AdvertisingModule.Domain.Constants
{
    /// <summary>
    /// Provides a centralized definition of permission constants for various application modules.
    /// </summary>
    /// <remarks>The <see cref="Permissions"/> class contains nested static classes that group permission
    /// constants  by module or feature area. Each constant represents a specific permission string that can be used 
    /// for access control within the application. These permissions are typically used in role-based  authorization
    /// systems to determine whether a user has the necessary rights to perform specific  actions. <para> For example,
    /// the <see cref="Advertisement"/> class defines permissions related to managing  advertisements, such as viewing,
    /// creating, editing, deleting, searching, and managing images.  Similarly, the <see cref="Affiliates"/> class
    /// defines permissions for managing affiliates. </para></remarks>
    public class Permissions
    {
        /// <summary>
        /// Provides a collection of constants representing permission names for advertisement-related operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions for managing
        /// advertisements.  These permissions can be used to control access to advertisement-related functionality,
        /// such as viewing,  creating, editing, deleting, searching, and managing advertisement images.</remarks>
        [DisplayName("Advertisement"), Description("Advertisement Permissions")]
        public static class Advertisement
        {
            public const string View = "Permissions.Advertisement.View";
            public const string Create = "Permissions.Advertisement.Create";
            public const string Edit = "Permissions.Advertisement.Edit";
            public const string Delete = "Permissions.Advertisement.Delete";
            public const string Search = "Permissions.Advertisement.Search";
            public const string Image = "Permissions.Advertisement.Image";
        }

        /// <summary>
        /// Provides a collection of permission constants related to affiliate management.
        /// </summary>
        /// <remarks>This class defines string constants representing specific permissions for operations 
        /// such as viewing, creating, editing, deleting, and searching affiliates. These constants  can be used to
        /// enforce or check access control in the application.</remarks>
        [DisplayName("Affiliates"), Description("Affiliates Permissions")]
        public static class Affiliates
        {
            public const string View = "Permissions.Affiliates.View";
            public const string Create = "Permissions.Affiliates.Create";
            public const string Edit = "Permissions.Affiliates.Edit";
            public const string Delete = "Permissions.Affiliates.Delete";
            public const string Search = "Permissions.Affiliates.Search";
        }

        /// <summary>
        /// Provides a collection of permission constants related to advertisement tier operations.
        /// </summary>
        /// <remarks>This static class defines string constants representing various permissions for
        /// managing advertisement tiers. These permissions can be used to control access to specific advertisement tier
        /// functionalities, such as viewing, creating, editing, deleting, searching, and managing images.</remarks>
        [DisplayName("AdvertisementTier"), Description("Advertisement Tier Permissions")]
        public static class AdvertisementTier
        {
            public const string View = "Permissions.AdvertisementTier.View";
            public const string Create = "Permissions.AdvertisementTier.Create";
            public const string Edit = "Permissions.AdvertisementTier.Edit";
            public const string Delete = "Permissions.AdvertisementTier.Delete";
            public const string Search = "Permissions.AdvertisementTier.Search";
            public const string Image = "Permissions.AdvertisementTier.Image";
        }

        /// <summary>
        /// Provides constants representing permissions for advertisement review operations.
        /// </summary>
        /// <remarks>This class defines string constants that represent specific permissions required for
        /// creating, editing,  and deleting advertisement reviews. These constants can be used to enforce or check
        /// access control  within the application.</remarks>
        [DisplayName("AdvertisementReview"), Description("Advertisement Review Permissions")]
        public static class AdvertisementReview
        {
            public const string Create = "Permissions.AdvertisementReview.Create";
            public const string Reject = "Permissions.AdvertisementReview.Reject";
            public const string Approve = "Permissions.AdvertisementReview.Approve";
        }
    }
}
