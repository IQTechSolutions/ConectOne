namespace ShoppingModule.Application.ViewModels;

/// <summary>
/// Represents the view model for a shopping cart, containing details about the cart's total cost and the URL to
/// redirect the user after completing an operation.
/// </summary>
public class CartViewModel
{
    /// <summary>
    /// Gets or sets the total cost of all items in the shopping cart.
    /// </summary>
    public decimal ShoppingCartTotal { get; set; }

    /// <summary>
    /// Gets or sets the URL to which the user should be redirected after completing the current operation.
    /// </summary>
    public string ReturnUrl { get; set; }
}
