using System.Net.Http.Headers;
using AccomodationModule.Application.ViewModels;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using KiburiOnline.Blazor.Shared.Pages.Vacations.Modals;
using KiburiOnline.Blazor.Shared.Pages.Vacations.Partials;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Code‑behind for the <c>Gallery.razor</c> component. Handles file selection, image cropping and
    /// asynchronous uploads for the Vacation Gallery feature set. The gallery supports four distinct media buckets:
    /// <list type="bullet">
    /// <item><description><strong>Slider</strong> – hero carousel imagery.</description></item>
    /// <item><description><strong>Banner</strong> – promotional banner imagery.</description></item>
    /// <item><description><strong>Map</strong> – map overlays / location images.</description></item>
    /// <item><description><strong>Video</strong> – marketing or property tour videos.</description></item>
    /// </list>
    /// The component integrates tightly with MudBlazor’s <see cref="MudFileUpload{T}"/> control for client‑side file capture,
    /// <see cref="IBaseHttpProvider"/> for RESTful persistence and <see cref="IDialogService"/> for user interaction dialogs.
    /// </summary>
    /// <remarks>
    /// • All long‑running work (file uploads) is executed on background threads via <see cref="Parallel.ForEachAsync"/> to maintain a responsive UI.<br/>
    /// • Progress feedback is surfaced to the user through the <see cref="UploadResult"/> collection bound to MudBlazor progress bars.<br/>
    /// • The class purposely contains <c>private</c> helpers only; the public surface area is limited to dependency injected properties.
    /// </remarks>
    public partial class Gallery
    {
        #region === Private State Flags & Constants ===

        /// <summary>
        /// Indicates if the modal gallery view should be rendered.
        /// </summary>
        private bool _visible = false;

        /// <summary>
        /// Default <see cref="CancellationToken"/> shared by all background upload tasks when the component is alive.
        /// </summary>
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        /// <summary>
        /// Reusable CSS class applied to drag‑and‑drop zones when idle.
        /// </summary>
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        #endregion

        #region === File / Media Collections ===

        // <editor‑fold desc="FileUpload Components References">
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _sliderBrowserFilesUpload;
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _bannerBrowserFilesUpload;
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _mapBrowserFilesUpload;
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
        // </editor‑fold>

        // <editor‑fold desc="Image Gallery Grid References">
        private VacationGalleryGrid _sliderImageGallery = null!;
        private VacationGalleryGrid _bannerImageGallery = null!;
        private VacationGalleryGrid _mapImageGallery = null!;
        // </editor‑fold>

        // <editor‑fold desc="Drag & Drop CSS Class State">
        private string _sliderDragClass = DefaultDragClass;
        private string _bannerDragClass = DefaultDragClass;
        private string _mapDragClass = DefaultDragClass;
        private string _dragClass = DefaultDragClass;
        // </editor‑fold>

        // <editor‑fold desc="Client‑side staging collections – display filenames only">
        private readonly ICollection<string> _sliderImagesToUploadForDisplay = [];
        private readonly ICollection<string> _bannerImagesToUploadForDisplay = [];
        private readonly ICollection<string> _mapImagesToUploadForDisplay = [];
        // </editor‑fold>

        // <editor‑fold desc="Client‑side staging collections – Base‑64 payloads from MudCropper">
        private readonly ICollection<MudCropperResponse> _sliderBas64FilesUpload = [];
        private readonly ICollection<MudCropperResponse> _bannerBas64FilesUpload = [];
        private readonly ICollection<MudCropperResponse> _mapBas64FilesUpload = [];
        // </editor‑fold>

        // <editor‑fold desc="Client‑side staging collections – raw browser files">
        private readonly ICollection<IBrowserFile> _sliderImagesToUpload = [];
        private readonly ICollection<IBrowserFile> _bannerImagesToUpload = [];
        private readonly ICollection<IBrowserFile> _mapImagesToUpload = [];
        private readonly ICollection<IBrowserFile> _videosToUpload = [];
        // </editor‑fold>

        // <editor‑fold desc="Upload progress trackers (bound to UI)">
        private readonly ICollection<UploadResult> _sliderImagesBusyUploading = [];
        private readonly ICollection<UploadResult> _bannerImagesBusyUploading = [];
        private readonly ICollection<UploadResult> _mapImagesBusyUploading = [];
        private readonly ICollection<UploadResult> _filesBusyUploading = [];
        // </editor‑fold>

        // <editor‑fold desc="Concurrency guards">
        private bool _sliderUploadInProgress;
        private bool _bannerUploadInProgress;
        private bool _mapUploadInProgress;
        private bool _uploadInProgress;
        // </editor‑fold>

        #endregion

        #region === Dependency Injection ===

        /// <summary>
        /// Modal dialog service from MudBlazor.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Snackbar notification provider for non‑blocking status messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Application <see cref="IConfiguration"/> for environment specific settings (e.g. max upload size).
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing video-related operations.
        /// </summary>
        [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing images.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        #endregion

        #region === Public Lifecycle ===

        /// <summary>
        /// Asynchronously initializes the component and loads the required data.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. It loads video data asynchronously and ensures the base class initialization logic is
        /// executed.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
          //  _videos = await LoadVideos();
            await base.OnInitializedAsync();
        }

        #endregion

        #region === Upload Entry‑Points (Slider / Banner / Map) ===

        /// <summary>
        /// Initiates the upload pipeline for <em>slider</em> images. The method merges raw browser files and cropped Base‑64 payloads into a unified
        /// asynchronous work queue and tracks progress for each item individually.
        /// </summary>
        private async Task UploadSliderImages()
        {
            if (_sliderUploadInProgress) return; // Prevent UI double‑click re‑entrancy.
            _sliderUploadInProgress = true;

            try
            {
                var uploadTasks = new List<Task>
                {
                    Parallel.ForEachAsync(_sliderImagesToUpload.ToList(), _cancellationToken, UploadSliderBrowserFileAsync),
                    Parallel.ForEachAsync(_sliderBas64FilesUpload.ToList(), _cancellationToken, UploadSliderBase64Async)
                };

                await Task.WhenAll(uploadTasks);
            }
            catch (Exception ex)
            {
                SnackBar.Add($"Unexpected error: {ex.Message}", Severity.Error);
            }
            finally { _sliderUploadInProgress = false; }
        }

        /// <summary>
        /// Upload handler for <em>banner</em> images.
        /// </summary>
        private async Task UploadBannerImages()
        {
            if (_bannerUploadInProgress) return;
            _bannerUploadInProgress = true;
            try
            {
                var uploadTasks = new List<Task>
                {
                    Parallel.ForEachAsync(_bannerImagesToUpload.ToList(), _cancellationToken, UploadBannerBrowserFileAsync),
                    Parallel.ForEachAsync(_bannerBas64FilesUpload.ToList(), _cancellationToken, UploadBannerBase64Async)
                };
                await Task.WhenAll(uploadTasks);
            }
            finally { _bannerUploadInProgress = false; }
        }

        /// <summary>
        /// Upload handler for <em>map</em> images.
        /// </summary>
        private async Task UploadMapImages()
        {
            if (_mapUploadInProgress) return;
            _mapUploadInProgress = true;
            try
            {
                var uploadTasks = new List<Task>
                {
                    Parallel.ForEachAsync(_mapImagesToUpload.ToList(), _cancellationToken, UploadMapBrowserFileAsync),
                    Parallel.ForEachAsync(_mapBas64FilesUpload.ToList(), _cancellationToken, UploadMapBase64Async)
                };
                await Task.WhenAll(uploadTasks);
            }
            finally { _mapUploadInProgress = false; }
        }

        #endregion

        #region === Image Cropper Dialogs ===

        /// <summary>
        /// Opens an image cropper dialog for the user to crop an image and updates the slider with the cropped image.
        /// </summary>
        /// <remarks>This method displays a modal dialog for cropping an image with a predefined aspect
        /// ratio of 8:5.  If the user completes the cropping operation without canceling, the cropped image is added to
        /// the slider's collection.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ChangeSliderCropperImageAsync()
        {
            var parameters = new DialogParameters<MudCropperModal>
            {
                { x => x.Src, "images/NoSliderImageAvailable.jpg" },
                { x => x.AspectRatio, "1920/921" },
            };
            var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var imageToAdd = (MudCropperResponse)result.Data;

                var uploadModel = new Base64ImageUploadRequest()
                {
                    Base64String = imageToAdd.Base64String,
                    Featured = false,
                    ImageType = UploadType.Slider,
                    Name = Guid.NewGuid().ToString(),
                    Order = 0,
                    Selector = ""
                };
                await ImageProcessingService.UploadImage(uploadModel, _cancellationToken);
                await _sliderImageGallery.ReloadImages();

                StateHasChanged();
            }
        }

        /// <summary>
        /// Opens a dialog to allow the user to crop an image for the banner.
        /// </summary>
        /// <remarks>This method displays a modal dialog with a cropping tool, preloaded with a default
        /// image and a specified aspect ratio. If the user completes the cropping operation without canceling, the
        /// cropped image data is added to the banner upload list.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task ChangeBannerCropperImageAsync()
        {
            var parameters = new DialogParameters<MudCropperModal>
            {
                { x => x.Src, "_content/FilingModule.Blazor/NoBannerImageAvailable.jpg" },
                { x => x.AspectRatio, "1024/253" }
            };
            var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var imageToAdd = (MudCropperResponse)result.Data;

                _bannerBas64FilesUpload.Add(imageToAdd);
                _bannerImagesToUploadForDisplay.Add(imageToAdd.File.Name);
            }
        }

        /// <summary>
        /// Opens a modal dialog to allow the user to crop an image and updates the collection of cropped images if the
        /// operation is successful.
        /// </summary>
        /// <remarks>This method displays a cropping dialog with a predefined image and aspect ratio. If
        /// the user completes the cropping operation,  the resulting cropped image is added to the internal collection
        /// of uploaded images.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task ChangeMapCropperImageAsync()
        {
            var parameters = new DialogParameters<MudCropperModal>
            {
                { x => x.Src, "_content/FilingModule.Blazor/NoImage.jpg" },
                { x => x.AspectRatio, "624/500" }
            };
            var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var imageToAdd = (MudCropperResponse)result.Data;

                _mapBas64FilesUpload.Add(imageToAdd);
                _mapImagesToUploadForDisplay.Add(imageToAdd.File.Name);
            }
        }

        #endregion

        #region === Confirmation Modals ===

        /// <summary>
        /// Presents a confirmation dialog asking the user to choose how an image should be used (e.g., featured, selector, etc.).
        /// Currently the returned <see cref="VacationGallerySetAsViewModel"/> is captured for future implementation.
        /// </summary>
        private async Task SetAs()
        {
            var dialog = await DialogService.ShowAsync<VacationGallerySetAsModal>("Confirm");
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var model = (VacationGallerySetAsViewModel)result.Data;
                // TODO: Persist model to API once server‑side endpoint is available.
            }
        }

        #endregion

        #region === Data Loading Helpers ===

        /// <summary>
        /// Retrieves all images for the current vacation context from the back‑end API.
        /// </summary>
        private async Task<IEnumerable<ImageDto>> LoadAllVacationImages()
        {
            var imageResult = await ImageProcessingService.AllImagesAsync();
            return imageResult.Succeeded ? imageResult.Data : [];
        }

        /// <summary>
        /// Retrieves the list of server‑side stored video metadata used for the gallery’s video section.
        /// </summary>
        private async Task<List<VideoDto>> LoadVideos()
        {
            var videoResult = await VideoProcessingService.AllVideosAsync();
            return videoResult.Succeeded ? videoResult.Data.ToList() : [];
        }

        #endregion

        #region === Deletion ===

        /// <summary>
        /// Permanently removes an image from the server after explicit user confirmation.
        /// </summary>
        private async Task DeleteImage(ImageDto image)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this image from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var deleteResult = await ImageProcessingService.DeleteImageAsync(image.Id);
                if (deleteResult.Succeeded)
                {
                    if(_sliderImageGallery != null)
                        await _sliderImageGallery?.ReloadImages();
                    if (_bannerImageGallery != null)
                        await _bannerImageGallery?.ReloadImages();
                    if (_mapImageGallery != null)
                        await _mapImageGallery?.ReloadImages();
                }
                else
                    SnackBar.Add("Failed to delete image", Severity.Error);
            }
        }

        #endregion

        #region === <input type="file"/> Change Handlers ===

        /// <summary>
        /// Handles the change event for the slider input file element, processing the selected files.
        /// </summary>
        /// <remarks>This method clears any drag-and-drop visual effects, validates the size of each
        /// selected file,  and adds valid files to the upload and display lists. Files that do not meet the size
        /// requirements  are ignored.</remarks>
        /// <param name="e">The event arguments containing information about the selected files.</param>
        /// <returns></returns>
        private async Task OnSliderInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearSliderDragClass();

            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                if (!ValidateFileSize(file)) continue;
                _sliderImagesToUpload.Add(file);
                _sliderImagesToUploadForDisplay.Add(file.Name);
            }
        }

        /// <summary>
        /// Handles the event triggered when a file is selected or changed in the banner input field.
        /// </summary>
        /// <remarks>This method processes the selected files by validating their size and adding them to
        /// the  collections for upload and display. Files that do not meet the size requirements are ignored.</remarks>
        /// <param name="e">The event arguments containing information about the selected files.</param>
        /// <returns></returns>
        private async Task OnBannerInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearBannerDragClass();

            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                if (!ValidateFileSize(file)) continue;
                _bannerImagesToUpload.Add(file);
                _bannerImagesToUploadForDisplay.Add(file.Name);
            }
        }

        /// <summary>
        /// Handles the event triggered when the input file for the map changes.
        /// </summary>
        /// <remarks>This method processes the uploaded files by validating their size and adding them to
        /// the internal collections  for further processing and display. Files that do not meet the size requirements
        /// are ignored.</remarks>
        /// <param name="e">The event arguments containing details about the file change, including the uploaded files.</param>
        /// <returns></returns>
        private async Task OnMapInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearMapDragClass();

            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                if (!ValidateFileSize(file)) continue;
                _mapImagesToUpload.Add(file);
                _mapImagesToUploadForDisplay.Add(file.Name);
            }
        }

        /// <summary>
        /// Handles the event triggered when a file is selected or changed in the input file control.
        /// </summary>
        /// <remarks>This method validates the selected file's size and, if valid, adds it to the upload
        /// queue.  It also clears any drag-and-drop visual indicators.</remarks>
        /// <param name="e">The event arguments containing information about the selected file.</param>
        /// <returns></returns>
        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            if (!ValidateFileSize(e.File)) return;
            _videosToUpload.Add(e.File);
        }

        /// <summary>
        /// Validates that the file does not exceed the configured upload limit (500&nbsp;MB).
        /// </summary>
        private bool ValidateFileSize(IBrowserFile file)
        {
            const long maxBytes = 500 * 1024 * 1024;
            if (file.Size > maxBytes)
            {
                SnackBar.Add("File too large (limit 500 MB)", Severity.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region === Drag‑drop Visual Helpers ===
        
        /// <summary>
        /// Sets the CSS class for the slider's drag operation, combining the default drag class with a primary border
        /// style.
        /// </summary>
        /// <remarks>This method updates the internal drag class used for visual styling during
        /// drag-and-drop operations on the slider.</remarks>
        private void SetSliderDragClass() => _sliderDragClass = $"{DefaultDragClass} mud-border-primary";

        /// <summary>
        /// Sets the CSS class for the banner drag element, combining the default drag class with a primary border
        /// style.
        /// </summary>
        /// <remarks>This method updates the internal drag class used for styling the banner during
        /// drag-and-drop operations. The resulting class includes the default drag class and a "mud-border-primary"
        /// style.</remarks>
        private void SetBannerDragClass() => _bannerDragClass = $"{DefaultDragClass} mud-border-primary";

        /// <summary>
        /// Sets the CSS class used for map dragging, combining the default drag class with a primary border style.
        /// </summary>
        /// <remarks>This method updates the internal drag class to include a primary border style, which
        /// can be used to visually indicate the dragging state of the map.</remarks>
        private void SetMapDragClass() => _mapDragClass = $"{DefaultDragClass} mud-border-primary";

        /// <summary>
        /// Sets the drag class to a default value combined with a primary border style.
        /// </summary>
        /// <remarks>This method updates the internal drag class used for styling drag-and-drop elements.
        /// The resulting class string combines the default drag class with a primary border modifier.</remarks>
        private void SetDragClass() => _dragClass = $"{DefaultDragClass} mud-border-primary";

        /// <summary>
        /// Resets the slider's drag class to its default value.
        /// </summary>
        /// <remarks>This method restores the drag class to the default state, which can be used to reset
        /// any customizations or temporary changes applied during slider interactions.</remarks>
        private void ClearSliderDragClass() => _sliderDragClass = DefaultDragClass;

        /// <summary>
        /// Resets the banner drag class to its default value.
        /// </summary>
        /// <remarks>This method clears any custom drag class applied to the banner and restores it to the
        /// default drag class.</remarks>
        private void ClearBannerDragClass() => _bannerDragClass = DefaultDragClass;

        /// <summary>
        /// Resets the map drag class to its default value.
        /// </summary>
        /// <remarks>This method restores the drag class to the default state, which may be used to reset
        /// any custom drag behavior applied to the map.</remarks>
        private void ClearMapDragClass() => _mapDragClass = DefaultDragClass;

        /// <summary>
        /// Resets the drag class to its default value.
        /// </summary>
        /// <remarks>This method clears any custom drag class that may have been set and reverts it to the
        /// default drag class.</remarks>
        private void ClearDragClass() => _dragClass = DefaultDragClass;
        #endregion

        #region === File Picker Helpers ===

        /// <summary>
        /// Opens the slider file picker asynchronously, allowing the user to select files.
        /// </summary>
        /// <remarks>If no file picker is available, the method completes immediately without performing
        /// any action.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private Task OpenSliderPickerAsync() => _sliderBrowserFilesUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

        /// <summary>
        /// Opens a file picker dialog for selecting banner files asynchronously.
        /// </summary>
        /// <remarks>If the file picker is not available, the method completes immediately without
        /// performing any action.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task completes when the file picker dialog is closed
        /// or if no action is performed.</returns>
        private Task OpenBannerPickerAsync() => _bannerBrowserFilesUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

        /// <summary>
        /// Opens a file picker dialog for selecting map files.
        /// </summary>
        /// <remarks>This method invokes the file picker associated with the map browser.  If no file
        /// picker is available, the method completes without performing any action.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task is completed immediately  if no file picker is
        /// available.</returns>
        private Task OpenMapPickerAsync() => _mapBrowserFilesUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

        /// <summary>
        /// Opens a file picker dialog asynchronously, allowing the user to select a file.
        /// </summary>
        /// <remarks>If a file upload component is available, this method invokes its file picker
        /// functionality.  If no file upload component is present, the method completes without performing any
        /// action.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task is completed immediately  if no
        /// file upload component is available.</returns>
        private Task OpenFilePickerAsync() => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;
        #endregion

        #region === Reset / Clear ===

        /// <summary>
        /// Resets the state of the slider picker by clearing all associated collections and resetting the drag class.
        /// </summary>
        /// <remarks>This method clears the internal collections used for managing slider images and
        /// resets any drag-related state. It should be called to ensure the slider picker is in a clean state before
        /// reuse.</remarks>
        protected void ResetSliderPicker()
        {
            _sliderImagesToUploadForDisplay.Clear();
            _sliderBas64FilesUpload.Clear();
            _sliderImagesToUpload.Clear();
            ClearSliderDragClass();
        }

        /// <summary>
        /// Resets the state of the banner picker by clearing all associated collections and resetting the drag class.
        /// </summary>
        /// <remarks>This method clears the internal collections used for managing banner images and
        /// resets any drag-and-drop styling  applied to the banner picker. It should be called to prepare the banner
        /// picker for a new set of banner images.</remarks>
        protected void ResetBannerPicker()
        {
            _bannerImagesToUploadForDisplay.Clear();
            _bannerBas64FilesUpload.Clear();
            _bannerImagesToUpload.Clear();
            ClearBannerDragClass();
        }

        /// <summary>
        /// Resets the state of the map picker by clearing all associated collections and removing any drag-related
        /// styling.
        /// </summary>
        /// <remarks>This method clears internal collections used for managing map images and resets any
        /// visual drag-and-drop indicators. It should be called to reinitialize the map picker to its default
        /// state.</remarks>
        protected void ResetMapPicker()
        {
            _mapImagesToUploadForDisplay.Clear();
            _mapBas64FilesUpload.Clear();
            _mapImagesToUpload.Clear();
            ClearMapDragClass();
        }

        /// <summary>
        /// Asynchronously clears the current file upload state, resets the list of videos to upload,  and removes any
        /// drag-and-drop visual indicators.
        /// </summary>
        /// <remarks>This method ensures that any ongoing file upload operations are cleared, the internal
        /// collection of videos to upload is emptied, and any drag-and-drop UI state is reset.  It is safe to call this
        /// method multiple times.</remarks>
        /// <returns>A task that represents the asynchronous clear operation.</returns>
        private async Task ClearAsync()
        {
            await (_fileUpload?.ClearAsync() ?? Task.CompletedTask);
            _videosToUpload.Clear();
            ClearDragClass();
        }

        #endregion

        #region === Private Upload Helpers (BrowserFile & Base64) ===
        
        /// <summary>
        /// Asynchronously uploads a slider image file provided by the browser.
        /// </summary>
        /// <remarks>This method processes the provided browser file, validates its size, and uploads it
        /// as a slider image. The maximum allowed file size is 500 MB. If the upload fails, an error message is
        /// displayed in the UI.</remarks>
        /// <param name="file">The browser file to upload. Must not be null.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async ValueTask UploadSliderBrowserFileAsync(IBrowserFile file, CancellationToken ct)
        {
            var result = InitialiseSliderUploadResult(file.Size);
            try
            {
                await using var stream = file.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteSliderImageUploadAsync(stream, file.Name, file.ContentType, UploadType.Slider, result, ct);
                _sliderImagesToUpload.Remove(file);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _sliderImagesBusyUploading.Remove(result); }
        }

        /// <summary>
        /// Uploads a banner image file provided by the browser asynchronously.
        /// </summary>
        /// <remarks>The method processes the file upload by reading the file stream and executing the
        /// upload operation. The maximum allowed file size is 500 MB. If the upload fails, an error message is
        /// displayed.</remarks>
        /// <param name="file">The browser file to be uploaded. Must not be null.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async ValueTask UploadBannerBrowserFileAsync(IBrowserFile file, CancellationToken ct)
        {
            var result = InitialiseBannerUploadResult(file.Size);
            try
            {
                await using var stream = file.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteBannerImageUploadAsync(stream, file.Name, file.ContentType, UploadType.Banner, result, ct);
                _bannerImagesToUpload.Remove(file);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _bannerImagesBusyUploading.Remove(result); }
        }

        /// <summary>
        /// Uploads a map file from the browser asynchronously.
        /// </summary>
        /// <remarks>The method processes the provided file by opening a read stream and uploading its
        /// content. The maximum allowed file size is 500 MB. If the upload fails and the operation is not canceled, an
        /// error message is displayed to the user.</remarks>
        /// <param name="file">The browser file to be uploaded. Must not be null.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async ValueTask UploadMapBrowserFileAsync(IBrowserFile file, CancellationToken ct)
        {
            var result = InitialiseMapUploadResult(file.Size);
            try
            {
                await using var stream = file.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteMapImageUploadAsync(stream, file.Name, file.ContentType, UploadType.Map, result, ct);
                _mapImagesToUpload.Remove(file);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _mapImagesBusyUploading.Remove(result); }
        }

        /// <summary>
        /// Batch uploads all files currently staged in <see cref="_videosToUpload"/>.
        /// </summary>
        private async Task Upload()
        {
            _uploadInProgress = true;

            await Parallel.ForEachAsync(_videosToUpload.ToList(), async (file, cancellationToken) =>
            {
                var uploadResult = new UploadResult { Progress = 0, TotalBytes = file.Size };
                _filesBusyUploading.Add(uploadResult);

                await using var stream = file.OpenReadStream(500 * 1024 * 1024);
                using var content = new MultipartFormDataContent();
                var streamContent = new ProgressableStreamContent(stream, 64 * 1024, uploaded => uploadResult.Progress = (int)(uploaded * 100 / uploadResult.TotalBytes));
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "video", file.Name);

                var response = await VideoProcessingService.UploadVideoAsync(content, cancellationToken);
                if (response.Succeeded)
                {
                    SnackBar.Add("Upload successful 🎉", Severity.Success);
                    _videosToUpload.Remove(file);
                }
                else
                {
                    SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
                }

                _filesBusyUploading.Remove(uploadResult);
            });

            _uploadInProgress = false;
        }

        /// <summary>
        /// Uploads a slider image from a Base64-encoded string asynchronously.
        /// </summary>
        /// <remarks>This method processes the Base64-encoded string into a byte array, uploads the image
        /// to the server,  and updates the internal state to reflect the upload progress. If an error occurs during the
        /// upload,  a notification is displayed to the user.</remarks>
        /// <param name="tuple">The <see cref="MudCropperResponse"/> containing the Base64-encoded string, file metadata, and other relevant
        /// information.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the operation to complete.  If the token is
        /// canceled, the operation will terminate early.</param>
        /// <returns></returns>
        private async ValueTask UploadSliderBase64Async(MudCropperResponse tuple, CancellationToken ct)
        {
            var result = InitialiseSliderUploadResult(tuple.File.Size);
            try
            {
                if (tuple.Base64String.Contains(","))
                {
                    tuple.Base64String = tuple.Base64String.Remove(0, tuple.Base64String.LastIndexOf(",")).TrimStart(',');

                }

                var bytes = Convert.FromBase64String(tuple.Base64String);
                await using var ms = new MemoryStream(bytes);
                await ExecuteSliderImageUploadAsync(ms, tuple.File.Name, tuple.File.ContentType, UploadType.Slider, result, ct);
                _sliderBas64FilesUpload.Remove(tuple);
                _sliderImagesToUploadForDisplay.Clear();
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _sliderImagesBusyUploading.Remove(result); }
        }

        /// <summary>
        /// Uploads a banner image from a Base64-encoded string asynchronously.
        /// </summary>
        /// <remarks>This method processes the Base64-encoded string to extract the image data, uploads
        /// the image to the server,  and updates the internal state to reflect the upload progress. If the operation
        /// fails and the cancellation token  has not been triggered, an error message is displayed to the
        /// user.</remarks>
        /// <param name="tuple">The <see cref="MudCropperResponse"/> containing the Base64-encoded string, file metadata, and other relevant
        /// information.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
        /// <returns></returns>
        private async ValueTask UploadBannerBase64Async(MudCropperResponse tuple, CancellationToken ct)
        {
            var result = InitialiseBannerUploadResult(tuple.File.Size);
            try
            {
                var bytes = Convert.FromBase64String(tuple.Base64String);
                await using var ms = new MemoryStream(bytes);
                await ExecuteBannerImageUploadAsync(ms, tuple.File.Name, tuple.File.ContentType, UploadType.Banner, result, ct);
                _bannerBas64FilesUpload.Remove(tuple);
                _bannerImagesToUploadForDisplay.Clear();
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _bannerImagesBusyUploading.Remove(result); }
        }

        /// <summary>
        /// Uploads a map image from a Base64-encoded string asynchronously.
        /// </summary>
        /// <remarks>This method processes the Base64-encoded string to extract the image data, uploads
        /// the image to the server,  and manages the upload state. If the operation fails and the cancellation token is
        /// not triggered, an error  message is displayed to the user.</remarks>
        /// <param name="tuple">The <see cref="MudCropperResponse"/> containing the Base64-encoded string, file metadata, and other relevant
        /// information.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async ValueTask UploadMapBase64Async(MudCropperResponse tuple, CancellationToken ct)
        {
            var result = InitialiseMapUploadResult(tuple.File.Size);
            try
            {
                var bytes = Convert.FromBase64String(tuple.Base64String);
                await using var ms = new MemoryStream(bytes);
                await ExecuteMapImageUploadAsync(ms, tuple.File.Name, tuple.File.ContentType, UploadType.Map, result, ct);
                _mapBas64FilesUpload.Remove(tuple);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                SnackBar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
            finally { _mapImagesBusyUploading.Remove(result); }
        }

        #endregion

        #region === Execute Upload to API ===
        
        /// <summary>
        /// Uploads a slider image to the API asynchronously.
        /// </summary>
        /// <remarks>This method performs the upload operation for slider images and updates the provided
        /// <paramref name="uiResult"/> with the outcome. The method also interacts with the slider image gallery and
        /// clears any drag-related UI classes upon completion.</remarks>
        /// <param name="stream">The <see cref="Stream"/> containing the image data to upload. Must not be null.</param>
        /// <param name="fileName">The name of the file being uploaded. This is used to identify the image on the server.</param>
        /// <param name="contentType">The MIME type of the image (e.g., "image/jpeg" or "image/png").</param>
        /// <param name="uploadType">The type of upload being performed, represented by the <see cref="UploadType"/> enumeration.</param>
        /// <param name="uiResult">An <see cref="UploadResult"/> object that will be updated with the result of the upload operation.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns></returns>
        private async Task ExecuteSliderImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, CancellationToken ct)
        {
            await ExecuteImageUploadAsync(stream, fileName, contentType, uploadType, uiResult, ct, _sliderImageGallery, ClearSliderDragClass);
        }

        /// <summary>
        /// Uploads a banner image asynchronously to the specified gallery.
        /// </summary>
        /// <remarks>This method is intended for uploading banner images to a specific gallery. It ensures
        /// that the image is processed according to the provided <paramref name="uploadType"/> and updates the UI state
        /// as necessary.</remarks>
        /// <param name="stream">The <see cref="Stream"/> containing the image data to upload. Must not be null.</param>
        /// <param name="fileName">The name of the file being uploaded. This is used to identify the image in the gallery.</param>
        /// <param name="contentType">The MIME type of the image (e.g., "image/jpeg" or "image/png"). Must be a valid image content type.</param>
        /// <param name="uploadType">The type of upload operation to perform. Determines how the image is processed or stored.</param>
        /// <param name="uiResult">An object representing the result of the upload operation. This will be updated with the outcome of the
        /// upload.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        private async Task ExecuteBannerImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, CancellationToken ct)
        {
            await ExecuteImageUploadAsync(stream, fileName, contentType, uploadType, uiResult, ct, _bannerImageGallery, ClearBannerDragClass);
        }

        /// <summary>
        /// Uploads a map image asynchronously to the specified gallery.
        /// </summary>
        /// <remarks>This method handles the upload of map images to a predefined gallery. It ensures that
        /// the image is processed according to the specified <paramref name="uploadType"/> and updates the provided
        /// <paramref name="uiResult"/> with the outcome of the operation.</remarks>
        /// <param name="stream">The <see cref="Stream"/> containing the image data to upload. Must not be null.</param>
        /// <param name="fileName">The name of the file being uploaded. This is used to identify the image in the gallery.</param>
        /// <param name="contentType">The MIME type of the image (e.g., "image/png" or "image/jpeg").</param>
        /// <param name="uploadType">The type of upload being performed, which determines how the image is processed or categorized.</param>
        /// <param name="uiResult">An object to store the result of the upload operation, including any status or error information.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. Can be used to cancel
        /// the operation.</param>
        private async Task ExecuteMapImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, CancellationToken ct)
        {
            await ExecuteImageUploadAsync(stream, fileName, contentType, uploadType, uiResult, ct, _mapImageGallery, ClearMapDragClass);
        }

        /// <summary>
        /// Core routine that constructs the multipart form, posts to <c>images/upload</c> and updates the provided gallery on success.
        /// </summary>
        private async Task ExecuteImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, CancellationToken ct, VacationGalleryGrid targetGallery, Action clearDragState)
        {
            //using var content = new MultipartFormDataContent();
            //var progressContent = new ProgressableStreamContent(stream, bufferSize: 64 * 1024, progress => uiResult.Progress = (int)(progress * 100 / uiResult.TotalBytes));
            //progressContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            //// Required API contract fields
            //content.Add(progressContent, "File", fileName);
            //content.Add(new StringContent(fileName), "Name");
            //content.Add(new StringContent("false"), "Featured");
            //content.Add(new StringContent(string.Empty), "Selector");
            //content.Add(new StringContent("0"), "Order");
            //content.Add(new StringContent(((int)uploadType).ToString()), "ImageType");

            //var response = await Provider.PostingAsync<ImageDto>("images/upload", content);
            //if (response.Succeeded)
            //{
            //    SnackBar.Add("Upload successful 🎉", Severity.Success);
            //    await targetGallery.ReloadImages();
            //    clearDragState();
            //}
            //else
            //{
            //    SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            //}
        }
        #endregion

        #region === UploadResult Factory ===

        /// <summary>
        /// Initializes a new <see cref="UploadResult"/> instance for tracking the progress of a slider image upload.
        /// </summary>
        /// <remarks>The created <see cref="UploadResult"/> instance is added to the internal collection
        /// of uploads in progress.</remarks>
        /// <param name="totalBytes">The total number of bytes to be uploaded for the slider image.</param>
        /// <returns>A new <see cref="UploadResult"/> instance with the progress set to 0 and the specified total byte count.</returns>
        private UploadResult InitialiseSliderUploadResult(long totalBytes)
        {
            var ui = new UploadResult { Progress = 0, TotalBytes = totalBytes };
            _sliderImagesBusyUploading.Add(ui);
            return ui;
        }

        /// <summary>
        /// Initializes a new <see cref="UploadResult"/> instance for tracking the progress of a banner image upload.
        /// </summary>
        /// <remarks>The initialized <see cref="UploadResult"/> is added to the internal collection of
        /// banner images currently being uploaded.</remarks>
        /// <param name="totalBytes">The total size of the file to be uploaded, in bytes.</param>
        /// <returns>An <see cref="UploadResult"/> object with the initial progress set to 0 and the specified total byte size.</returns>
        private UploadResult InitialiseBannerUploadResult(long totalBytes)
        {
            var ui = new UploadResult { Progress = 0, TotalBytes = totalBytes };
            _bannerImagesBusyUploading.Add(ui);
            return ui;
        }

        /// <summary>
        /// Initializes a new <see cref="UploadResult"/> instance for tracking the progress of a map upload.
        /// </summary>
        /// <remarks>The initialized <see cref="UploadResult"/> is added to the internal collection of
        /// active uploads.</remarks>
        /// <param name="totalBytes">The total number of bytes to be uploaded.</param>
        /// <returns>An <see cref="UploadResult"/> object with the initial progress set to 0 and the specified total byte count.</returns>
        private UploadResult InitialiseMapUploadResult(long totalBytes)
        {
            var ui = new UploadResult { Progress = 0, TotalBytes = totalBytes };
            _mapImagesBusyUploading.Add(ui);
            return ui;
        }

        #endregion
    }
}
