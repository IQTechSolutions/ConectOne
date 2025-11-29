using Microsoft.AspNetCore.Http;

namespace FilingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to upload a video, including metadata and the video file.
    /// </summary>
    /// <remarks>This class is used to encapsulate the details of a video upload operation,  including the
    /// video's name, whether it should be featured, and the file itself.</remarks>
    public class VideoUploadRequest
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
        /// Gets or sets the uploaded file associated with the request.
        /// </summary>
        public IFormFile File { get; set; } 
    }
}
