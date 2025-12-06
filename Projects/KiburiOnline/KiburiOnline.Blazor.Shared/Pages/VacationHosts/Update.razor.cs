using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.VacationHosts
{
    /// <summary>
    /// Component for updating a Vacation Host.
    /// </summary>
    public partial class Update
    {
        private bool _canEditVacationHost;
        private bool _canEditVacationHostImage;
        private bool _canViewVacations;
        private bool _canViewExtensions;
        private bool _canViewTourGuideSchedules;

        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

        private List<AreaDto> _availableAreas = new();
        private Func<AreaDto, string> _areaConverter = p => p?.Name is null ? "" : p.Name;

        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";
        private string? _coverImageToUpload;
        private VacationHostViewModel _vacationHost = new();

        #region Injected Services

        /// <summary>
        /// Gets or sets the service used for image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> contains information about the user's identity
        /// and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation host operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set before
        /// using any functionality that depends on vacation host operations.</remarks>
        [Inject] public IVacationHostService VacationHostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing area-related operations.
        /// </summary>
        [Inject] public IAreaService AreaService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the Vacation Host to be updated.
        /// </summary>
        [Parameter] public string VacationHostId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback that is invoked when the address changes.
        /// </summary>
        [Parameter] public EventCallback<string?> OnAddressChanged { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the change of the cover image.
        /// </summary>
        /// <param name="coverImage">The new cover image path.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Updates the Vacation Host asynchronously.
        /// </summary>
        private async Task UpdateAsync()
        {
            var updateResult = await VacationHostService.UpdateAsync(_vacationHost.ToDto());
            updateResult.ProcessResponseForDisplay(SnackBar, async () =>
            {
                if (!string.IsNullOrEmpty(_coverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{_vacationHost.Name} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (_vacationHost.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await VacationHostService.RemoveImage(_vacationHost.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await VacationHostService.AddImage(new AddEntityImageRequest() { EntityId = _vacationHost.VacationHostId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add($"Action Successful. Vacation Host \"{_vacationHost.Name}\" was successfully updated.", Severity.Success);
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates to the lodgings categories page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/lodgings/categories");
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
            var latLng = new LatLngLiteral(_vacationHost.Lng, _vacationHost.Lat);

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
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        { 
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;

            _canEditVacationHost = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Edit)).Succeeded;
            _canViewVacations = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Delete)).Succeeded;
            _canViewExtensions = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Search)).Succeeded;
            _canEditVacationHostImage = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.VacationHost.Image)).Succeeded;
            _canViewExtensions = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.Extensions.View)).Succeeded;
            _canViewTourGuideSchedules = (await AuthorizationService.AuthorizeAsync(currentUser, Permissions.TourGuidSchedule.View)).Succeeded;

            var areaResult = await AreaService.AllAreasAsync();
            if (areaResult.Succeeded)
            {
                _availableAreas = areaResult.Data.ToList();
            }

            var result = await VacationHostService.VacationHostAsync(VacationHostId);
            _vacationHost = new VacationHostViewModel(result.Data);
            _imageSource = _vacationHost.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) is null ? "_content/Accomodation.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_vacationHost.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).RelativePath.TrimStart('/')}";

            _mapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral(_vacationHost.Lat, _vacationHost.Lng),
                MapTypeId = MapTypeId.Roadmap
            };
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
