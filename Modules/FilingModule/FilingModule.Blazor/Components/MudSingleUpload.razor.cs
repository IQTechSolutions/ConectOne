using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FilingModule.Blazor.Components
{
    /// <summary>
    /// The MudSingleImageCropperUpload component is responsible for handling the upload and cropping of a single image.
    /// It provides a button to open a dialog for cropping the image and displays the cropped image.
    /// </summary>
    public partial class MudSingleUpload
    {
        /// <summary>
        /// Event callback for when a file is accepted.
        /// </summary>
        [Parameter] public EventCallback<MudCropperResponse> OnFileAccepted { get; set; }

        /// <summary>
        /// The type of the entity associated with the image.
        /// </summary>
        [Parameter] public Type Entity { get; set; } = null!;

        /// <summary>
        /// The type of the image to be uploaded.
        /// </summary>
        [Parameter] public UploadType ImageType { get; set; }

        /// <summary>
        /// The source URL of the image.
        /// </summary>
        [Parameter] public string? Src { get; set; }

        /// <summary>
        /// The caption for the image.
        /// </summary>
        [Parameter] public string Caption { get; set; } = null!;

        /// <summary>
        /// The subtext for the image.
        /// </summary>
        [Parameter] public string SubText { get; set; } = null!;

        /// <summary>
        /// The ID of the parent entity.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// The placeholder image URL.
        /// </summary>
        [Parameter] public string PlaceHolder { get; set; } = "/_content/NeuralTech.Blazor/images/NoImage.jpg";

        /// <summary>
        /// Indicates whether to display the image.
        /// </summary>
        [Parameter] public bool DisplayImage { get; set; } = true;

        /// <summary>
        /// Additional attributes for the component.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; } = null!;

        /// <summary>
        /// The text for the button to change the cover image.
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = "Change Cover Image";

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit the image.
        /// </summary>
        [Parameter] public bool CanEditImage { get; set; } = true;
        
        /// <summary>
        /// List of files to be uploaded.
        /// </summary>
        private List<IBrowserFile> _files = [];

        /// <summary>
        /// Indicates whether the component should render.
        /// </summary>
        private bool _shouldRender;

        /// <summary>
        /// The upload path for the image.
        /// </summary>
        private string _uploadPath = "users/images";

        /// <summary>
        /// The maximum file size allowed for the upload.
        /// </summary>
        private long _maxFileSize = 1024 * 1024 * 1024;

        /// <summary>
        /// The maximum number of files allowed for the upload.
        /// </summary>
        private int _maxAllowedFiles = 10;

        /// <summary>
        /// List of upload results.
        /// </summary>
        private List<UploadResult> _uploadResults = new();

        /// <summary>
        /// Opens a dialog for cropping the image and updates the source URL with the cropped image.
        /// </summary>
        private async Task UploadCoverImageAsync(IBrowserFile browserFile)
        {
            using var stream = browserFile.OpenReadStream(_maxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            byte[] fileBytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(fileBytes);
            Src = $"data:{browserFile.ContentType};base64,{base64String}";
                await OnFileAccepted.InvokeAsync(new MudCropperResponse(){ Base64String = Src, File = browserFile});
            
        }
    }
}
