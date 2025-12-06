using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationHosts
{
    /// <summary>
    /// Component for creating a new vacation host.
    /// </summary>
    public partial class Create
    {
        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

        private List<AreaDto> _availableAreas = new();
        private string? _coverImageToUpload;
        private Func<AreaDto, string> _areaConverter = p => p?.Name is null ? "" : p.Name;

        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
        private VacationHostViewModel _vacationHost = new();

        #region Parameter

        /// <summary>
        /// Gets or sets the callback that is invoked when the address changes.
        /// </summary>
        [Parameter] public EventCallback<string?> OnAddressChanged { get; set; }

        #endregion

        #region Injected Services

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation host operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid implementation of <see cref="IVacationHostService"/> is registered in the dependency
        /// container.</remarks>
        [Inject] public IVacationHostService VacationHostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing area-related operations.
        /// </summary>
        /// <remarks>The property must be set to a valid instance of <see cref="IAreaService"/> before it
        /// can be used.  Dependency injection ensures that this requirement is fulfilled at runtime.</remarks>
        [Inject] public IAreaService AreaService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Image Upload Methods

        /// <summary>
        /// Handles cover image change.
        /// </summary>
        /// <param name="coverImage">The new cover image path.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        #endregion

        #region Vacation Host Methods

        /// <summary>
        /// Creates a new vacation host.
        /// </summary>
        private async Task CreateAsync()
        {
            var creationResult = await VacationHostService.CreateAsync(_vacationHost.ToDto());
            if (!creationResult.Succeeded)
            {
                // Display error messages if the creation operation failed
                SnackBar.AddErrors(creationResult.Messages);
                return;
            }

            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_vacationHost.Name} CoverImage",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await VacationHostService.AddImage(new AddEntityImageRequest() { EntityId = _vacationHost.VacationHostId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            // Display success message
            SnackBar.Add($"Action Successful. Vacation host \"{_vacationHost.Name}\" was successfully created.", Severity.Success);

            // Show confirmation dialog to ask if the user wants to add another vacation host
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Would you like to add another service?" },
                { x => x.ButtonText, "Yes" },
                { x => x.CancelButtonText, "No" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (result!.Canceled)
            {
                // Navigate to the vacation hosts list page if the user chooses not to add another vacation host
                NavigationManager.NavigateTo($"/vacationhosts/update/{_vacationHost.VacationHostId}");
            }
            else
            {
                // Reset the vacation host ViewModel to allow adding a new vacation host
                _vacationHost = new VacationHostViewModel();
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the vacation hosts list page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/vacationhosts");
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the autocomplete functionality and sets up event listeners for handling place selection on the
        /// map.
        /// </summary>
        /// <remarks>This method configures the autocomplete feature for a specified input element,
        /// allowing users to search for places. When a place is selected, the map is updated to center on the selected
        /// location, and relevant details such as  address components, latitude, and longitude are extracted and
        /// stored. A marker is also added to the map at the  selected location.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task OnAfterMapInit()
        {
            var latLng = new LatLngLiteral(-31.9241074, 23.7627237);

            _autocomplete = await Autocomplete.CreateAsync(_map1.JsRuntime, _autoCompleteBox.InputReference.ElementReference, new AutocompleteOptions
            {
                StrictBounds = false,
                ComponentRestrictions = new ComponentRestrictions { Country = new[] { "za" } }
            });

            await _autocomplete.SetFields(new[] { "address_components", "geometry", "name" });

            await _autocomplete.AddListener("place_changed", async () =>
            {
                var place = await _autocomplete.GetPlace();

                if (place?.Geometry == null)
                {
                    SnackBar.AddError("No results available for " + place?.Name);
                }
                else if (place.Geometry.Location != null)
                {
                    await _map1.InteropObject.SetCenter(place.Geometry.Location);
                    await _map1.InteropObject.SetZoom(13);

                    _vacationHost.Lng = place.Geometry.Location.Lng;
                    _vacationHost.Lat = place.Geometry.Location.Lat;

                    var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
                    {
                        Position = place.Geometry.Location,
                        Map = _map1.InteropObject,
                        Title = place.Name
                    });

                    _markers.Push(marker);

                    _vacationHost.Address = string.Empty;
                    foreach (var component in place.AddressComponents)
                    {
                        var componentType = component.Types[0];

                        switch (componentType)
                        {
                            case "street_number":
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                            case "route":
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                            case "sublocality_level_1":
                            case "sublocality_level_2":
                                _vacationHost.Suburb = component.LongName;
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                            case "postal_code":
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                            case "locality":
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                            case "country":
                                _vacationHost.Address = _vacationHost.Address + " " + component.LongName;
                                break;
                        }
                    }
                }
                else if (place.Geometry.Viewport != null)
                {
                    await _map1.InteropObject.FitBounds(place.Geometry.Viewport, 5);
                }

                StateHasChanged();
            });
        }

        /// <summary>
        /// Initializes the component and sets the metadata for the page.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _mapOptions = new MapOptions
            {
                Zoom = 8,
                Center = new LatLngLiteral(-33.9258, 18.4231),
                MapTypeId = MapTypeId.Roadmap
            };

            var areaResult = await AreaService.AllAreasAsync();
            if (areaResult.Succeeded)
            {
                _availableAreas = areaResult.Data.ToList();
            }

            // Set the image source based on the vacation host's image path
            _imageSource = _vacationHost.ImgPath is null ? "_content/Accomodation.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_vacationHost.ImgPath.TrimStart('/')}";

            await base.OnInitializedAsync();
        }

        #endregion
    }
}

