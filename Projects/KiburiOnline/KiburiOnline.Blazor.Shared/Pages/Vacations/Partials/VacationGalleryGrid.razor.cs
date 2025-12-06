using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    /// <summary>
    /// Represents a responsive grid component for displaying and managing a collection of vacation images.
    /// </summary>
    /// <remarks>This component allows users to load, display, and interact with images in a gallery format. 
    /// It supports actions such as removing an image or setting an image as a featured item.  The grid layout is
    /// configurable for different screen sizes using the <see cref="xs"/>, <see cref="md"/>, <see cref="lg"/>, and <see
    /// cref="xl"/> parameters.</remarks>
    public partial class VacationGalleryGrid
    {
        private IEnumerable<ImageDto>? _images = [];

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the delegate used to asynchronously load a collection of images.
        /// </summary>
        /// <remarks>The delegate should return an <see cref="IEnumerable{T}"/> of <see
        /// cref="ImageDto"/> objects when the task completes. This property is typically used to provide a
        /// custom image-loading mechanism for components or services.</remarks>
        [Parameter] public Func<Task<IEnumerable<ImageDto>>> LoadImages { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when an image is removed.
        /// </summary>
        /// <remarks>Use this property to handle the removal of images in the component. The callback is
        /// invoked  with the relevant image data when the removal action occurs.</remarks>
        [Parameter] public EventCallback<ImageDto> RemoveImage { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked to set the specified image as the active display.
        /// </summary>
        [Parameter] public EventCallback<ImageDto> SetAs { get; set; }

        /// <summary>
        /// Gets or sets the action menu content to be rendered for an image display.
        /// </summary>
        [Parameter] public RenderFragment<ImageDto>? ActionMenu { get; set; }

        /// <summary>
        /// Gets or sets the type of image to be uploaded.
        /// </summary>
        [Parameter] public UploadType ImageType { get; set; }

        /// <summary>
        /// Gets or sets the value representing the size or scale factor for the component.
        /// </summary>
        [Parameter] public int xs { get; set; } = 4;

        /// <summary>
        /// Gets or sets the margin density value for the component.
        /// </summary>
        [Parameter] public int md { get; set; } = 4;

        /// <summary>
        /// Gets or sets the value representing the large breakpoint size in a responsive layout.
        /// </summary>
        [Parameter] public int lg { get; set; } = 4;

        /// <summary>
        /// Gets or sets the value of the 'xl' parameter.
        /// </summary>
        [Parameter] public int xl { get; set; } = 4;

        /// <summary>
        /// Sets the specified image as the active display image.
        /// </summary>
        /// <remarks>This method invokes the <see cref="SetAs"/> event asynchronously, passing the
        /// provided image. Ensure that the <paramref name="image"/> parameter is valid and properly initialized before
        /// calling this method.</remarks>
        /// <param name="image">The image to be set as the active display. Cannot be null.</param>
        public async Task OnSetAs(ImageDto image)
        {
            await SetAs.InvokeAsync(image);
        }

        /// <summary>
        /// Handles the removal of an image by invoking the associated callback.
        /// </summary>
        /// <remarks>This method triggers the <see cref="RemoveImage"/> callback with the specified image.
        /// Ensure that the <paramref name="image"/> parameter is valid and properly initialized before calling this
        /// method.</remarks>
        /// <param name="image">The image to be removed. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnRemoveImage(ImageDto image)
        {
            await RemoveImage.InvokeAsync(image);
        }

        /// <summary>
        /// Asynchronously reloads the image collection by invoking the configured image loading delegate.
        /// </summary>
        /// <remarks>Use this method to refresh the current set of images. The method completes when the
        /// image collection has been updated. If the image loading delegate is not set, this method may throw an
        /// exception.</remarks>
        /// <returns>A task that represents the asynchronous reload operation.</returns>
        public async Task ReloadImages()
        {
            _images = await LoadImages.Invoke();
        }

        /// <summary>
        /// Asynchronously performs initialization logic when the component is first initialized.
        /// </summary>
        /// <remarks>This method is invoked automatically by the Blazor framework during the component's
        /// lifecycle. It loads images by invoking the <see cref="LoadImages"/> delegate.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            _images = await LoadImages.Invoke();
            StateHasChanged();
        }
    }
}
