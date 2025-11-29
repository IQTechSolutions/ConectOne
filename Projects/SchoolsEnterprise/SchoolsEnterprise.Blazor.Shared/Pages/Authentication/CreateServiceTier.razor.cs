using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using SchoolsEnterprise.Blazor.Shared.Pages.Authentication.Modals;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents a component for creating and managing a service tier within the application.
    /// </summary>
    /// <remarks>This class provides functionality to add services to a service tier, create the service tier,
    /// and navigate to appropriate views based on user actions. It relies on dependency injection  for services such as
    /// navigation, dialogs, HTTP communication, and notifications.</remarks>
    public partial class CreateServiceTier
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Roles", href: $"/security/roles", icon: Icons.Material.Filled.List),
            new("Create Role Service", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in a Blazor application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider is typically injected and used to abstract HTTP communication, allowing
        /// for easier testing and dependency management.</remarks>
        [Inject] public IServiceTierService ServiceTierService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the role.
        /// </summary>
        [Parameter] public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role associated with the current context.
        /// </summary>
        [Parameter] public string RoleName { get; set; }

        /// <summary>
        /// Represents the current service tier configuration for the application.
        /// </summary>
        /// <remarks>This field is initialized with a new instance of <see cref="ServiceTierViewModel"/>.
        /// It is intended to store the state or settings related to the service tier.</remarks>
        public ServiceTierViewModel _serviceTier = new();

        #region Methods

        /// <summary>
        /// Displays a dialog for adding a service to the current service tier and updates the service tier  with the
        /// selected service if the operation is confirmed.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to add a new service to the
        /// current service tier. If the user confirms the operation, the selected service is added to the service
        /// tier's collection. If the operation is canceled, no changes are made.</remarks>
        /// <returns></returns>
        public async Task AddServiceTierService()
        {
            var parameters = new DialogParameters<AddServiceTierServiceModal>
            {
                { x => x.ServiceTier, _serviceTier }
            };

            var dialog = await DialogService.ShowAsync<AddServiceTierServiceModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (ServiceDto)result.Data;
                _serviceTier.Services.Add(model);
            }
        }

        /// <summary>
        /// Creates a new service tier asynchronously and processes the result.
        /// </summary>
        /// <remarks>This method sends a request to create a new service tier using the provided data.  If
        /// the operation is successful, it navigates to the settings page for the specified role.</remarks>
        /// <returns></returns>
        public async Task CreateAsync()
        {
            _serviceTier.RoleId = RoleId;
            var creationResult = await ServiceTierService.CreateAsync(_serviceTier.ToDto());
            creationResult.ProcessResponseForDisplay(Snackbar, () =>
            {
                NavigationManager.NavigateTo($"/userroles/settings/{RoleName}");
            });
        }

        /// <summary>
        /// Cancels the current operation and navigates to the service tiers page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/service-tiers" route. Ensure that the  <see
        /// cref="NavigationManager"/> is properly configured and the target route exists  in the application.</remarks>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/service-tiers");
        }

        #endregion
    }
}
