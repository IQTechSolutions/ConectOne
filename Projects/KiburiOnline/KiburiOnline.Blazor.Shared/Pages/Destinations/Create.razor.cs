using AccomodationModule.Application.ViewModels;
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

namespace KiburiOnline.Blazor.Shared.Pages.Destinations
{
    /// <summary>
    /// Represents the page for creating a new vacation host in the application.
    /// </summary>
    /// <remarks>This class provides functionality for initializing page metadata, handling user interactions,
    /// and managing the creation process for a new vacation host. It includes dependency-injected services  for
    /// navigation, dialog management, HTTP communication, and notifications.</remarks>
    public partial class Create
    {
        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
        private DestinationViewModel _destination = new();

        private string? _coverImageToUpload;
        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

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
        [Inject] public ISnackbar SnackBar { get; set; } = null!;/// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling destination-related operations.
        /// </summary>
        [Inject] public IDestinationService DestinationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing area-related operations.
        /// </summary>
        [Inject] public IAreaService AreaService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing video-related operations.
        /// </summary>
        [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;
        
        /// <summary>
        /// Updates the cover image for the destination.
        /// </summary>
        /// <remarks>This method sets the image source to the specified cover image and updates the
        /// destination's image path. Ensure the <paramref name="coverImage"/> parameter is valid and
        /// accessible.</remarks>
        /// <param name="coverImage">The file path or URL of the new cover image. Cannot be null or empty.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Creates a new vacation host destination asynchronously and displays a confirmation dialog.
        /// </summary>
        /// <remarks>This method performs the following actions: 1. Sends a request to create a new
        /// destination using the provided data. 2. Displays success or error messages based on the result of the
        /// creation. 3. Shows a confirmation dialog asking whether to add another service. 4. Navigates to the
        /// destinations page or resets the destination data based on the user's choice.</remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateAsync()
        {
            var creationResult = await DestinationService.UpdateAsync(_destination.ToDto());
            if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);

            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_destination.Name} Cover Image",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await DestinationService.AddImage(new AddEntityImageRequest() { EntityId = _destination.DestinationId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Vacation host \"{_destination.Name}\" was successfully created.", Severity.Success);
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
                NavigationManager.NavigateTo($"/destinations/update/{_destination.DestinationId}");
            }
            else
            {
                _destination = new DestinationViewModel();
                _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates to the vacation hosts page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/vacationhosts" route, effectively terminating
        /// the current creation workflow. Ensure that any unsaved changes are handled prior to calling this method, as
        /// navigation will occur immediately.</remarks>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/vacationhosts");
        }

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

                    _destination.Lng = place.Geometry.Location.Lng;
                    _destination.Lat = place.Geometry.Location.Lat;

                    var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
                    {
                        Position = place.Geometry.Location,
                        Map = _map1.InteropObject,
                        Title = place.Name
                    });

                    _markers.Push(marker);

                    _destination.Address = string.Empty;
                    foreach (var component in place.AddressComponents)
                    {
                        var componentType = component.Types[0];

                        switch (componentType)
                        {
                            case "street_number":
                                _destination.Address = _destination.Address + " " + component.LongName;
                                break;
                            case "route":
                                _destination.Address = _destination.Address + " " + component.LongName;
                                break;
                            case "sublocality_level_1":
                            case "sublocality_level_2":
                                _destination.Address = _destination.Address + " " + component.LongName;
                                break;
                            case "postal_code":
                                _destination.Address = _destination.Address + " " + component.LongName;
                                break;
                            case "locality":
                                _destination.Address = _destination.Address + " " + component.LongName;
                                break;
                            case "country":
                                _destination.Address = _destination.Address + " " + component.LongName;
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
        /// Performs initialization logic for the component, setting up metadata and image source values.
        /// </summary>
        /// <remarks>This method configures the page metadata, including breadcrumbs, URL, and SEO
        /// settings,  and initializes the image source based on the destination's image path. It also calls the base 
        /// implementation to ensure proper lifecycle behavior.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            _mapOptions = new MapOptions
            {
                Zoom = 8,
                Center = new LatLngLiteral(-33.9258, 18.4231),
                MapTypeId = MapTypeId.Roadmap
            };

            _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
            
            await base.OnInitializedAsync();
        }
    }
}
