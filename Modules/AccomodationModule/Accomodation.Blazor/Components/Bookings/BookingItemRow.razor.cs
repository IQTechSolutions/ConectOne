using System.Security.Principal;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShoppingModule.Application.ViewModels;

namespace Accomodation.Blazor.Components.Bookings
{
    /// <summary>
    /// Represents a row in the booking item list, providing functionality for displaying booking details and handling
    /// user interactions such as removing items or managing bookings.
    /// </summary>
    /// <remarks>This component is designed to work within a Blazor application and relies on cascading and
    /// parameterized data for its functionality. It interacts with authentication state and external services to fetch
    /// booking-related details.</remarks>
    public partial class BookingItemRow
    {
        private LodgingDto? _lodging = null!;
        private IEnumerable<BedTypeDto> _bedTypes = [];
        private RoomDto? _room = null!;
        private IPrincipal _user;

        /// <summary>
        /// Gets or sets the <see cref="Task{TResult}"/> representing the asynchronous operation  to retrieve the
        /// current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> object provides details such as the user's
        /// identity and claims.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        /// <summary>
        /// Gets or sets the booking view model associated with the current item.
        /// </summary>
        [Parameter] public BookingViewModel Item { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback invoked when an item is removed from the cart.
        /// </summary>
        /// <remarks>Use this property to define the behavior for removing items from the cart.  The
        /// callback is typically bound to a method that handles the removal logic.</remarks>
        [Parameter] public EventCallback<CartItemViewModel> RemoveItem { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing lodging-related operations.
        /// </summary>
        [Inject] public ILodgingService LodgingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing room data operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the service is properly configured in the dependency injection container.</remarks>
        [Inject] public IRoomDataService RoomDataService { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and retrieves data required for rendering.
        /// </summary>
        /// <remarks>This method fetches authentication state and user information, as well as lodging and
        /// room details based on the specified identifiers. It ensures that the component is properly initialized with
        /// the necessary data before rendering.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            var result = await LodgingService.LodgingAsync(Item.LodgingId);
            if (result.Succeeded)
                _lodging = result.Data;

            var roomResult = await RoomDataService.RoomAsync(Item.RoomId.Value);
            if (roomResult.Succeeded)
            {
                _room = roomResult.Data;
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Removes the specified item from the cart asynchronously.
        /// </summary>
        /// <param name="cartItem">The item to be removed from the cart. This parameter cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DoRemoveItem(CartItemViewModel cartItem)
        {
            await RemoveItem.InvokeAsync(cartItem);
        }

        /// <summary>
        /// Cancels an existing booking identified by the specified booking ID.
        /// </summary>
        /// <remarks>Use this method to cancel a booking that is no longer needed. Ensure the booking ID
        /// corresponds to a valid and active booking.</remarks>
        /// <param name="bookingId">The unique identifier of the booking to be canceled. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when the booking has been successfully
        /// canceled.</returns>
        public async Task CancelBooking(int bookingId)
        {
            
        }

        /// <summary>
        /// Confirms a manual booking by its unique identifier.
        /// </summary>
        /// <remarks>This method performs the confirmation of a booking that was manually created. Ensure
        /// the booking ID provided corresponds to an existing booking.</remarks>
        /// <param name="bookingId">The unique identifier of the booking to confirm. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ConfirmManualBooking(int bookingId)
        {
            
        }
    }
}
