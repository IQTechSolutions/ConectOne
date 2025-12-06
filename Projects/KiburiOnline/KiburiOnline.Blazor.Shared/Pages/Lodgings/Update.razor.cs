using System.Net.Http.Headers;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Interfaces;
using LocationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ProductsModule.Domain.Interfaces;

namespace KiburiOnline.Blazor.Shared.Pages.Lodgings
{
    /// <summary>
    /// Code-behind for the Update Lodging page in the Blazor application.
    /// Handles the update of lodging details, including image uploads and metadata updates.
    /// </summary>
    public partial class Update
    {
        private LodgingViewModel _lodging = new();
        private bool _loaded;

        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string _dragClass = DefaultDragClass;
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;

        private string? _coverImageToUpload;
        private ICollection<string> _galleryImageToUpload = new List<string>();
        private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
        private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();

        private bool uploadInProgress;

        private List<AreaDto> _availableAreas = new();
        private List<LodgingTypeDto> _availableLodgingTyoes = [];
        private List<DestinationDto> _availableDestinations = [];

        private readonly Func<LodgingTypeDto?, string> _lodgingTypeConverter = p => p?.Name ?? "";
        private readonly Func<DestinationDto?, string> _destinationConverter = p => p?.Name ?? "";
        private Func<AreaDto, string> _areaConverter = p => p?.Name is null ? "" : p.Name;

        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

        private List<CountryDto> _availableCountries = [];
        private readonly Func<CountryDto?, string?> _countryConverter = p => p?.Name;

        #region Injected Services

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing lodging-related operations.
        /// </summary>
        [Inject] public ILodgingService LodgingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling operations related to services.
        /// </summary>
        [Inject] public IServiceService ServiceService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing images.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing video-related operations.
        /// </summary>
        [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing room data operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the service is properly configured in the dependency injection container before accessing this
        /// property.</remarks>
        [Inject] public IRoomDataService RoomDataService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling destination-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the  dependency is properly configured in the service container before accessing this
        /// property.</remarks>
        [Inject] public IDestinationService DestinationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing country-related operations.
        /// </summary>
        [Inject] public ICountryService CountryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing lodging types.
        /// </summary>
        [Inject] public ILodgingTypeService LodgingTypeService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the lodging to be updated.
        /// </summary>
        [Parameter] public string LodgingId { get; set; } = null!;

        #endregion

        #region Image Upload Methods

        /// <summary>
        /// Removes an uploaded image after confirmation.
        /// </summary>
        /// <param name="url">The URL of the image to remove.</param>
        private async Task RemoveOnUploadedImage(string url)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
            {
                var toRemove = _galleryImageToUpload.FirstOrDefault(c => c == url.Replace(Configuration["ApiConfiguration:BaseApiAddress"], ""));
                if (toRemove == null)
                {
                    SnackBar.AddError("No image found");
                    return;
                }

                _galleryImageToUpload.Remove(toRemove);
                SnackBar.AddSuccess("Image successfully removed");
            }
        }

