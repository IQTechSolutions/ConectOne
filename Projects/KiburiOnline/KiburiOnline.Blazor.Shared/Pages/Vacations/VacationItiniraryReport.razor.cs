using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Represents a report component for displaying details of a vacation itinerary.
    /// </summary>
    /// <remarks>This class is designed to be used in Blazor applications and provides functionality for
    /// loading and displaying vacation details based on a specified vacation identifier. It relies on dependency
    /// injection for accessing services such as HTTP operations, navigation management, configuration settings, and
    /// notification display.</remarks>
    public partial class VacationItiniraryReport
    {
        private VacationViewModel? _vacation;
        private NumberFormatInfo nfi;
        private List<DateTime> dates;

        /// <summary>
        /// The identity of the featured vacation.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage vacation-related operations.
        /// </summary>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the vacation details based on the vacation name.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var vacationResult = await VacationService.VacationAsync(VacationId);
            if (!vacationResult.Succeeded)
                SnackBar.AddErrors(vacationResult.Messages);
            else
            {
                _vacation = new VacationViewModel(vacationResult.Data);

                dates = DateTimeExtensions.GetDatesBetween(_vacation.StartDate.Value, _vacation.EndDate.Value);

                nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                nfi.CurrencySymbol = _vacation.CurrencySymbol;
            }

            StateHasChanged();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
