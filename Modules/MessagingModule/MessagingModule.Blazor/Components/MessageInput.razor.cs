using MessagingModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace MessagingModule.Blazor.Components
{
    /// <summary>
    /// Represents a component for composing and sending messages, including support for text, images, and files.
    /// </summary>
    /// <remarks>This class provides functionality for handling user input, uploading images and files, and
    /// sending messages. It includes event handling for key presses (e.g., sending a message on "Enter" key) and
    /// supports image and file uploads.</remarks>
    public partial class MessageInput
    {
        private MessageInputViewModel _message = new MessageInputViewModel();
        private string _mainImageUrl = "";
        private List<string> _carouselImages = [];

        /// <summary>
        /// Gets or sets the callback invoked when a message is sent.
        /// </summary>
        [Parameter] public EventCallback<MessageInputViewModel> OnSendMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the system is currently busy sending data.
        /// </summary>
        [Parameter] public bool BusySending { get; set; }

        /// <summary>
        /// Updates the main image URL based on the specified state index.
        /// </summary>
        /// <param name="state">The index of the state used to select the corresponding image. Must be a valid index within the
        /// <c>Images</c> collection.</param>
        private void SetImageIndex(string state)
        {
            _mainImageUrl = state;
        }

        /// <summary>
        /// Converts the content of the specified <see cref="IBrowserFile"/> to a Base64-encoded string.
        /// </summary>
        /// <remarks>The method reads the file content as a stream, encodes it in Base64 format, and
        /// includes the MIME type in the resulting string. The maximum file size allowed is 5 MB. If the file exceeds
        /// this size, an exception will be thrown.</remarks>
        /// <param name="state">The <see cref="IBrowserFile"/> representing the file to be encoded. Must not be null.</param>
        /// <returns>A Base64-encoded string representing the file's content, prefixed with the file's MIME type.</returns>
        private async Task<string> GetBase64String(IBrowserFile state)
        {
            long maxFileSize = 1024 * 1024 * 5;

            var readStream = state.OpenReadStream(maxFileSize);
            var buf = new byte[readStream.Length];
            var ms = new MemoryStream(buf);

            await readStream.CopyToAsync(ms);

            var buffer = ms.ToArray();

            return $"data:{state.ContentType};base64,{Convert.ToBase64String(buffer)}" ;
        }

        /// <summary>
        /// Sends the current message asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="OnSendMessage"/> event with the current message
        /// and then resets the message input to a new instance of <see cref="MessageInputViewModel"/>.</remarks>
        /// <returns></returns>
        private async Task SendMessageAsync()
        {
            await OnSendMessage.InvokeAsync(_message);
            _message = new MessageInputViewModel();
            _mainImageUrl = "";
            _carouselImages = new List<string>();
        }

        /// <summary>
        /// Uploads a collection of image files and processes them into base64-encoded strings.
        /// </summary>
        /// <remarks>The first file in the list is processed and stored as the main image URL.  All files
        /// in the list are processed and added to the collection of message images.</remarks>
        /// <param name="files">A read-only list of image files to be uploaded. Must not be null or empty.</param>
        /// <returns></returns>
        private async Task UploadImages(IReadOnlyList<IBrowserFile> files)
        {
            if(files.Any())
            {
                _mainImageUrl = await GetBase64String(files[0]);
                foreach (var file in files)
                {
                    _message.Images.Add(file);
                    _carouselImages.Add(await GetBase64String(file));
                }
            }

            
        }

        /// <summary>
        /// Adds the specified files to the current message's file collection.
        /// </summary>
        /// <remarks>This method appends the provided files to the existing collection of files in the
        /// message.  The caller is responsible for ensuring that the files meet any required validation or constraints 
        /// before invoking this method.</remarks>
        /// <param name="files">A read-only list of files to be added. Each file must implement the <see cref="IBrowserFile"/> interface.</param>
        private void UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            foreach (var file in files)
            {
                _message.Files.Add(file);
            }
        }

        /// <summary>
        /// Handles the key press event in the chat input field.
        /// </summary>
        /// <remarks>If the <see cref="KeyboardEventArgs.Key"/> is <see langword="Enter"/>, this method
        /// triggers the  asynchronous sending of the chat message.</remarks>
        /// <param name="e">The keyboard event arguments containing details about the key press.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnKeyPressInChat(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendMessageAsync();
            }
        }
    }
}
