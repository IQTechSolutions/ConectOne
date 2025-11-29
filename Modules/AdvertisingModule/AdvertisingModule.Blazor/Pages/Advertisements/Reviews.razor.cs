using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Advertisements
{
    /// <summary>
    /// Represents a collection of advertisement reviews, including navigation metadata for breadcrumb display.
    /// </summary>
    /// <remarks>This class provides functionality for managing advertisement reviews and includes predefined
    /// breadcrumb navigation items. The breadcrumb items include links to the dashboard and the advertisement reviews
    /// section.</remarks>
    public partial class Reviews
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new BreadcrumbItem("Advertisement Reviews", href: null, disabled: true, icon: Icons.Material.Filled.List)
        };

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }
    }
}
