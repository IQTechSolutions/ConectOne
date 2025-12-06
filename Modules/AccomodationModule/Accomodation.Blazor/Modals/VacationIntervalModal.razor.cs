using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class VacationIntervalModal
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public VacationDto? Vacation { get; set; }

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public VacationIntervalViewModel VacationInterval { get; set; } = new();

        /// <summary>
        /// Gets or sets the service responsible for managing lodging-related operations.
        /// </summary>
        [Inject] public ILodgingService LodgingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling destination-related operations.
        /// </summary>
        [Inject] public IDestinationService DestinationService { get; set; } = null!;

        #endregion

        #region Fields

        /// <summary>
        /// A list of available lodgings for selection in the modal.
        /// </summary>
        private readonly List<LodgingDto> _availableLodgings = [];

        /// <summary>
        /// A function to convert a lodging DTO to its name for display purposes.
        /// </summary>
        private readonly Func<LodgingDto?, string?> _lodgingConverter = p => p?.Name;

        /// <summary>
        /// A list of available destinations for selection in the modal.
        /// </summary>
        private readonly List<DestinationDto> _availableDestinations = [];

        /// <summary>
        /// A function to convert a destination DTO to its name for display purposes.
        /// </summary>
        private readonly Func<DestinationDto?, string?> _destinationConverter = p => p?.Name;

        #endregion

        #region Methods

        /// <summary>
        /// Searches for lodgings that match the specified search value.
        /// </summary>
        /// <remarks>The search is case-insensitive and matches lodgings whose names contain the specified
        /// search term.</remarks>
        /// <param name="value">The search term to filter lodgings by name. If <see langword="null"/> or empty, all available lodgings are
        /// returned.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A collection of <see cref="LodgingDto"/> objects representing the lodgings that match the search criteria.
        /// If no lodgings match, an empty collection is returned.</returns>
        private async Task<IEnumerable<LodgingDto>> SearchLodgings(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _availableLodgings;
            }

            return _availableLodgings.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Searches for destinations that match the specified value.
        /// </summary>
        /// <remarks>The search is case-insensitive and matches destinations whose names contain the
        /// specified value.</remarks>
        /// <param name="value">The search term to filter destinations by. If <see langword="null"/> or empty, all available destinations
        /// are returned.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A collection of <see cref="DestinationDto"/> objects representing the destinations that match the search
        /// term. If no destinations match, an empty collection is returned.</returns>
        private async Task<IEnumerable<DestinationDto>> SearchDestinations(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _availableDestinations;
            }

            return _availableDestinations.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(VacationInterval);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is initialized. Loads the available lodgings from the server.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            VacationInterval.VacationId = VacationInterval.VacationId;

            var lodgingResult = await LodgingService.AllLodgings();
            if (lodgingResult.Succeeded)
                _availableLodgings.AddRange(lodgingResult.Data);

            var destinationResult = await DestinationService.AllDestinationsAsync();
            if (destinationResult.Succeeded)
                _availableDestinations.AddRange(destinationResult.Data);

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
