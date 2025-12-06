using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for managing vacation itineraries.
    /// </summary>
    public partial class VacationItineraryEditor
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs and notifications.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        [Inject] public IVacationService VacationService { get; set; }

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the itineraries belong.
        /// </summary>
        [Parameter, EditorRequired] public string VacationIntervalId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the itineraries associated with the vacation.
        /// </summary>
        private List<ItineraryDto> _itineraries = [];

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new itinerary item.
        /// </summary>
        private async Task CreateItineraryItem()
        {
            var parameters = new DialogParameters<AddItineraryModal> { { x => x.VacationIntervalId, VacationIntervalId } };

            var dialog = await DialogService.ShowAsync<AddItineraryModal>("Confirm", parameters);
            var result = await dialog.Result;
            //if (!result!.Canceled)
            //{
            //    var createdItem = ((ItineraryViewModel)result.Data!).ToDto();
            //    var creationResult = await Provider.PutAsync("vacations/itineraries", createdItem);
            //    creationResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        _itineraries.Add(createdItem);
            //    });
            //}
        }

        /// <summary>
        /// Updates an existing itinerary item by displaying a confirmation dialog, processing user input,  and sending
        /// the updated data to the server.
        /// </summary>
        /// <remarks>This method displays a modal dialog to allow the user to edit the itinerary item. If
        /// the user  confirms the changes, the updated itinerary is sent to the server for persistence. The local 
        /// collection of itineraries is updated upon successful server response.</remarks>
        /// <param name="itinerary">The itinerary item to be updated. Must not be <see langword="null"/>.</param>
        private async Task UpdateItineraryItem(ItineraryDto itinerary)
        {
            var parameters = new DialogParameters<AddItineraryModal> { { x => x.Itinerary, new ItineraryViewModel(itinerary) } };

            var dialog = await DialogService.ShowAsync<AddItineraryModal>("Confirm", parameters);
            var result = await dialog.Result;
            //if (!result!.Canceled)
            //{
            //    var createdItem = new ItineraryDto((ItineraryViewModel)result.Data!);
            //    var creationResult = await Provider.PutAsync("vacations/itineraries", createdItem);
            //    creationResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        var modalResult = (ItineraryViewModel)result.Data!;
            //        var vacationInclusion = _itineraries.FirstOrDefault(c => c.ItineraryId == itinerary.ItineraryId)!;
            //        var index = _itineraries.IndexOf(vacationInclusion);
            //        _itineraries[index] = new ItineraryDto(modalResult);
            //    });
            //}
        }

        /// <summary>
        /// Removes an itinerary item.
        /// </summary>
        /// <param name="itineraryId">The ID of the itinerary item to remove.</param>
        private async Task RemoveItineraryItem(string itineraryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this pricing item from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            //if (!result!.Canceled)
            //{
            //    var removalResult = await Provider.DeleteAsync($"vacations/itineraries", itineraryId);
            //    removalResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        _itineraries.Remove(_itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId)!);
            //    });
            //}
        }

        /// <summary>
        /// Creates a new itinerary detail item.
        /// </summary>
        /// <param name="itineraryId">The ID of the itinerary to which the detail item belongs.</param>
        private async Task CreateItineraryDetailItem(string itineraryId)
        {
            var parameters = new DialogParameters<AddItineraryItemModal> { { x => x.ItineraryId, VacationIntervalId } };

            var dialog = await DialogService.ShowAsync<AddItineraryItemModal>("Confirm", parameters);
            var result = await dialog.Result;
            //if (!result!.Canceled)
            //{
            //    var itineraryItem = new ItineraryItemDto((ItineraryItemViewModel)result.Data!);
            //    var creationResult = await Provider.PutAsync("vacations/itineraries/details", itineraryItem);
            //    creationResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        var itinerary = _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId);
            //        itinerary!.Details.Add(itineraryItem);
            //        _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId)!.ShowItems = true;
            //        StateHasChanged();
            //    });
            //}
        }

        /// <summary>
        /// Updates the details of an itinerary item by displaying a modal dialog for user input and processing the
        /// result.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to edit the details of the
        /// specified itinerary item. If the user confirms the changes, the updated item is sent to the server for
        /// persistence. The local state is updated to reflect the changes, and the UI is refreshed.</remarks>
        /// <param name="itineraryDetail">The itinerary item to be updated, represented as a <see cref="ItineraryItemDto"/>.  This parameter must
        /// contain valid identifiers and description for the itinerary item.</param>
        private async Task UpdateItineraryDetailItem(ItineraryItemDto itineraryDetail)
        {
            var parameters = new DialogParameters<AddItineraryItemModal> { { x => x.ItineraryItem, new ItineraryItemViewModel(){ ItineraryId = itineraryDetail.ItineraryId, ItineraryItemId = itineraryDetail.ItineraryItemId, Description = itineraryDetail.Description} } };

            var dialog = await DialogService.ShowAsync<AddItineraryItemModal>("Confirm", parameters);
            var result = await dialog.Result;
            //if (!result!.Canceled)
            //{
            //    var itineraryItem = new ItineraryItemDto((ItineraryItemViewModel)result.Data!);
            //    var creationResult = await Provider.PutAsync("vacations/itineraries/details", itineraryItem);
            //    creationResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        var modalResult = (ItineraryItemViewModel)result.Data!;
            //        var itineraryItem = _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryDetail.ItineraryId).Details!.FirstOrDefault(g => g.ItineraryItemId == itineraryDetail.ItineraryItemId); ;
            //        var index = _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryDetail.ItineraryId).Details.IndexOf(itineraryItem);

            //        _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryDetail.ItineraryId).Details[index] = new ItineraryItemDto(modalResult);
            //        _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryDetail.ItineraryId).ShowItems = true;

            //        StateHasChanged();
            //    });
            //}
        }

        /// <summary>
        /// Removes an itinerary detail item.
        /// </summary>
        /// <param name="itineraryId">The ID of the itinerary to which the detail item belongs.</param>
        /// <param name="itineraryItemId">The ID of the itinerary detail item to remove.</param>
        private async Task RemoveItineraryDetailItem(string itineraryId, string itineraryItemId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this itinerary from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            //if (!result!.Canceled)
            //{
            //    var removalResult = await Provider.DeleteAsync($"vacations/itineraries/details", itineraryItemId);
            //    removalResult.ProcessResponseForDisplay(SnackBar, () =>
            //    {
            //        var itinerary = _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId);
            //        itinerary!.Details.Remove(itinerary.Details.FirstOrDefault(c => c.ItineraryItemId == itineraryItemId)!);
            //    });
            //}
        }

        /// <summary>
        /// Toggles the visibility of itinerary details for a specific itinerary.
        /// </summary>
        /// <param name="itineraryId">The ID of the itinerary to show or hide details for.</param>
        private void ShowDetails(string itineraryId)
        {
            _itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId)!.ShowItems = !_itineraries.FirstOrDefault(c => c.ItineraryId == itineraryId)!.ShowItems;
            StateHasChanged();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the itineraries associated with the vacation.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            //var itinerariesResult = await Provider.GetAsync<IEnumerable<ItineraryDto>>(($"vacations/itineraries/{VacationIntervalId}"));
            //itinerariesResult.ProcessResponseForDisplay(SnackBar, () =>
            //{
            //    _itineraries = itinerariesResult.Data.OrderBy(c => c.Date).ToList();
            //});

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
