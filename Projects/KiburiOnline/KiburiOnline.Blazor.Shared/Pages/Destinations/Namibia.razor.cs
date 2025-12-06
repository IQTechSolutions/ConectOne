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
    /// Represents the Namibia component, which manages vacation-related operations and displays.
    /// </summary>
    /// <remarks>This class is a Blazor component that interacts with various services, including  <see
    /// cref="IVacationService"/> for vacation data management and <see cref="ISnackbar"/>  for user notifications. It
    /// relies on dependency injection to provide the required services.</remarks>
    public partial class Namibia
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
        /// Asynchronously initializes the component and retrieves the initial list of vacations.
        /// </summary>
        /// <remarks>This method fetches vacation data using the <see cref="VacationService"/> and
        /// processes the response  for display. If the response is successful, the vacation data is stored for
        /// rendering.  Additionally, this method invokes the base class's initialization logic.</remarks>
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
