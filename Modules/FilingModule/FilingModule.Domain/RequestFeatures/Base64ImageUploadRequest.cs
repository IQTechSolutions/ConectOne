using FilingModule.Domain.Enums;

namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to upload an image encoded in Base64 format.
    /// </summary>
    /// <remarks>This class is used to encapsulate the details required for uploading an image, including its
    /// name, display order, and type. The image data is provided as a Base64-encoded string.</remarks>
    public class Base64ImageUploadRequest
    {
        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool Featured { get; set; } = false;

        /// <summary>
        /// Gets or sets the CSS selector used to identify elements within a document.
        /// </summary>
        public string? Selector { get; set; }

        /// <summary>
        /// Gets or sets the order of the item in a sequence or list.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the type of the image being uploaded.
        /// </summary>
        public UploadType ImageType { get; set; } = UploadType.Image;

        /// <summary>
        /// Gets or sets the Base64-encoded string representation of the data.
        /// </summary>
        public string? Base64String { get; set; }
    }
}
