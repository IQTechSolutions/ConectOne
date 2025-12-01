using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using ProductsModule.Domain.Entities;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile;

/// <summary>
/// Represents a component that facilitates the selection of service tiers for upgrading an account,  based on the roles
/// associated with a specific user.
/// </summary>
/// <remarks>This component retrieves the roles associated with the specified user and the available service tiers
/// for each role. It uses dependency-injected services to fetch data and manages navigation within the
/// application.</remarks>
public partial class UpgradeAccountTierSelection
{
    private Dictionary<string, List<ServiceTier>> _serviceTiersByRole = new();
    private IEnumerable<RoleDto> _roles = [];

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IBaseHttpProvider Provider { get; set; }

    /// <summary>
    /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection in Blazor applications. Use it to
    /// programmatically navigate to different URIs or to access information about the current navigation
    /// state.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [Parameter] public string? UserId { get; set; }

    /// <summary>
    /// Navigates the application to the specified URL.
    /// </summary>
    /// <remarks>This method uses the <see cref="NavigationManager"/> to perform the navigation. Ensure that
    /// the URL is properly formatted and accessible.</remarks>
    /// <param name="url">The URL to navigate to. This must be a valid, non-null, and non-empty string.</param>
    private void NavigateToPage(string url)
    {
        NavigationManager.NavigateTo(url);
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves user roles and their associated service tiers.
    /// </summary>
    /// <remarks>This method fetches the roles associated with the current user and, for each role, retrieves
    /// the corresponding service tiers. The retrieved data is stored for use within the component. The base
    /// implementation of <see cref="OnInitializedAsync"/> is invoked after the data is loaded.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var rolesResult = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles/user/{UserId}");
        if (rolesResult.Succeeded)
        {
            _roles = rolesResult.Data;
            foreach (var role in _roles)
            {
                var serviceTierResult = await Provider.GetAsync<List<ServiceTier>>($"service-tiers/roles/{role.Id}");
                if (serviceTierResult.Succeeded && serviceTierResult.Data.Any())
                    _serviceTiersByRole[role.Id] = serviceTierResult.Data;
            }
        }

        await base.OnInitializedAsync();
    }
}