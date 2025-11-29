using FilingModule.Domain.Enums;

namespace FilingModule.Domain.Entities
{
    /// <summary>
    /// Represents an image file with additional metadata, such as whether it is featured and its type.
    /// </summary>
    /// <remarks>The <see cref="Image"/> class extends <see cref="FileBase"/> to include properties specific
    /// to image files. Use the <see cref="Featured"/> property to indicate if the image is highlighted, and the <see
    /// cref="ImageType"/>  property to specify the type of the image (e.g., JPEG, PNG).</remarks>
    public class Image : FileBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        public UploadType ImageType { get; set; }
    }
}
