using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a service tier to a service.
    /// </summary>
    /// <remarks>This component allows users to select a service from a list and associate it with a service
    /// tier. The modal provides options to submit the selected service or cancel the operation.</remarks>
    public partial class AddServiceTierServiceModal
    {
        private bool _loaded;
        private ServiceDto _service = new();
        private IEnumerable<ServiceDto> _services = Enumerable.Empty<ServiceDto>();
        private Func<ServiceDto, string> _serviceConverter = p => p?.DisplayName ?? string.Empty;

        /// <summary>
        /// Gets or sets the service tier configuration for the application.
        /// </summary>
        [Parameter] public ServiceTierViewModel ServiceTier { get; set; } = new ServiceTierViewModel();

        /// <summary>
        /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
        /// </summary>
        /// <remarks>This property is typically used within a dialog component to manage its behavior,
        /// such as closing the dialog programmatically.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = default!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        /// <remarks>The provider must be set to a valid instance before making any HTTP operations. 
        /// Dependency injection is typically used to supply this property.</remarks>
        [Inject] public IServiceService ServiceService { get; set; } = default!;

        /// <summary>
        /// Submits the current operation and closes the dialog.
        /// </summary>
        /// <remarks>This method asynchronously performs the submission process and closes the dialog
        /// using the provided service. Ensure that the dialog is in a valid state before calling this method.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SubmitAsync()
        {
            MudDialog.Close(_service);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method terminates the dialog and triggers the cancellation logic.  It is
        /// typically used to close the dialog without completing the operation.</remarks>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #region Overrides

        /// <summary>
        /// Asynchronously initializes the component and loads the initial data.
        /// </summary>
        /// <remarks>This method overrides the base implementation to fetch a paginated list of services 
        /// and process the response for display. It sets the component's state to indicate that  the data has been
        /// loaded.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var serviceResult = await ServiceService.PagedServicesAsync(new ServiceParameters() { PageSize = 100 });
            serviceResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _services = serviceResult.Data;
            });
            _loaded = true;
        }

        #endregion
    }
}
