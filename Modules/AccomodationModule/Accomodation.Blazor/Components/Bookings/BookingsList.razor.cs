using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace Accomodation.Blazor.Components.Bookings
{
    /// <summary>
    /// Component to display a list of bookings
    /// </summary>
    public partial class BookingsList : ComponentBase
    {
        private string _searhText = null!;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private MudTable<BookingViewModel> _table = null!;
        private BookingParameters _pageParameters = new() { PageSize = 12 };

        #region Parameters and Injected Services

        /// <summary>
        /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication
        /// state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The task should be awaited to retrieve the <see
        /// cref="AuthenticationState"/>.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service provider used to resolve dependencies.
        /// </summary>
        /// <remarks>This property is typically injected and used to resolve services at runtime. Ensure
        /// that the service provider is properly configured before accessing it.</remarks>
        [Inject] public IServiceProvider ServiceProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing booking operations.
        /// </summary>
        [Inject] public IBookingService BookingService { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the current booking status for the reservation.
        /// </summary>
        [Parameter] public BookingStatus? BookingStatus { get; set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the change event for the active checkbox
        /// </summary>
        /// <param name="value">New value of the checkbox</param>
        private void CheckedChanged(bool value)
        {
            _pageParameters.Active = value;
            _table.ReloadServerData();
        }

        /// <summary>
        /// Handles the change event for the featured checkbox
        /// </summary>
        /// <param name="value">New value of the checkbox</param>
        private void FeaturedChanged(bool? value)
        {
            _pageParameters.Featured = value;
            _table.ReloadServerData();
        }

        #endregion

        #region Booking Operations


        /// <summary>
        /// Retrieves bookings asynchronously for the table
        /// </summary>
        /// <param name="state">State of the table</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Table data containing booking view models</returns>
        private async Task<TableData<BookingViewModel>> GetBookingsAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var pagingResponse = await BookingService.PagedBookingsAsync(_pageParameters);

            return new TableData<BookingViewModel>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new BookingViewModel(c)) };
        }

        /// <summary>
        /// Triggers a search operation
        /// </summary>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Method invoked when the component is initialized
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            UserId = authState.User.GetUserId();

            if (UserId is not null)
                _pageParameters.UserId = UserId;

            if (BookingStatus is not null)
                _pageParameters.BookingStatus = BookingStatus;
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
