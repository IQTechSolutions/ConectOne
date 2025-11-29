using Microsoft.AspNetCore.Components;

namespace FilingModule.Blazor.Components
{
    /// <summary>
    /// Represents a card component for displaying an image within a gallery, with support for image removal.
    /// </summary>
    public partial class MudGalleryImageCard
    {
        /// <summary>
        /// Gets or sets the image source to display.
        /// </summary>
        [Parameter, EditorRequired] public string Image { get; set; } = null;

        /// <summary>
        /// Gets or sets the callback that is invoked when an image is removed.
        /// </summary>
        /// <remarks>The callback receives the identifier or URL of the image to be removed as its
        /// argument. Use this event to handle image removal logic in the parent component.</remarks>
        [Parameter] public EventCallback<string> OnRemoveImage { get; set; }

        /// <summary>
        /// Invokes the image removal callback for the specified uploaded image.
        /// </summary>
        /// <param name="image">The identifier or path of the uploaded image to be removed. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RemoveOnUploadedImage(string image)
        {
            await OnRemoveImage.InvokeAsync(image);
        }
    }
}
