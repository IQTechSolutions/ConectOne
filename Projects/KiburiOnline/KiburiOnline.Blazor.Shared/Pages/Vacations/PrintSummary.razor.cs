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
    /// Represents a component that displays a summary of a vacation, including its details and inclusions.
    /// </summary>
    /// <remarks>The <see cref="PrintSummary"/> component is designed to fetch and display vacation details
    /// based on the  provided vacation name. It uses dependency injection to access configuration settings, HTTP
    /// services,  and a snackbar notification system for error handling.</remarks>
    public partial class PrintSummary
    {
        private VacationViewModel? _vacation;
        private IEnumerable<VacationInclusionDto> _inclusions = new List<VacationInclusionDto>();
        private NumberFormatInfo nfi;
        private MarkupString _shortDescriptionText;

        /// <summary>
        /// Gets or sets the name of the vacation.
        /// </summary>
        [Parameter] public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the service used to manage vacation requests and related operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="IVacationService"/> to enable vacation management features within the
        /// component.</remarks>
        [Inject] public IVacationService VacationService { get; set; }

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

                if (_vacation.ShortDescription != null)
                    _shortDescriptionText = (MarkupString)_vacation.ShortDescription.Content;
            }

            StateHasChanged();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
