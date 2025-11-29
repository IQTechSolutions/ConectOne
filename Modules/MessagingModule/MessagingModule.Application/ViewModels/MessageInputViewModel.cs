using Microsoft.AspNetCore.Components.Forms;

namespace MessagingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the input data for a message, including text, file attachments, and image URLs.
    /// </summary>
    /// <remarks>This view model is typically used to capture user input for creating or sending a message. It
    /// includes the message text, a collection of files to be attached, and a collection of image URLs.</remarks>
    public class MessageInputViewModel
    {
        /// <summary>
        /// Gets or sets the message associated with the current operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of files uploaded by the user.
        /// </summary>
        public List<IBrowserFile> Files { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of image file paths.
        /// </summary>
        public List<IBrowserFile> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of video files uploaded by the user.
        /// </summary>
        public List<IBrowserFile> Videos { get; set; } = [];
    }
}
