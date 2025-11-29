using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessModule.Blazor.Pages.BusinessListings
{
    /// <summary>
    /// Represents the reviews section of a business listing, including navigation breadcrumbs and user-specific data.
    /// </summary>
    /// <remarks>This class is designed to manage and display reviews for a specific user, identified by the
    /// <see cref="UserId"/> property.  It also provides breadcrumb navigation for the associated dashboard and review
    /// pages.</remarks>
    public partial class Reviews
    {
        /// <summary>
        /// Represents the default breadcrumb navigation items for the current view.
        /// </summary>
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new BreadcrumbItem("Business Listing Reviews", href: null, disabled: true, icon: Icons.Material.Filled.List)
        };

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }
    }
}
