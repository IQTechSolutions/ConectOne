using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProductsModule.Blazor.Pages.Products;

/// <summary>
/// Represents a component that displays and manages a paginated list of products.
/// </summary>
/// <remarks>This component provides functionality for displaying, sorting, and managing a list of products in a
/// table format. It supports server-side data loading, pagination, and user actions such as creating, editing, and
/// deleting products. The component also integrates with authentication and authorization services to determine user
/// permissions for these actions.</remarks>
public partial class List
{
    private List<RoleDto> _shopOwners = new();

    /// <summary>
    /// Represents the collection of breadcrumb items used for navigation.
    /// </summary>
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Products", href: null, disabled: true, icon: Icons.Material.Filled.List)
    ];

    /// <summary>
    /// Gets or sets the service used to manage user roles within the application.
    /// </summary>
    [Inject] public IRoleService RoleService { get; set; } = null!;

    /// <summary>
    /// Asynchronously initializes the component and loads the list of shop owners based on product manager roles.
    /// </summary>
    /// <remarks>This method is called by the Blazor framework during component initialization. It retrieves
    /// product manager roles and populates the shop owners collection accordingly. Override this method to perform
    /// additional asynchronous setup when the component is initialized.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var productManagers = await RoleService.ProductManagers();
        if (productManagers.Succeeded && productManagers.Data.Any())
        {
            foreach (var manager in productManagers.Data)
            {
                if (manager.ChildRoles.Any())
                {
                    _shopOwners.AddRange(manager.ChildRoles);
                }
                else
                {
                    _shopOwners.Add(manager);
                }
            }
        }
    }
}