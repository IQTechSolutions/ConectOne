using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FilingModule.Blazor.Modals
{
    /// <summary>
    /// The MudSingleImageCropperUpload component is responsible for handling the upload and cropping of a single image.
    /// It provides a button to open a dialog for cropping the image and displays the cropped image.
    /// </summary>
    public partial class MudSingleImageCropperUpload
    {
        /// <summary>
        /// Injected dialog service for showing dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

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
        /// Event callback for when a file is accepted.
        /// </summary>
        [Parameter] public EventCallback<MudCropperResponse> OnFileAccepted { get; set; }

        /// <summary>
        /// Gets or sets the aspect ratio of the component.
        /// </summary>
        [Parameter] public string AspectRatio { get; set; } = "4/3";

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
        private long _maxFileSize = 1024 * 1024 * 3;

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
        private async Task UploadCoverImageAsync()
        {
            var parameters = new DialogParameters<MudCropperModal> { { x => x.Src, Src }, {x => x.AspectRatio, AspectRatio} };

            var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                Src = result.Data!.ToString()!;
                await OnFileAccepted.InvokeAsync((MudCropperResponse)result.Data);
            }
        }
    }
}
