using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingModule.Application.ViewModels;
using ShoppingModule.Domain.Enums;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Blazor.Pages.Components
{
	/// <summary>
	/// Represents a table component for displaying and managing sales orders in a paginated and sortable format.
	/// </summary>
	/// <remarks>This class is designed to work with the MudBlazor table component and provides functionality for
	/// retrieving and displaying sales order data. It supports pagination, sorting, and customization of table appearance
	/// (e.g., dense, striped, or bordered styles).</remarks>
	public partial class MudSalesOrderTable
	{
		private bool _dense = true;
		private bool _striped = true;
		private bool _bordered = false;
		private SalesOrderPageParameters _pageParameters = new SalesOrderPageParameters();
		private MudTable<SalesOrderViewModel> table;

		/// <summary>
		/// Gets or sets the service responsible for managing sales orders.
		/// </summary>
		[Inject] public ISalesOrderService SalesOrderService { get; set; }

		/// <summary>
		/// Gets or sets the current status of the order.
		/// </summary>
		[Parameter] public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// Retrieves a paginated and optionally sorted list of sales orders based on the specified table state.
        /// </summary>
        /// <remarks>The method adjusts the page number to be one-based before querying the data source.  Sorting is
        /// applied based on the specified sort label and direction, if provided.</remarks>
        /// <param name="state">The state of the table, including pagination and sorting information.  The <see cref="TableState.Page"/> property
        /// specifies the zero-based page index,  <see cref="TableState.PageSize"/> specifies the number of items per page, 
        /// and <see cref="TableState.SortDirection"/> and <see cref="TableState.SortLabel"/> determine the sorting behavior.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of sales orders and a collection of  <see
        /// cref="SalesOrderViewModel"/> objects representing the sales orders for the specified page.</returns>
        public async Task<TableData<SalesOrderViewModel>> GetSalesOrdersAsync(TableState state, CancellationToken token)
		{			
			_pageParameters.PageNr = state.Page+1;
			_pageParameters.PageSize = state.PageSize;

			if (state.SortDirection == SortDirection.Ascending)
				_pageParameters.OrderBy = $"{state.SortLabel} desc";
			else if (state.SortDirection == SortDirection.Descending)
				_pageParameters.OrderBy = $"{state.SortLabel} asc";
			else _pageParameters.OrderBy = null;

            _pageParameters.OrderStatus = OrderStatus;

			var pagingResponse = await SalesOrderService.PagedSalesOrdersAsync(_pageParameters);

			return new TableData<SalesOrderViewModel>() { TotalItems = pagingResponse.TotalCount, Items = pagingResponse.Data.Select(c => new SalesOrderViewModel(c)) };
		}
	}
}
