using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Represents the schedule and details for a tour guide vacation.
    /// </summary>
    /// <remarks>This class is responsible for managing the vacation details, including fetching data from an
    /// HTTP provider and displaying relevant information. It integrates with dependency-injected services such as an
    /// HTTP provider, configuration settings, and a snackbar for user notifications.</remarks>
    public partial class TourGuidSchedule
    {
        private VacationViewModel? _vacation;
        private IEnumerable<VacationInclusionDto> _inclusions = new List<VacationInclusionDto>();
        private NumberFormatInfo nfi;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        [Inject] public IVacationService VacationService { get; set; }

        /// <summary>
        /// Gets or sets the name of the vacation.
        /// </summary>
        [Parameter] public string Slug { get; set; }
        
        #region Overrides

        /// <summary>
        /// Initializes the component and loads the vacation details based on the vacation name.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the vacation details based on the vacation name
            var vacationIdResult = await VacationService.VacationFromSlugAsync(Slug);
            if (!vacationIdResult.Succeeded)
            {
                // Display error messages if the fetch operation failed
                SnackBar.AddErrors(vacationIdResult.Messages);
            }
            else
            {
                // Populate the vacation ViewModel with the fetched data
                _vacation = new VacationViewModel(vacationIdResult.Data);
            }

            StateHasChanged();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
