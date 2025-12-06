using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationSummaries
{
    /// <summary>
    /// The Details component is responsible for displaying detailed information about a specific vacation.
    /// </summary>
    public partial class Details
    {
        private VacationViewModel? _vacation;
        private NumberFormatInfo nfi;
        private MarkupString? _bookingTermsText;
        private MarkupString? _paymentExclusionText;
        private MarkupString _shortDescriptionText;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;


        #region Parameters

        /// <summary>
        /// The name of the vacation to display details for.
        /// </summary>
        [Parameter] public string Slug { get; set; } = null!;

        #endregion

        /// <summary>
        /// Navigates to the booking page for the specified vacation.
        /// </summary>
        /// <param name="vacationId">The identity of the vacation that is being booked</param>
        public void NavigateToBookNow(string vacationId)
        {
            NavigationManager.NavigateTo($"/bookings/create/{vacationId}", true);
        }

        #region Lifecycle Methods

        /// <summary>
        /// Called after the component has rendered. If this is the first render,
        /// it fetches the featured categories from the server.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render of the component.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var vacationIdResult = await VacationService.VacationFromSlugAsync(Slug);
                if (!vacationIdResult.Succeeded)
                    SnackBar.AddErrors(vacationIdResult.Messages);
                else
                {
                    _vacation = new VacationViewModel(vacationIdResult.Data);

                    if (_vacation.ShortDescription != null)
                        _shortDescriptionText = (MarkupString)_vacation.ShortDescription.Content;
                    if (_vacation.BookingTerms != null)
                        _bookingTermsText = (MarkupString)_vacation.ShortDescription.Content;
                    if (_vacation.PaymentExclusion != null)
                        _paymentExclusionText = (MarkupString)_vacation.ShortDescription.Content;

                    nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                    nfi.CurrencySymbol = _vacation.CurrencySymbol;
                }
                   

                StateHasChanged();
            }
        }

        #endregion
    }
}
