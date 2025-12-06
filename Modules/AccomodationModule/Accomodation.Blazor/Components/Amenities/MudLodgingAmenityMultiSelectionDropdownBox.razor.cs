using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Amenities
{
    /// <summary>
    /// Component for displaying a multi-selection dropdown box for lodging amenities.
    /// </summary>
    public partial class MudLodgingAmenityMultiSelectionDropdownBox : ComponentBase
    {
        private bool _loaded;
        private LodgingAmenityViewModel _amenity = null!;
        private IEnumerable<LodgingAmenityViewModel> _amenities = null!;

        #region Parameters

        /// <summary>
        /// Gets or sets the service responsible for managing amenities.
        /// </summary>
        [Inject] public IAmenityService AmenityService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snackbars or toast notifications in the user interface.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the parent ID for the amenities.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of amenities.
        /// </summary>
        [Parameter] public IEnumerable<LodgingAmenityViewModel> Amenities { get; set; } = null!;

        /// <summary>
        /// Event callback for when the selected amenities change.
        /// </summary>
        [Parameter] public EventCallback<IEnumerable<LodgingAmenityViewModel>> AmenitiesChanged { get; set; }

        /// <summary>
        /// Gets or sets the room view model.
        /// </summary>
        [Parameter] public RoomViewModel? Room { get; set; }

        /// <summary>
        /// Gets or sets the variant of the dropdown box.
        /// </summary>
        [Parameter] public Variant Variant { get; set; } = Variant.Text;

        #endregion

        #region Methods

        /// <summary>
        /// Handles the change in selected values.
        /// </summary>
        /// <param name="values">The new selected values.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SelectedValuesChanged(IEnumerable<LodgingAmenityViewModel> values)
        {
            var lodgingAmenityViewModels = values as LodgingAmenityViewModel[] ?? values.ToArray();
            Amenities = lodgingAmenityViewModels;
            await AmenitiesChanged.InvokeAsync(lodgingAmenityViewModels);
        }

        /// <summary>
        /// Converter function for lodging amenities.
        /// </summary>
        Func<LodgingAmenityViewModel?, string> _lodgingAmenityConverter = p => p?.Name != null ? p.Name : "";

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var amenityResult = await AmenityService.PagedAmenitiesAsync(new RequestParameters { PageSize = 100 });
            if (amenityResult.Succeeded)
            {
                _amenities = amenityResult.Data.Select(c => new LodgingAmenityViewModel(c)).ToList();

                var selectionResult = Amenities;

                var selectionList = new List<LodgingAmenityViewModel>();

                selectionList.AddRange(from category in selectionResult where _amenities.Any(c => c.AmenityId == category.AmenityId) select _amenities.FirstOrDefault(c => c.AmenityId == category.AmenityId));

                Amenities = selectionList;

                await base.OnInitializedAsync();
                _loaded = true;
            }
            else
            {
                foreach (var error in amenityResult.Messages)
                {
                    Snackbar.Add(error, Severity.Error);
                }
            }
        }

        #endregion
    }
}
