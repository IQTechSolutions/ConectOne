using Microsoft.AspNetCore.Components;
using ShoppingModule.Application.ViewModels;

namespace ShoppingModule.Blazor.Pages.Components
{
    /// <summary>
    /// Represents a table component for displaying and managing sales order details.
    /// </summary>
    /// <remarks>This component is designed to work with a collection of <see
    /// cref="SalesOrderDetailViewModel"/> items. It provides functionality to increase or decrease the quantity of
    /// individual sales order items.</remarks>
    public partial class MudSalesOrderDetailsTable
    {
        /// <summary>
        /// Gets or sets the collection of sales order details to be displayed or processed.
        /// </summary>
        [Parameter] public IEnumerable<SalesOrderDetailViewModel> Items { get; set; }

        /// <summary>
        /// Increases the quantity of the specified sales order detail by one.
        /// </summary>
        /// <param name="viewModel">The sales order detail view model whose quantity will be incremented.  This parameter cannot be <see
        /// langword="null"/>.</param>
        public void IncreaseQty(SalesOrderDetailViewModel viewModel)
        {
            viewModel.Qty++;
        }

        /// <summary>
        /// Decreases the quantity of the specified sales order detail by one.
        /// </summary>
        /// <remarks>This method decrements the <see cref="SalesOrderDetailViewModel.Qty"/> property of
        /// the provided view model. Ensure that the quantity is not already at its minimum value before calling this
        /// method.</remarks>
        /// <param name="viewModel">The sales order detail view model whose quantity will be decreased.  The <see
        /// cref="SalesOrderDetailViewModel.Qty"/> property must be greater than zero to avoid underflow.</param>
        public void DecreaseQty(SalesOrderDetailViewModel viewModel)
        {
            viewModel.Qty--;
        }
    }
}
