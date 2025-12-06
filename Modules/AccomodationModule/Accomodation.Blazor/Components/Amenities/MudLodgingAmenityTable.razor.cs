using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Amenities
{
    /// <summary>
    /// Component for displaying and managing lodging amenities in a table format.
    /// </summary>
    public partial class MudLodgingAmenityTable
    {
        private readonly bool _dense = true;
        private readonly bool _striped = true;
        private readonly bool _bordered = false;

        private MudTable<LodgingAmenityViewModel> _table = null!;
        private RequestParameters _pageParameters = new RequestParameters();

        /// <summary>
        /// Gets or sets the parent ID for the amenities.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing amenities.
        /// </summary>
        [Inject] public IAmenityService AmenityService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snackbars or notifications in the user interface. Assigning this property manually is not recommended
        /// outside of dependency injection scenarios.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Retrieves a paginated list of amenities based on the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<LodgingAmenityViewModel>> GetAmenitiesAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var pagingResponse = await AmenityService.PagedAmenitiesAsync(_pageParameters, token);

            return new TableData<LodgingAmenityViewModel>()
            {
                TotalItems = pagingResponse.Data.Count(),
                Items = pagingResponse.Data.Select(c => new LodgingAmenityViewModel(c))
            };
        }

        /// <summary>
        /// Deletes an amenity by its ID after user confirmation.
        /// </summary>
        /// <param name="amenityId">The ID of the amenity to delete.</param>
        private async Task DeleteAmenity(int amenityId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this amenity from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await AmenityService.RemoveAmentity(amenityId);
                removalResult.ProcessResponseForDisplay(Snackbar, () =>
                {
                    _table.ReloadServerData();
                });
            }
        }

        /// <summary>
        /// Displays a modal dialog to add a new amenity.
        /// </summary>
        private async Task ShowAddAmenities()
        {
            var dialog = await DialogService.ShowAsync<AddAmenityModal>("Create New Amenity");
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Displays a modal dialog to update an existing amenity.
        /// </summary>
        /// <param name="amenityId">The ID of the amenity to update.</param>
        private async Task ShowUpdateAmenities(int amenityId)
        {
            var parameters = new DialogParameters<AddAmenityModal>();
            parameters.Add(x => x.AmenityId, amenityId);

            var dialog = await DialogService.ShowAsync<AddAmenityModal>("Create New Amenity", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Triggers a search and reloads the table data.
        /// </summary>
        private void Search()
        {
            _table.ReloadServerData();
        }
    }
}
