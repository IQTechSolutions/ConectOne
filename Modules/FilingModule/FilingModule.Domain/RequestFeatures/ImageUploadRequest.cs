using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to upload an image, including metadata and the file itself.
    /// </summary>
    /// <remarks>This class is used to encapsulate the details of an image upload operation,  such as the
    /// image's name, whether it is featured, its type, and the file content.</remarks>
    public class ImageUploadRequest
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the item is marked as featured.
        /// </summary>
        public bool Featured { get; set; } = false;

        /// <summary>
        /// Gets or sets the type of the uploaded content, indicating whether it is an image or another supported type.
        /// </summary>
        public UploadType ImageType { get; set; } = UploadType.Image;

        /// <summary>
        /// Gets or sets the uploaded file associated with the current request.
        /// </summary>
        public IFormFile File { get; set; }
    }
}
