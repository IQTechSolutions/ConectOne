using System.Globalization;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Cropper.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace FilingModule.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for cropping images using the Cropper.Blazor component library.
    /// </summary>
    /// <remarks>
    /// This component replaces the legacy JavaScript-powered cropper implementation and relies on
    /// the strongly-typed <see cref="CropperComponent"/> provided by the Cropper.Blazor NuGet package.
    /// </remarks>
    public partial class MudCropperModal : IAsyncDisposable
    {
        private readonly Options _options = new() { CheckCrossOrigin = false };
        private CropperComponent? _cropperComponent;
        private IBrowserFile? _file;
        private string? _currentSrc;
        private string? _objectUrl;
        private bool _isCropperReady;
        private bool _isSubmitting;
        private bool _hasValidCropSelection;
        private string? _submitErrorMessage;
        private ElementReference _modalContentRef;
        private DotNetObjectReference<MudCropperModal>? _modalVisibilityReference;
        private int _cropperRenderVersion;
        private bool _isModalVisibilityObserverRegistered;
        private bool _isReinitializingCropper;
        private bool _triggeredVisibilityFallback;
        private bool _renderCropper;

        /// <summary>
        /// Gets a value indicating whether there is a current image source available.
        /// </summary>
        private bool HasCurrentImage => !string.IsNullOrWhiteSpace(_currentSrc);

        /// <summary>
        /// Gets a value indicating whether the cropper can be reset to its initial state.
        /// </summary>
        private bool CanReset => _cropperComponent is not null && _isCropperReady && HasCurrentImage;

        /// <summary>
        /// Gets a value indicating whether the current image can be submitted for processing.
        /// </summary>
        private bool CanSubmit =>
            _cropperComponent is not null &&
            _isCropperReady &&
            !_isSubmitting &&
            HasCurrentImage &&
            _hasValidCropSelection;

        /// <summary>
        /// Gets or sets the current instance of the dialog being managed by the component.
        /// </summary>
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Provides access to the Cropper.Blazor image streaming utilities.
        /// </summary>
        [Inject] public IUrlImageInterop UrlImageInterop { get; set; } = null!;

        /// <summary>
        /// Provides access to the JavaScript runtime for modal lifecycle coordination.
        /// </summary>
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the source URL of the media or resource.
        /// </summary>
        [Parameter, EditorRequired] public string Src { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the aspect ratio of the component, expressed as a string (for example "4/3" or "1.777").
        /// </summary>
        [Parameter] public string AspectRatio { get; set; } = "4/3";

        /// <summary>
        /// Handles updates when component parameters are set or changed.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework after parameters are set. Override this
        /// method to respond to parameter changes, such as updating internal state or recalculating values based on new
        /// parameters. This method is invoked before the component is rendered.</remarks>
        protected override void OnParametersSet()
        {
            var normalizedSrc = string.IsNullOrWhiteSpace(Src) ? null : Src;
            if (!string.Equals(_currentSrc, normalizedSrc, StringComparison.Ordinal))
            {
                _currentSrc = normalizedSrc;
                _isCropperReady = false;
                _hasValidCropSelection = false;
            }

            _options.AspectRatio = ParseAspectRatio(AspectRatio);
        }

        /// <summary>
        /// Invoked after the component has rendered. Registers a JavaScript observer to monitor the visibility of the
        /// modal content if it has not already been registered.
        /// </summary>
        /// <remarks>If the JavaScript runtime is not connected when this method is called, the observer
        /// registration will be retried on subsequent renders. If observer registration fails due to a JavaScript
        /// error, a fallback mechanism is triggered to handle modal visibility.</remarks>
        /// <param name="firstRender">Indicates whether this is the first time the component has rendered. This value is true on the first render;
        /// otherwise, false.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_isModalVisibilityObserverRegistered)
            {
                return;
            }

            try
            {
                if (_modalContentRef.Equals(default))
                {
                    return;
                }

                _modalVisibilityReference ??= DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync(
                    "mudCropperModal.observeVisibility",
                    _modalContentRef,
                    _modalVisibilityReference);
                _isModalVisibilityObserverRegistered = true;
            }
            catch (JSDisconnectedException)
            {
                // The circuit is not connected yet; Blazor will call OnAfterRenderAsync again once the connection is established.
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Failed to register modal visibility observer: {ex.Message}");
                _isModalVisibilityObserverRegistered = true;

                if (!_triggeredVisibilityFallback)
                {
                    _triggeredVisibilityFallback = true;
                    await HandleModalShownAsync();
                }
            }
        }

        /// <summary>
        /// Closes the dialog and cancels the current operation.
        /// </summary>
        /// <remarks>This method should be called to dismiss the dialog when the user cancels or aborts
        /// the action. Any changes made in the dialog will not be applied.</remarks>
        private void Close()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Processes the current crop selection and submits the cropped image data asynchronously.
        /// </summary>
        /// <remarks>If the crop selection is invalid or no image is available, the method returns without
        /// submitting. Any errors encountered during image processing or retrieval are captured and reported to the
        /// user. This method is intended to be called in response to a user action, such as clicking a submit button in
        /// an image cropping dialog.</remarks>
        /// <returns>A task that represents the asynchronous submit operation. The task completes when the submission process
        /// finishes.</returns>
        private async Task SubmitAsync()
        {
            if (_cropperComponent is null || !HasCurrentImage)
            {
                return;
            }

            _submitErrorMessage = null;
            _isSubmitting = true;
            StateHasChanged();

            try
            {
                if (!await EnsureValidCropSelectionAsync())
                {
                    return;
                }

                const string imageType = "image/jpeg";
                var canvasOptions = new GetCroppedCanvasOptions
                {
                    MaxWidth = 4096,
                    MaxHeight = 4096,
                    FillColor = "#fff",
                };

                var imageReceiver = await _cropperComponent.GetCroppedCanvasDataInBackgroundAsync(
                    canvasOptions,
                    imageType,
                    number: 1f,
                    maximumReceiveChunkSize: 60 * 1024,
                    cancellationToken: CancellationToken.None);

                await using var imageStream = await imageReceiver.GetImageChunkStreamAsync(CancellationToken.None);
                if (imageStream.Length == 0)
                {
                    _submitErrorMessage = "Cropper did not return any image data. Please adjust your selection and try again.";
                    Console.Error.WriteLine("Cropper completed without providing image data.");
                    return;
                }

                var imageBytes = imageStream.ToArray();
                if (imageBytes.Length == 0)
                {
                    _submitErrorMessage = "Cropper returned an empty image payload. Please adjust your selection and try again.";
                    Console.Error.WriteLine("Cropper returned an empty payload during submission.");
                    return;
                }

                var base64Data = Convert.ToBase64String(imageBytes);
                if (string.IsNullOrWhiteSpace(base64Data))
                {
                    _submitErrorMessage = "Cropper returned empty image data. Please adjust your selection and try again.";
                    Console.Error.WriteLine("Cropper returned an empty base64 string during submission.");
                    return;
                }

                var response = new MudCropperResponse
                {
                    File = _file,
                    Base64String = $"data:{imageType};base64,{base64Data}",
                };

                MudDialog.Close(DialogResult.Ok(response));
            }
            catch (ImageProcessingException ex)
            {
                _submitErrorMessage = ex.Message;
                Console.Error.WriteLine($"Failed to process cropped image: {ex.Message}");
            }
            catch (JSException ex)
            {
                _submitErrorMessage = ex.Message;
                Console.Error.WriteLine($"Failed to retrieve cropped canvas data: {ex.Message}");
            }
            finally
            {
                _isSubmitting = false;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Handles the file change event by updating the component state and preparing the selected image for further
        /// processing.
        /// </summary>
        /// <remarks>This method revokes any previously created object URLs before generating a new one
        /// for the selected image. If an error occurs during image processing or object URL creation, an error message
        /// is set and the component state is updated accordingly.</remarks>
        /// <param name="args">The event arguments containing information about the newly selected file. The <see
        /// cref="InputFileChangeEventArgs.File"/> property must not be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnFileChangedAsync(InputFileChangeEventArgs args)
        {
            if (args.File is null)
            {
                return;
            }

            _file = args.File;
            _isCropperReady = false;
            _hasValidCropSelection = false;
            _submitErrorMessage = null;
            StateHasChanged();

            if (!string.IsNullOrWhiteSpace(_objectUrl))
            {
                await UrlImageInterop.RevokeObjectUrlAsync(_objectUrl);
                _objectUrl = null;
            }

            string? newObjectUrl = null;

            try
            {
                newObjectUrl = await UrlImageInterop.GetImageUsingStreamingAsync(_file, _file.Size);
            }
            catch (ImageProcessingException ex)
            {
                _submitErrorMessage = ex.Message;
                Console.Error.WriteLine($"Failed to stream selected image: {ex.Message}");
            }
            catch (JSException ex)
            {
                _submitErrorMessage = ex.Message;
                Console.Error.WriteLine($"Failed to create an object URL for the selected image: {ex.Message}");
            }

            if (string.IsNullOrWhiteSpace(newObjectUrl))
            {
                StateHasChanged();
                return;
            }

            _objectUrl = newObjectUrl;
            _currentSrc = newObjectUrl;

            await RebuildCropperAsync();
        }

        /// <summary>
        /// Asynchronously resets the cropper component to its initial state if an image is currently loaded.
        /// </summary>
        /// <returns>A completed task that represents the asynchronous reset operation.</returns>
        private Task ResetAsync()
        {
            if (_cropperComponent is null || !HasCurrentImage)
            {
                return Task.CompletedTask;
            }

            _hasValidCropSelection = false;
            _submitErrorMessage = null;
            _cropperComponent.Reset();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the event indicating that the cropper component is ready and updates the component state
        /// accordingly.
        /// </summary>
        /// <remarks>This method is typically invoked in response to a JavaScript event signaling that the
        /// cropper UI is ready for user interaction. It updates internal state flags and may trigger UI updates based
        /// on the readiness and validity of the crop selection.</remarks>
        /// <param name="eventData">The event data associated with the cropper readiness event. Contains information about the cropper's
        /// initialization state.</param>
        private void OnCropperReady(JSEventData<CropReadyEvent> eventData)
        {
            _isCropperReady = HasCurrentImage;
            if (!_isCropperReady)
            {
                _hasValidCropSelection = false;
                _submitErrorMessage = null;
                _ = InvokeAsync(StateHasChanged);
                return;
            }

            _ = InvokeAsync(async () =>
            {
                await TryRefreshCropSelectionAsync();
                if (_hasValidCropSelection)
                {
                    _submitErrorMessage = null;
                }
                StateHasChanged();
            });
        }

        /// <summary>
        /// Handles changes to the crop selection event data and updates the component state accordingly.
        /// </summary>
        /// <remarks>If the crop selection becomes valid, any existing submit error message is cleared and
        /// the component is re-rendered to reflect the updated state.</remarks>
        /// <param name="eventData">The event data containing details about the crop selection. Cannot be null.</param>
        private void OnCropChanged(JSEventData<CropEvent> eventData)
        {
            var hasCropSelection = HasPositiveCropSelection(eventData?.Detail);
            if (_hasValidCropSelection != hasCropSelection)
            {
                _hasValidCropSelection = hasCropSelection;
                if (hasCropSelection)
                {
                    _submitErrorMessage = null;
                }
                _ = InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Asynchronously releases all resources used by the component.
        /// </summary>
        /// <remarks>Call this method to clean up resources such as JavaScript interop objects and object
        /// URLs when the component is no longer needed. This method should be called instead of Dispose when using
        /// asynchronous resource cleanup in Blazor components.</remarks>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DestroyCropperAsync();

            if (_isModalVisibilityObserverRegistered)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("mudCropperModal.disposeVisibilityObserver", _modalContentRef);
                }
                catch (JSException ex)
                {
                    Console.Error.WriteLine($"Failed to dispose modal visibility observer: {ex.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(_objectUrl))
            {
                await UrlImageInterop.RevokeObjectUrlAsync(_objectUrl);
                _objectUrl = null;
            }

            _modalVisibilityReference?.Dispose();
            _modalVisibilityReference = null;
        }

        /// <summary>
        /// Ensures that a valid crop selection exists before proceeding with image submission.
        /// </summary>
        /// <remarks>If no valid crop selection is available, an error message is set and the operation
        /// cannot proceed. This method should be called before attempting to submit an image to ensure user input is
        /// valid.</remarks>
        /// <returns>true if a valid crop selection is present or successfully refreshed; otherwise, false.</returns>
        private async Task<bool> EnsureValidCropSelectionAsync()
        {
            if (_hasValidCropSelection)
            {
                return true;
            }

            var hasSelection = await TryRefreshCropSelectionAsync();
            if (!hasSelection)
            {
                _submitErrorMessage = "Select a valid portion of the image before submitting.";
                Console.Error.WriteLine("Cannot submit image without a valid crop selection.");
            }

            return hasSelection;
        }

        /// <summary>
        /// Attempts to refresh the current crop selection by retrieving the latest crop data from the cropper
        /// component.
        /// </summary>
        /// <remarks>If the cropper component is unavailable or an error occurs while retrieving crop
        /// data, the crop selection is considered invalid and the method returns false.</remarks>
        /// <returns>true if a valid crop selection with positive width and height is available; otherwise, false.</returns>
        private async Task<bool> TryRefreshCropSelectionAsync()
        {
            if (_cropperComponent is null)
            {
                _hasValidCropSelection = false;
                return false;
            }

            try
            {
                var cropData = await _cropperComponent.GetDataAsync(rounded: false, cancellationToken: CancellationToken.None);
                _hasValidCropSelection = cropData is { Width: > 0m, Height: > 0m };
                if (_hasValidCropSelection)
                {
                    _submitErrorMessage = null;
                }
                return _hasValidCropSelection;
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Failed to read crop selection data: {ex.Message}");
                _hasValidCropSelection = false;
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified crop event represents a positive crop selection with both width and height
        /// greater than zero.
        /// </summary>
        /// <param name="detail">The crop event to evaluate. May be null.</param>
        /// <returns>true if the crop event is not null and both its width and height are greater than zero; otherwise, false.</returns>
        private static bool HasPositiveCropSelection(CropEvent? detail) => detail is { Width: > 0m, Height: > 0m };

        /// <summary>
        /// Parses a string representation of an aspect ratio and returns its decimal value, if valid.
        /// </summary>
        /// <remarks>The method supports both direct decimal representations and fractional forms
        /// separated by a forward slash ('/'). Parsing uses invariant culture. Returns null if the input is null,
        /// empty, whitespace, or not a valid aspect ratio.</remarks>
        /// <param name="aspectRatio">A string containing the aspect ratio to parse. This can be a decimal value (e.g., "1.78") or a fractional
        /// value in the form "width/height" (e.g., "16/9").</param>
        /// <returns>A decimal value representing the aspect ratio if parsing is successful; otherwise, null.</returns>
        private static decimal? ParseAspectRatio(string? aspectRatio)
        {
            if (string.IsNullOrWhiteSpace(aspectRatio))
            {
                return null;
            }

            if (decimal.TryParse(aspectRatio, NumberStyles.Any, CultureInfo.InvariantCulture, out var directRatio) && directRatio > 0)
            {
                return directRatio;
            }

            var separatorIndex = aspectRatio.IndexOf('/');
            if (separatorIndex > 0 && separatorIndex < aspectRatio.Length - 1)
            {
                var numeratorText = aspectRatio[..separatorIndex];
                var denominatorText = aspectRatio[(separatorIndex + 1)..];

                if (decimal.TryParse(numeratorText, NumberStyles.Any, CultureInfo.InvariantCulture, out var numerator) &&
                    decimal.TryParse(denominatorText, NumberStyles.Any, CultureInfo.InvariantCulture, out var denominator) &&
                    denominator != 0)
                {
                    return numerator / denominator;
                }
            }

            return null;
        }

        /// <summary>
        /// Handles the event when the modal dialog is shown and ensures the cropper component is properly initialized.
        /// </summary>
        /// <remarks>This method is intended to be invoked from JavaScript via interop when the modal is
        /// displayed. It prevents concurrent reinitialization of the cropper component if a previous initialization is
        /// still in progress.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [JSInvokable] public Task HandleModalShownAsync()
        {
            return InvokeAsync(async () =>
            {
                if (_isReinitializingCropper)
                {
                    return;
                }

                _isReinitializingCropper = true;

                try
                {
                    await RebuildCropperAsync();
                }
                finally
                {
                    _isReinitializingCropper = false;
                }
            });
        }

        /// <summary>
        /// Asynchronously resets and reinitializes the cropper component to ensure it is in a clean state.
        /// </summary>
        /// <remarks>Call this method when the cropper needs to be fully rebuilt, such as after
        /// configuration changes or error recovery. The method ensures that any previous cropper instance is destroyed
        /// before reinitialization.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RebuildCropperAsync()
        {
            await DestroyCropperAsync();

            _isCropperReady = false;
            _hasValidCropSelection = false;
            _submitErrorMessage = null;

            if (_renderCropper)
            {
                _renderCropper = false;
                StateHasChanged();
                await Task.Yield();
            }

            _cropperRenderVersion++;
            _renderCropper = true;
            StateHasChanged();
        }

        /// <summary>
        /// Asynchronously releases resources associated with the cropper component, if it has been initialized.
        /// </summary>
        /// <remarks>If the cropper component has not been initialized, this method performs no action.
        /// This method is safe to call multiple times; subsequent calls after the cropper has been destroyed have no
        /// effect.</remarks>
        /// <returns>A completed task that represents the asynchronous destroy operation.</returns>
        private Task DestroyCropperAsync()
        {
            if (_cropperComponent is null)
            {
                return Task.CompletedTask;
            }

            try
            {
                _cropperComponent.Destroy(CancellationToken.None);
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Failed to destroy cropper instance: {ex.Message}");
            }
            finally
            {
                _cropperComponent = null;
            }

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Represents the result of a cropping operation performed by a MudCropper component, including the cropped file
    /// and its Base64-encoded string representation.
    /// </summary>
    public class MudCropperResponse
    {
        /// <summary>
        /// Gets or sets the file selected by the user.
        /// </summary>
        public IBrowserFile? File { get; set; }

        /// <summary>
        /// Gets or sets the value as a Base64-encoded string.
        /// </summary>
        public string? Base64String { get; set; }
    }
}
