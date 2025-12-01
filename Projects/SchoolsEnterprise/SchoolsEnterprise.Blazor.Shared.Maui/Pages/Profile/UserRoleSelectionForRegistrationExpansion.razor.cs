using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile
{
    /// <summary>
    /// Represents a Blazor component that manages user role selection during registration, including retrieving
    /// available roles and handling navigation.
    /// </summary>
    /// <remarks>This component interacts with an API to fetch role data and provides functionality for
    /// navigating to specific pages based on user actions. It is designed to be used within the context of a Blazor
    /// application.</remarks>
    public partial class UserRoleSelectionForRegistrationExpansion
    {
        public bool _loaded;
        private List<RoleDto> _availableRoles = [];
        private Dictionary<string, List<ServiceTierDto>> _serviceTiersByRole = new();

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically injected by the Blazor
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Called by the Blazor runtime when the component initializes.
        /// Simulates a loading/data retrieval process via Task.Delay.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var response = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles");
            response.ProcessResponseForDisplay(SnackBar, () =>
            {
                _availableRoles = response.Data.Where(c => c.AdvertiseRegistration).ToList();
            });
            _loaded = true;
        }

        /// <summary>
        /// Navigates the user back to the commerce index page.
        /// </summary>
        public async Task NavigateToCategoryPage(string roleId)
        {
            var serviceTierResult = await Provider.GetAsync<List<ServiceTierDto>>($"service-tiers/roles/{roleId}");
            if (serviceTierResult.Succeeded && serviceTierResult.Data.Any())
            {
                _serviceTiersByRole.Add(roleId, serviceTierResult.Data);

                if(_serviceTiersByRole.Count > 1)
                {
                    NavigationManager.NavigateTo($"/account/roles/tiers/{_serviceTiersByRole.FirstOrDefault(c => c.Value.Count > 1).Key}");
                }
                else
                {
                    NavigationManager.NavigateTo($"/account/roles/tiers/{ _serviceTiersByRole.FirstOrDefault().Key}"); 
                }
            }
        }
    }
}
