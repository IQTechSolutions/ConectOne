using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.AdvertisementTiers;

/// <summary>
/// Represents a component that displays and manages a list of affiliates, including functionality for loading,
/// displaying, and interacting with affiliate data.
/// </summary>
/// <remarks>This component provides server-side data loading, table management, and user interactions such as
/// deleting affiliates. It integrates with various services, including HTTP providers, dialog services, and
/// notifications, to facilitate these operations.</remarks>
public partial class List
{
   private readonly List<BreadcrumbItem> _items = new()
    {
        new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new BreadcrumbItem("Advertisement Tiers", href: null, disabled: true, icon: Icons.Material.Filled.List)
    };
}
