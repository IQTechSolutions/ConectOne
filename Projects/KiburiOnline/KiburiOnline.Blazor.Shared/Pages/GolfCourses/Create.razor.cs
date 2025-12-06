using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments;
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

namespace KiburiOnline.Blazor.Shared.Pages.GolfCourses
{
    /// <summary>
    /// Component for creating a new golf course.
    /// </summary>
    public partial class Create
    {
        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
        private GolfCourseViewModel _golfCourse = new();
        private List<DestinationDto> _availableDestinations = [];
        private readonly Func<DestinationDto?, string> _destinationConverter = p => p?.Name ?? "";

        private string? _coverImageToUpload;
        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

        #region Parameters

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
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The <see cref="Configuration"/> property is typically used to retrieve application
        /// settings and configuration values, such as connection strings, API keys, or other environment-specific
        /// settings. Ensure that the property is properly initialized before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve golf course data.
        /// </summary>
        [Inject] public IGolfCoursesService GolfCoursesService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve golf course data.
        /// </summary>
        [Inject] public IDestinationService DestinationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing images.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Handles the change event for the cover image.
        /// </summary>
        /// <param name="coverImage">The new cover image path.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Creates a new golf course with the current data.
        /// </summary>
        private async Task CreateAsync()
        {
            var creationResult = await GolfCoursesService.CreateAsync(_golfCourse.ToDto());
            if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);

            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_golfCourse.Name} Cover Image",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_golfCourse.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await GolfCoursesService.RemoveImage(_golfCourse.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await GolfCoursesService.AddImage(new AddEntityImageRequest() { EntityId = _golfCourse.GolfCourseId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Golf Course \"{_golfCourse.Name}\" was successfully created.", Severity.Success);
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Would you like to add another course?" },
                { x => x.ButtonText, "Yes" },
                { x => x.CancelButtonText, "No" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (result!.Canceled)
            {
                NavigationManager.NavigateTo("/golfcourses");
            }
            else
            {
                _golfCourse = new GolfCourseViewModel();
                _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
                StateHasChanged();
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the golf courses page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/golfcourses");
        }

        #endregion

        #region Lifecycle Methods

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
            try
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

                        _golfCourse.Lng = place.Geometry.Location.Lng;
                        _golfCourse.Lat = place.Geometry.Location.Lat;

                        var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
                        {
                            Position = place.Geometry.Location,
                            Map = _map1.InteropObject,
                            Title = place.Name
                        });

                        _markers.Push(marker);

                        _golfCourse.Address = string.Empty;
                        foreach (var component in place.AddressComponents)
                        {
                            if (component.Types.Any() && component.Types[0] != null)
                            {
                                var componentType = component.Types[0];

                                switch (componentType)
                                {
                                    case "street_number":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                    case "route":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                    case "sublocality_level_1":
                                    case "sublocality_level_2":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                    case "postal_code":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                    case "locality":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                    case "country":
                                        _golfCourse.Address = _golfCourse.Address + " " + component.LongName;
                                        break;
                                }
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
            catch (Exception e)
            {
                SnackBar.AddError(e.Message);
            }

           
        }

        /// <summary>
        /// Method invoked when the component is initialized.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var pagingResponse = await DestinationService.PagedDestinationsAsync(new DestinationPageParameters() { PageSize = 100 });
            if (pagingResponse.Succeeded)
                _availableDestinations = pagingResponse.Data;

            _mapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral(-33.9258, 18.4231),
                MapTypeId = MapTypeId.Roadmap
            };

            _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
