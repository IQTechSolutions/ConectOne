using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying the details of a vacation.
    /// </summary>
    public partial class Details
    {
        private MarkupString _shortDescriptionText;
        private MarkupString? _cancellationTermsText;
        private MarkupString? _termsAndConditionsText;
        private MarkupString? _bookingTermsText;
        private MarkupString? _paymentExclusionText;
        private MarkupString? _generalInformationText;

        private VacationViewModel? _vacation;
        private NumberFormatInfo nfi;

        private bool _loaded = false;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing custom variable tags.
        /// </summary>
        [Inject] private IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing custom variable tags.
        /// </summary>
        [Inject] private ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
        /// </summary>
        [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

        #region Parameters

        /// <summary>
        /// Gets or sets the unique identifier for the vacation request.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// The name of the vacation to display details for.
        /// </summary>
        [Parameter] public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier used to reference the associated resource in a URL-friendly format.
        /// </summary>
        [Parameter] public string Slug { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Navigates to the booking page for the specified vacation.
        /// </summary>
        /// <param name="vacationId">The identity of the vacation that is being booked</param>
        public void NavigateToBookNow(string vacationId)
        {
            NavigationManager.NavigateTo($"/contact-us", true);
        }

        #endregion

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

                if(_vacation.ShortDescription != null)
                    _shortDescriptionText = (MarkupString)_vacation.ShortDescription.Content;
                if (_vacation.CancellationTerms != null)
                    _cancellationTermsText = (MarkupString)_vacation.ShortDescription.Content;
                if (_vacation.TermsAndConditions != null)
                    _termsAndConditionsText = (MarkupString)_vacation.ShortDescription.Content;
                if (_vacation.BookingTerms != null)
                    _bookingTermsText = (MarkupString)_vacation.ShortDescription.Content;
                if (_vacation.PaymentExclusion != null)
                    _paymentExclusionText = (MarkupString)_vacation.ShortDescription.Content;
                if (_vacation.GeneralInformation != null)
                    _generalInformationText = (MarkupString)_vacation.ShortDescription.Content;

                nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                nfi.CurrencySymbol = _vacation.CurrencySymbol;

                _loaded = true;
            }

            

            await base.OnInitializedAsync();
        }

        #endregion

        private async Task<string> ProcessVariableTagsOfText(string text)
        {
            var result = text.Replace("<strong style=\"color: blue\">&lt;---ReferenceNr---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.ReferenceNr}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---HostName---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Host?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---ArrivalCity---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Flights?.FirstOrDefault(c => c.DepartureDayNr == null)?.ArrivalAirport?.City?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---StartDate---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.StartDate?.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---EndDate---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.EndDate.Value.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---NightCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Nights.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---DayCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.DayCount.ToString(CultureInfo.InvariantCulture)}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---AccommodationCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.RoomCount.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---FlightCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Flights?.Count.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---GolferPackagesCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.GolferPackages.Count().ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Consultant---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Consultant?.Name + _vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Coordinator---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Coordinator?.Name + _vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---TourDirector---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Coordinator?.Name + _vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---CompanyEmail---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:EmailAddress"]}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---CompanyPhoneNr---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:PhoneNr"]}</strong>");

            var availableCustomVariablePlaceholderResult = await CustomVariableTagService.GetAllAsync();
            if (availableCustomVariablePlaceholderResult.Succeeded)
            {
                foreach (var customTag in availableCustomVariablePlaceholderResult.Data)
                {
                    result = result.Replace($"<strong style=\"color: blue\">&lt;---{customTag.VariablePlaceholder}---&gt;</strong>", $"<strong style=\"color: black\">{customTag.VariablePlaceholder}</strong>");
                }
            }

            return result;
        }
    }
}
