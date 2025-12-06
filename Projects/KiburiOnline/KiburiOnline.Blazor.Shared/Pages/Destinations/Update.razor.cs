using System.Net.Http.Headers;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Destinations
{
    /// <summary>
    /// Code-behind for the Update Destination page in the Blazor application.
    /// Handles the update of destination details, including image changes and metadata updates.
    /// </summary>
    public partial class Update
    {
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string _dragClass = DefaultDragClass;
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;

        private string? _coverImageToUpload;
        private ICollection<string> _galleryImageToUpload = new List<string>();
        private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
        private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
        private List<AreaDto> _availableAreas = new();

        private Func<AreaDto, string> _areaConverter = p => p?.Name is null ? "" : p.Name;
        
        private bool uploadInProgress;
        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private Autocomplete _autocomplete = null!;
        private MudTextField<string> _autoCompleteBox = null!;
        private readonly Stack<Marker> _markers = new();

        #region Injected Services

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of the <see cref="ISnackbar"/> service.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
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

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the destination to be updated.
        /// </summary>
        [Parameter] public string DestinationId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// The source URL of the destination's cover image.
        /// </summary>
        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";

        /// <summary>
        /// The view model for the destination being updated.
        /// </summary>
        private DestinationViewModel _destination = new();

        #endregion

        #region Image Methods

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
        private async Task RemoveGalleryImage(string url)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
            {
                var urldd = url.Replace(Configuration["ApiConfiguration:BaseApiAddress"] + "\\", "").Replace("/", "\\");

                var toRemove = _galleryImageToUpload.FirstOrDefault(c => c == urldd);
                if (toRemove == null)
                {
                    SnackBar.AddError("No image found");
                    return;
                }

                var imageRemovalResult = await VacationService.RemoveVacationImage(toRemove);
                imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _galleryImageToUpload.Remove(toRemove);
                    SnackBar.AddSuccess("Image successfully removed");
                });
            }
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

                var response = await VideoProcessingService.UploadVideoAsync(content);
                if (response.Succeeded)
                {
                    SnackBar.Add("Upload successful 🎉", Severity.Success);
                    var additionResult = await VacationService.AddVideo(new AddEntityVideoRequest() { EntityId = _destination.DestinationId, VideoId = response.Data.VideoId });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    else _videosToUpload.Remove(file);
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

        #region Event Handlers

        /// <summary>
        /// Handles the event when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Handles the update action for the destination.
        /// </summary>
        private async Task UpdateAsync()
        {
            var updateResult = await DestinationService.UpdateAsync(_destination.ToDto());
            updateResult.ProcessResponseForDisplay(SnackBar, async () =>
            {
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
                        if (_destination.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await DestinationService.RemoveImage(_destination.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await DestinationService.AddImage(new AddEntityImageRequest() { EntityId = _destination.DestinationId, ImageId = imageUploadResult.Data.Id });
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
                        var additionResult = await DestinationService.AddImage(new AddEntityImageRequest() { EntityId = _destination.DestinationId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                await Upload();

                SnackBar.Add($"Action Successful. Destination \"{_destination.Name}\" was successfully updated.", Severity.Success);
            });
        }

        /// <summary>
        /// Cancels the update and navigates back to the destinations list.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/destinations");
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
            var latLng = new LatLngLiteral(-_destination.Lat, _destination.Lng);

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
        /// Initializes the component and loads the destination details.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var areaResult = await AreaService.AllAreasAsync();
            if (areaResult.Succeeded)
                _availableAreas = areaResult.Data.ToList();

            var result = await DestinationService.DestinationAsync(DestinationId);
            _destination = new DestinationViewModel(result.Data);
            _imageSource = _destination.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover) is null ? "_content/Accomodation.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_destination.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath?.TrimStart('/')}";

            _mapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral(_destination.Lat, _destination.Lng),
                MapTypeId = MapTypeId.Roadmap
            };

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
