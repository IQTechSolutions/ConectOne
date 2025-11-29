using FilingModule.Application.ViewModels;
using FilingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FilingModule.Blazor.Modals
{
    /// <summary>
    /// The CreateDocumentLinkUrl component is responsible for creating a document link URL.
    /// It provides functionality to upload files and submit the document link URL.
    /// </summary>
    public partial class CreateDocumentLinkUrlModal
    {
        /// <summary>
        /// Injected HTTP client factory for creating HTTP clients.
        /// </summary>
        [Inject] public IHttpClientFactory ClientFactory { get; set; } = null!;

        /// <summary>
        /// The ID of the entity associated with the document link.
        /// </summary>
        [Parameter] public string EntityId { get; set; } = null!;

        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// The view model for creating the document link URL.
        /// </summary>
        private readonly CreateDocumentLinkUrlViewModel _model = new();

        /// <summary>
        /// Uploads the selected file by converting it to a Base64 string and storing it in the model.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        private async Task UploadFiles(IBrowserFile file)
        {
            if (file is null)
            {
                return;
            }

            try
            {
                // Prevent duplicate entries when the same file is uploaded multiple times.
                if (_model.Documents.Any(document => document.FileName == file.Name && document.Size == file.Size))
                {
                    return;
                }

                await using var readStream = file.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await readStream.CopyToAsync(memoryStream);

                var base64String = Convert.ToBase64String(memoryStream.ToArray());

                _model.Documents.Add(new DocumentDto
                {
                    Url = base64String,
                    FileName = file.Name,
                    Size = file.Size,
                    ContentType = file.ContentType
                });
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Failed to upload file '{file.Name}': {exception.Message}");
            }
        }

        /// <summary>
        /// Submits the document link URL and closes the dialog.
        /// </summary>
        public void SubmitAsync()
        {
            MudDialog.Close(_model);
        }

        /// <summary>
        /// Cancels the document link creation and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
