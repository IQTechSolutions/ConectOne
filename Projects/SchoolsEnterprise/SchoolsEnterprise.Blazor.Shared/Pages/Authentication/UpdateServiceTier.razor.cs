using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using SchoolsEnterprise.Blazor.Shared.Pages.Authentication.Modals;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents a component or service responsible for managing and updating the service tier configuration within
    /// the application.
    /// </summary>
    /// <remarks>This class provides functionality to manage service tiers, including adding services to a
    /// tier, creating new service tiers, and navigating between related views. It relies on injected services such as
    /// <see cref="NavigationManager"/>, <see cref="IDialogService"/>, <see cref="IBaseHttpProvider"/>, and <see
    /// cref="ISnackbar"/> to perform its operations. Ensure that all dependencies are properly configured and injected
    /// before using this class.</remarks>
    public partial class UpdateServiceTier
    {
        public bool _loaded;
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new BreadcrumbItem("Roles", href: $"/security/roles", icon: Icons.Material.Filled.List),
            new BreadcrumbItem("Update Role Service", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
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
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing service tier operations.
        /// </summary>
        [Inject] public IServiceTierService ServiceTierService { get; set; }

        /// <summary>
        /// Gets or sets the name of the role associated with the current context.
        /// </summary>
        [Parameter] public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the service tier.
        /// </summary>
        [Parameter] public string ServiceTierId { get; set; }

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

                var createServieResult = await ServiceTierService.CreateServiceTierServiceAsync(model.Id, ServiceTierId);
                createServieResult.ProcessResponseForDisplay(Snackbar, () =>
                {
                    _serviceTier.Services.Add(model);
                });
            }
        }

        /// <summary>
        /// Removes a service tier service from the application after user confirmation.
        /// </summary>
        /// <remarks>This method prompts the user for confirmation before proceeding with the removal. If
        /// the deletion is successful, the service is removed from the local collection. If the deletion fails, error
        /// messages are displayed to the user.</remarks>
        /// <param name="dto">The data transfer object representing the service tier service to be removed. The <see
        /// cref="ServiceDto.Id"/> property is used to identify the service to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RemoveServiceTierService(ServiceDto dto)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this parent from this application?"))
            {
                var deleteResult = await ServiceTierService.DeleteAsync(dto.Id);
                if (deleteResult.Succeeded)
                {
                    var createServieResult = await ServiceTierService.RemoveServiceTierServiceAsync(dto.Id, ServiceTierId);
                    createServieResult.ProcessResponseForDisplay(Snackbar, () =>
                    {
                        _serviceTier.Services.Remove(_serviceTier.Services.FirstOrDefault(c => c.Id == dto.Id));
                    });
                }
                else
                    Snackbar.AddErrors(deleteResult.Messages);
            }
        }

        /// <summary>
        /// Creates a new service tier asynchronously and processes the result.
        /// </summary>
        /// <remarks>This method sends a request to create a new service tier using the provided data.  If
        /// the operation is successful, it navigates to the settings page for the specified role.</remarks>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var creationResult = await ServiceTierService.UpdateAsync(_serviceTier.ToDto());
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

        /// <summary>
        /// Asynchronously initializes the component and loads the service tier data.
        /// </summary>
        /// <remarks>This method retrieves the service tier data associated with the specified
        /// <c>ServiceTierId</c>  and processes the response for display. It also sets the component's loaded state to
        /// indicate  that initialization is complete.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var serviceTierResult = await ServiceTierService.ServiceTierAsync(ServiceTierId);
            serviceTierResult.ProcessResponseForDisplay(Snackbar, () =>
            {
                _serviceTier = new ServiceTierViewModel(serviceTierResult.Data);

            });

            await base.OnInitializedAsync();
            _loaded = true;
        }
    }
}