        /// <summary>
        /// Removes a gallery image after confirmation.
        /// </summary>
        /// <param name="url">The URL of the image to remove.</param>
        private async Task RemoveGalleryImage(string imageId)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
            {
                var imageRemovalResult = await LodgingService.RemoveImage(imageId);
                imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _lodging.Details.Images.Remove(_lodging.Details.Images.FirstOrDefault(c => c.Id == imageId));
                    SnackBar.AddSuccess("Image successfully removed");
                });
            }
        }

        /// <summary>
        /// Uploads an image using a modal dialog.
        /// </summary>
        private async Task UploadImageAsync()
        {
            var parameters = new DialogParameters<MudCropperModal> { { x => x.Src, "/_content/NeuralTech.Blazor/images/NoImage.jpg" } };

            var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var galleryImage = (MudCropperResponse)result.Data;

                _galleryImageToUpload.Add(galleryImage.Base64String);
            }
        }

        /// <summary>
        /// Removes a gallery image after confirmation.
        /// </summary>
        /// <param name="url">The URL of the image to remove.</param>
        private async Task RemoveVideo(string videoId)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this Video from this lodging?"))
            {
                var imageRemovalResult = await LodgingService.RemoveVideo(videoId);
                imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _lodging.Details.Videos.Remove(_lodging.Details.Videos.FirstOrDefault(c => c.Id == videoId));
                    SnackBar.AddSuccess("Video successfully removed");
                });
            }
        }

        /// <summary>
        /// Updates the cover image to be uploaded with the specified image data.
        /// </summary>
        /// <param name="coverImage">The response containing the base64-encoded string of the new cover image.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _coverImageToUpload = coverImage.Base64String;
        }

        #endregion

        #region Video Methods

        /// <summary>
        /// Clears the current state of the file upload process, including uploaded files and drag-and-drop styling.
        /// </summary>
        /// <remarks>This method resets the file upload component by clearing any uploaded files, removing
        /// drag-and-drop styling,  and ensuring the component is ready for new uploads. If no file upload operation is
        /// active, the method completes without action.</remarks>
        /// <returns>A task that represents the asynchronous clear operation.</returns>
        private async Task ClearAsync()
        {
            await (_fileUpload?.ClearAsync() ?? Task.CompletedTask);
            _videosToUpload.Clear();
            ClearDragClass();
        }

        /// <summary>
        /// Handles the event triggered when the input file changes.
        /// </summary>
        /// <remarks>This method processes multiple files provided in the event arguments and updates the
        /// internal file name collection. It also clears any drag-related visual state before processing the
        /// files.</remarks>
        /// <param name="e">An <see cref="InputFileChangeEventArgs"/> instance containing information about the changed input file(s).</param>
        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();

            const long maxBytes = 500 * 1024 * 1024;
            if (e.File.Size > maxBytes)
            {
                SnackBar.Add("File too large (limit 500 MB)", Severity.Error);
                return;
            }

            _videosToUpload.Add(e.File);
        }

        /// <summary>
        /// Uploads a collection of video files asynchronously to the server.
        /// </summary>
        /// <remarks>This method processes the video files in parallel, uploading each file while tracking
        /// its progress.  If an upload succeeds, the video is added to the destination collection. If an upload fails,
        /// an error  message is displayed. The method ensures that the upload progress is updated in real-time and
        /// handles  large files efficiently by limiting the read stream size.</remarks>
        /// <returns></returns>
        private async Task Upload()
        {
            uploadInProgress = true;

            await Parallel.ForEachAsync(_videosToUpload.ToList(), async (file, cancellationToken) =>
            {
                var uploadResult = new UploadResult
                {
                    Progress = 0,
                    TotalBytes = file.Size
                };

                _filesBusyUploading.Add(uploadResult);

                await using var stream = file.OpenReadStream(500 * 1024 * 1024); // limit server-side too
                using var content = new MultipartFormDataContent();
                var streamContent = new ProgressableStreamContent(stream, 64 * 1024, (uploaded) => uploadResult.Progress = (int)(uploaded * 100 / uploadResult.TotalBytes));
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "file", file.Name);

                var response = await VideoProcessingService.UploadVideoAsync(content, cancellationToken);
                if (response.Succeeded)
                {
                    SnackBar.Add("Upload successful 🎉", Severity.Success);

                    var additionResult = await LodgingService.AddVideo(new AddEntityVideoRequest() { EntityId = _lodging.Details.LodgingId, VideoId = response.Data.VideoId }, cancellationToken);
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    _videosToUpload.Remove(file);
                }

                else
                    SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);

                _filesBusyUploading.Remove(uploadResult);
            });

            uploadInProgress = false;
        }

        /// <summary>
        /// Updates the drag class to include the default drag class and a primary border style.    
        /// </summary>
        /// <remarks>This method sets the drag class to a combination of the default drag class and a
        /// predefined  primary border style. It is typically used to visually indicate a drag-and-drop
        /// operation.</remarks>
        private void SetDragClass() => _dragClass = $"{DefaultDragClass} mud-border-primary";

        /// <summary>
        /// Resets the drag class to its default value.
        /// </summary>
        /// <remarks>This method clears any custom drag class and restores the default drag class. It is
        /// typically used to reset the state after a drag-and-drop operation.</remarks>
        private void ClearDragClass() => _dragClass = DefaultDragClass;

        /// <summary>
        /// Opens a file picker dialog asynchronously, allowing the user to select a file.
        /// </summary>
        /// <remarks>If no file upload component is available, the method completes immediately without
        /// performing any action.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task completes when the file picker dialog
        /// is closed or if no file upload component is available.</returns>
        private Task OpenFilePickerAsync() => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

        #endregion

        #region Rooms

        /// <summary>
        /// Initiates the process to create a new room by displaying a dialog for user input and  sending the room data
        /// to the server if confirmed.
        /// </summary>
        /// <remarks>This method displays a modal dialog to the user to input room details. If the user 
        /// confirms the dialog, the room data is sent to the server for creation. Upon successful  creation, the room
        /// is added to the current lodging's room list, and a success message  is displayed. If the creation fails,
        /// error messages are shown.</remarks>
        /// <returns></returns>
        private async Task CreateNewRoom()
        {
            var dialog = await DialogService.ShowAsync<RoomModal>("Confirm");
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var roomModel = ((RoomViewModel)result.Data).ToDto() with { LodgingId = _lodging.LodgingId };

                var creationResult = await RoomDataService.CreateRoomAsync(roomModel);
                if(creationResult.Succeeded)
                {
                    _lodging.Rooms.Add(roomModel);
                    SnackBar.AddSuccess("Room created successfully");
                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        /// <summary>
        /// Updates the specified room with new details provided by the user through a confirmation dialog.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user, allowing them to modify the
        /// room details. If the user confirms the changes, the room is updated in the lodging's room list and a success
        /// message is displayed. If the update fails, error messages are shown to the user.</remarks>
        /// <param name="room">The <see cref="RoomDto"/> object representing the room to be updated.</param>
        /// <returns></returns>
        private async Task UpdateRoom(RoomDto room)
        {
            var parameters = new DialogParameters<RoomModal>
            {
                { x => x.Room, new RoomViewModel(room) }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var index = _lodging.Rooms.IndexOf(room);

                var roomModel = ((RoomViewModel)result.Data).ToDto() with { LodgingId = _lodging.LodgingId };
                var updateResult = await RoomDataService.UpdateRoomAsync(roomModel);

                if (updateResult.Succeeded)
                {
                    _lodging.Rooms[index] = roomModel;
                    SnackBar.AddSuccess("Room created successfully");
                }
                else
                {
                    SnackBar.AddErrors(updateResult.Messages);
                }
            }
        }

        /// <summary>
        /// Removes the specified room from the current lodging after user confirmation.
        /// </summary>
        /// <remarks>This method prompts the user for confirmation before proceeding with the removal.  If
        /// confirmed, it attempts to delete the room using the specified provider and updates the lodging's room
        /// list.</remarks>
        /// <param name="room">The room to be removed, represented by a <see cref="RoomDto"/> object.</param>
        /// <returns></returns>
        private async Task RemoveRoom(RoomDto room)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this room from this lodging?"))
            {
                var imageRemovalResult = await RoomDataService.RemoveRoomAsync(room.RoomTypeId.Value);
                imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _lodging.Rooms.Remove(room);
                    SnackBar.AddSuccess("Video successfully removed");
                });
            }
        }

        #endregion

        #region Update and Cancel

        /// <summary>
        /// Updates the lodging details.
        /// </summary>
        private async Task UpdateAsync()
        {
            var result = await LodgingService.UpdateLodgingAsync(_lodging.ToDto());
            result.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    if (!string.IsNullOrEmpty(_coverImageToUpload))
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = $"{_lodging.Details.Name} Cover Image",
                            ImageType = UploadType.Cover,
                            Base64String = _coverImageToUpload
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            if (_lodging.Details.Images.Any(c => c.ImageType == UploadType.Cover))
                            {
                                var removalResult = await LodgingService.RemoveImage(_lodging.Details.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                            }
                            var additionResult = await LodgingService.AddImage(new AddEntityImageRequest() { EntityId = _lodging.Details.LodgingId, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }

                    foreach (var galleryImage in _galleryImageToUpload)
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = Guid.NewGuid().ToString(),
                            ImageType = UploadType.Image,
                            Base64String = galleryImage
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            var additionResult = await LodgingService.AddImage(new AddEntityImageRequest() { EntityId = _lodging.Details.LodgingId, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }

                    await Upload();

                    SnackBar.AddSuccess($"Action Successful. \"{_lodging.Details.Name}\" was successfully updated.");
                });
        }

        /// <summary>
        /// Cancels the update process and navigates back to the lodgings page.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/lodgings");
        }

        #endregion

        #region Initialization

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
            var latLng = new LatLngLiteral(_lodging.Address.Lat, _lodging.Address.Lng);

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

                    _lodging.Address.Lng = place.Geometry.Location.Lng;
                    _lodging.Address.Lat = place.Geometry.Location.Lat;

                    var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
                    {
                        Position = place.Geometry.Location,
                        Map = _map1.InteropObject,
                        Title = place.Name
                    });

                    _markers.Push(marker);

                    _lodging.Address.Address = string.Empty;
                    foreach (var component in place.AddressComponents)
                    {
                        var componentType = component.Types[0];

                        switch (componentType)
                        {
                            case "street_number":
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
                                break;
                            case "route":
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
                                break;
                            case "sublocality_level_1":
                            case "sublocality_level_2":
                                _lodging.Address.Suburb = component.LongName;
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
                                break;
                            case "postal_code":
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
                                break;
                            case "locality":
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
                                break;
                            case "country":
                                _lodging.Address.Address = _lodging.Address.Address + " " + component.LongName;
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
        /// Initializes the component and loads the lodging details.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await LodgingService.LodgingAsync(LodgingId);
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                //var areaResult = await Provider.GetAsync<IEnumerable<AreaDto>>("areas");
                //if (areaResult.Succeeded)
                //    _availableAreas = areaResult.Data.ToList();

                _lodging = new LodgingViewModel(result.Data);

                _mapOptions = new MapOptions
                {
                    Zoom = 13,
                    Center = new LatLngLiteral(_lodging.Address.Lat, _lodging.Address.Lng),
                    MapTypeId = MapTypeId.Roadmap
                };

                var pagingResponse = await DestinationService.PagedDestinationsAsync(new DestinationPageParameters() { PageSize = 100 });
                if (pagingResponse.Succeeded)
                    _availableDestinations = pagingResponse.Data;

                var countriesResult = await CountryService.PagedCountriesAsync(new CountryPageParameters() { PageSize = 100 });
                if (countriesResult.Succeeded)
                    _availableCountries = countriesResult.Data.ToList();

                var lodgingTypesResponse = await LodgingTypeService.AllLodgingTypesAsync();
                if (lodgingTypesResponse.Succeeded)
                    _availableLodgingTyoes = lodgingTypesResponse.Data!.ToList();

                StateHasChanged();
            });

            _loaded = true;
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
