using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ShoppingModule.Application.ViewModels;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// Represents a shopping cart that provides functionality for managing cart items, navigating to the checkout page,
    /// and interacting with the cart's state.
    /// </summary>
    /// <remarks>This class is designed to work with a <see cref="CartStateProvider"/> to manage the state of
    /// the shopping cart. It includes methods for removing items from the cart and navigating to specific pages, as
    /// well as properties for storing theme and numeric values.</remarks>
    public partial class Cart
    {
        /// <summary>
        /// Gets or sets the current shopping cart state for the application.
        /// </summary>
        /// <remarks>This property is typically used to provide access to the shopping cart's state within
        /// a Blazor component hierarchy.</remarks>
        [CascadingParameter] public CartStateProvider ShoppingCart { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection and allows components to
        /// show notifications such as alerts, confirmations, or status messages. The specific behavior and appearance
        /// of the messages depend on the implementation of the ISnackbar service.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Navigates the user to the checkout page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/checkout" route. Ensure that the route is
        /// properly configured in the application to handle the navigation.</remarks>
        public void NavigateToCheckout()
        {
            NavigationManager.NavigateTo("/checkout");
        }

        /// <summary>
        /// Represents the theme configuration for the application.
        /// </summary>
        /// <remarks>This field holds an instance of <see cref="MudTheme"/>, which defines the visual
        /// appearance and styling options for the application. Modify this field to customize the theme
        /// settings.</remarks>
        public MudTheme Theme = new MudTheme();

        /// <summary>
        /// Gets or sets an integer value.
        /// </summary>
        public int IntValue { get; set; }

        /// <summary>
        /// Gets or sets a double-precision floating-point value.
        /// </summary>
        public double DoubleValue { get; set; }

        /// <summary>
        /// Gets or sets the decimal value associated with this instance.
        /// </summary>
        public decimal DecimalValue { get; set; }

        #region Methods

        /// <summary>
        /// Removes an item from the shopping cart and updates the cart state.
        /// </summary>
        /// <remarks>If the removal operation fails, error messages are displayed using the snack bar.  If
        /// the shopping cart becomes empty after the removal, the user is redirected to the home page.</remarks>
        /// <param name="vm">The view model representing the item to be removed from the cart.</param>
        /// <returns></returns>
        public async Task RemoveCartItem(CartItemViewModel vm)
        {
            var result = await ShoppingCart.RemoveFromCartAsync(vm);
            if(!result.Succeeded) SnackBar.AddErrors(result.Messages);

            if (ShoppingCart.ShoppingCart.CartItems.Count == 0)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        #endregion
    }
}
