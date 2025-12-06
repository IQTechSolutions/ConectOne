using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Vouchers
{
    /// <summary>
    /// Component for displaying and managing lodging vouchers in a table format.
    /// </summary>
    public partial class MudLodgingVoucherTable
    {
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered;

        private MudTable<VoucherViewModel> _table = null!;
        private VoucherPageParameters _pageParameters = new VoucherPageParameters() { PageSize = 100 };

        #region Parameters

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the ISnackbar
        /// service to show brief, unobtrusive messages (snackbars) in the application's user interface.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage vouchers.
        /// </summary>
        [Inject] public IVoucherService VoucherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user ID for filtering vouchers.
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a paginated list of vouchers based on the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        public async Task<TableData<VoucherViewModel>> GetVouchersAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            PaginatedResult<VoucherDto> pagingResponse = new PaginatedResult<VoucherDto>(new List<VoucherDto>());

            if (string.IsNullOrEmpty(UserId))
                pagingResponse = await VoucherService.PagedVouchersAsync(_pageParameters, token);

            return new TableData<VoucherViewModel>()
            {
                TotalItems = pagingResponse.Data.Count(),
                Items = pagingResponse.Data.Select(c => new VoucherViewModel(c))
            };
        }

        /// <summary>
        /// Deletes a voucher by its ID after user confirmation.
        /// </summary>
        /// <param name="voucherId">The ID of the voucher to delete.</param>
        private async Task DeleteCategory(int voucherId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this product from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await VoucherService.DeleteVoucherAsync(voucherId);
                removalResult.ProcessResponseForDisplay(Snackbar, async () =>
                {
                    await _table.ReloadServerData();
                });
            }
        }

        #endregion
    }
}
