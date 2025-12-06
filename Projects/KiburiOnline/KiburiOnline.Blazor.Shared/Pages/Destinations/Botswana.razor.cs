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
    /// Represents a component responsible for managing and displaying vacation-related data.
    /// </summary>
    /// <remarks>This class is a Blazor component that interacts with various services, including
    /// configuration, vacation management, and user notifications. It relies on dependency injection to provide the
    /// required services. The component fetches and processes vacation data asynchronously during
    /// initialization.</remarks>
    public partial class Botswana
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
        /// Asynchronously initializes the component and retrieves a list of vacations.
        /// </summary>
        /// <remarks>This method fetches vacation data using the <see cref="VacationService"/> and
        /// processes the response  for display. If the response is successful, the vacation data is stored for
        /// rendering.  Additionally, this method invokes the base implementation of <see
        /// cref="OnInitializedAsync"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
