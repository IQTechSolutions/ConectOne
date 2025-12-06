using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Destinations
{
    /// <summary>
    /// Represents the Zambia component, which is responsible for managing vacation-related data and displaying
    /// notifications within the application.
    /// </summary>
    /// <remarks>This class is a Blazor component that relies on dependency injection to provide services for
    /// configuration, vacation management, and user notifications. Ensure that all required services, such as <see
    /// cref="IConfiguration"/>, <see cref="IVacationService"/>, and <see cref="ISnackbar"/>, are properly registered in
    /// the dependency injection container.</remarks>
    public partial class Zambia
    {
        private ICollection<VacationDto> _vacations = [];

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snackbars in the application.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and loads the initial set of vacation data.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during component initialization.
        /// Override this method to perform additional asynchronous setup before the component is rendered.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var response = await VacationService.AllVacationsAsync(new VacationPageParameters() { PageSize = 100 });
            response.ProcessResponseForDisplay(Snackbar, () =>
            {
                _vacations = response.Data.ToList();
            });

            await base.OnInitializedAsync();
        }
    }
}
