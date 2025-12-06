using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages
{
    /// <summary>
    /// Represents the home component responsible for managing vacation data and displaying notifications.
    /// </summary>
    /// <remarks>This component interacts with an HTTP provider to fetch vacation data and uses a snackbar
    /// service to display messages to the user. It is designed to be used within a Blazor application.</remarks>
    public partial class Home
    {
        private ICollection<VacationDto> _vacations = [];

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snackbars in the application.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> used to manage and perform navigation within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for
        /// programmatic navigation and access to the current URI. It should not be set manually except in advanced
        /// scenarios, such as unit testing.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and retrieves a collection of vacation data.
        /// </summary>
        /// <remarks>This method is called when the component is initialized. It fetches vacation data
        /// from the specified API endpoint.</remarks>
        protected override async Task OnInitializedAsync()
        {
            var response = await VacationService.AllVacationsAsync(new VacationPageParameters() { PageSize = 100 });
            response.ProcessResponseForDisplay(Snackbar, () =>
            {
                _vacations = response.Data.ToList();
            });

            await InvokeAsync(StateHasChanged);

            await base.OnInitializedAsync();
        }
    }
}
