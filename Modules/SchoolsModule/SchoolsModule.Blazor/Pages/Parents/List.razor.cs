using MudBlazor;

namespace SchoolsModule.Blazor.Pages.Parents
{
    /// <summary>
    /// Represents a Blazor page for listing and managing parent records within the Schools Module.
    /// Provides breadcrumb navigation, potentially linking back to a dashboard or other relevant areas.
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// A collection of breadcrumb items for UI navigation, indicating the user's path:
        /// e.g., Dashboard -> Parents.
        /// </summary>
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Parents", href: "#", icon: Icons.Material.Filled.PeopleAlt)
        ];
    }
}
