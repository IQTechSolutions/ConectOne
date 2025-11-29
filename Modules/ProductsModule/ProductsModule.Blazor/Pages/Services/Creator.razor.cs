using System.Net.Http.Headers;
using ConectOne.Blazor.Extensions;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.Services;

/// <summary>
/// Provides functionality for creating new products, including managing product details, handling user interactions,
/// and navigating between views.
/// </summary>
/// <remarks>This class is designed to facilitate the creation of new products within the application. It includes
/// methods for saving product data, canceling the creation process, and updating the product's cover image. 
/// Dependencies such as HTTP communication, dialog services, and navigation are injected to support these
/// operations.</remarks>
public partial class Creator
{
    private string _imageSource = "_content/ProductsModule.Blazor/background.png";
    private string? _coverImageToUpload;
    private readonly ServiceViewModel _service = new();
    private ICollection<string> _galleryImageToUpload = new List<string>();

    private List<ProductDto> _variantsToUpload = [];
    private List<ProductMetadataDto> _metaDataToUpload = [];

    private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
    private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
    private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
    private bool uploadInProgress;
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;

    private readonly List<BreadcrumbItem> _items = new()
    {
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Services", href: "/services", icon: Icons.Material.Filled.List),
        new("Create Product", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework and should not be
    /// set manually in most cases.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be manually set in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for handling operations related to services.
    /// </summary>
    [Inject] public IServiceService ServiceService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing images.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing product-related operations.
    /// </summary>
    [Inject] public IProductService ProductService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of selected destinations.
    /// </summary>
    public IEnumerable<CategoryDto>? SelectedCategories { get; set; } = [];

    /// <summary>
    /// Handles the event triggered when the input file changes.
    /// </summary>
    /// <remarks>This method processes multiple files provided in the event arguments and updates the
    /// internal file name collection. It also clears any drag-related visual state before processing the
    /// files.</remarks>
    /// <param name="e">An <see cref="InputFileChangeEventArgs"/> instance containing information about the changed input file(s).</param>
    private async Task OnInputFileChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();

        const long maxBytes = 500 * 1024 * 1024;
        if (e.File.Size > maxBytes)
        {
            SnackBar.Add("File too large (limit 500 MB)", Severity.Error);
            return;
        }

        _videosToUpload.Add(e.File);
    }

    /// <summary>
    /// Clears the current state of the file upload process, including uploaded files and drag-and-drop styling.
    /// </summary>
    /// <remarks>This method resets the file upload component by clearing any uploaded files, removing
    /// drag-and-drop styling,  and ensuring the component is ready for new uploads. If no file upload operation is
    /// active, the method completes without action.</remarks>
    /// <returns>A task that represents the asynchronous clear operation.</returns>
    private async Task ClearAsync()
    {
        await (_fileUpload?.ClearAsync() ?? Task.CompletedTask);
        _videosToUpload.Clear();
        ClearDragClass();
    }

    /// <summary>
    /// Updates the drag class to include the default drag class and a primary border style.    
    /// </summary>
    /// <remarks>This method sets the drag class to a combination of the default drag class and a
    /// predefined  primary border style. It is typically used to visually indicate a drag-and-drop
    /// operation.</remarks>
    private void SetDragClass() => _dragClass = $"{DefaultDragClass} mud-border-primary";

    /// <summary>
    /// Resets the drag class to its default value.
    /// </summary>
    /// <remarks>This method clears any custom drag class and restores the default drag class. It is
    /// typically used to reset the state after a drag-and-drop operation.</remarks>
    private void ClearDragClass() => _dragClass = DefaultDragClass;

    /// <summary>
    /// Opens a file picker dialog asynchronously, allowing the user to select a file.
    /// </summary>
    /// <remarks>If no file upload component is available, the method completes immediately without
    /// performing any action.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task completes when the file picker dialog
    /// is closed or if no file upload component is available.</returns>
    private Task OpenFilePickerAsync() => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

    /// <summary>
    /// Updates the cover image for the product.
    /// </summary>
    /// <remarks>This method updates both the internal image source and the product's cover image URL. Ensure
    /// that the provided <paramref name="coverImage"/> is a valid and accessible image path or URL.</remarks>
    /// <param name="coverImage">The URL or path of the new cover image. Cannot be null or empty.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Displays an image cropping dialog and uploads the cropped image to the gallery.
    /// </summary>
    /// <remarks>This method opens a modal dialog for the user to crop an image. If the user confirms
    /// the operation,  the cropped image is added to the gallery upload list. If the operation is canceled, no
    /// changes are made.</remarks>
    private async Task UploadImageAsync()
    {
        var parameters = new DialogParameters<MudCropperModal> { { x => x.Src, "/_content/NeuralTech.Blazor/images/NoImage.jpg" } };

        var dialog = await DialogService.ShowAsync<MudCropperModal>("Crop Your Image", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var galleryImage = (MudCropperResponse)result.Data;
            _galleryImageToUpload.Add(galleryImage.Base64String);
        }
    }

    /// <summary>
    /// Removes an image from the lodging's gallery upload list after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user for confirmation before removing the image. If the image
    /// is not found in the upload list,  an error message is displayed. Upon successful removal, a success message
    /// is shown.</remarks>
    /// <param name="url">The URL of the image to be removed from the gallery upload list.</param>
    private async Task RemoveOnUploadedImage(string url)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
        {
            var toRemove = _galleryImageToUpload.FirstOrDefault(c => c == url);
            if (toRemove == null)
            {
                SnackBar.AddError("No image found");
                return;
            }

            _galleryImageToUpload.Remove(toRemove);
            SnackBar.AddSuccess("Image successfully removed");
        }
    }

    /// <summary>
    /// Adds the specified product attribute to the collection of variants to be uploaded.
    /// </summary>
    /// <remarks>This method queues the provided product attribute for upload by adding it to an internal
    /// collection. Ensure that <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The product attribute data transfer object to add. Cannot be null.</param>
    private void CreateProductAttribute(ProductDto dto)
    {
        _variantsToUpload.Add(dto);
    }

    /// <summary>
    /// Updates the product attribute in the collection of variants to upload.
    /// </summary>
    /// <remarks>If a product with the same <see cref="ProductDto.ProductId"/> exists in the collection,  it
    /// will be replaced with the provided <paramref name="dto"/>. If no matching product is found,  the behavior of
    /// this method is undefined.</remarks>
    /// <param name="dto">The product data transfer object containing the updated product information.  The <see
    /// cref="ProductDto.ProductId"/> property is used to locate the product to update.</param>
    private void UpdateProductAttribute(ProductDto dto)
    {
        var index = _variantsToUpload.IndexOf(_variantsToUpload.FirstOrDefault(c => c.ProductId == dto.ProductId));
        _variantsToUpload[index] = dto;
    }

    /// <summary>
    /// Removes the product attribute associated with the specified variant ID from the collection of variants to
    /// upload.
    /// </summary>
    /// <remarks>If no product variant with the specified ID exists in the collection, the method performs no
    /// action.</remarks>
    /// <param name="variantId">The unique identifier of the product variant to remove. Cannot be <see langword="null"/> or empty.</param>
    private void RemoveProductAttribute(string variantId)
    {
        _variantsToUpload.Remove(_variantsToUpload.FirstOrDefault(c => c.ProductId == variantId));
    }

    /// <summary>
    /// Adds the specified product metadata to the collection of metadata to be uploaded.
    /// </summary>
    /// <param name="dto">The product metadata to add. Cannot be null.</param>
    private void CreateProductMetadata(ProductMetadataDto dto)
    {
        _metaDataToUpload.Add(dto);
    }

    /// <summary>
    /// Updates the metadata for a specific product in the collection.
    /// </summary>
    /// <remarks>This method replaces the metadata of the product identified by <see
    /// cref="ProductMetadataDto.ProductId"/>  with the provided <paramref name="dto"/>. If no matching product is
    /// found, the behavior is undefined.</remarks>
    /// <param name="dto">The updated product metadata. The <see cref="ProductMetadataDto.ProductId"/> property must match an existing
    /// product in the collection.</param>
    private void UpdateProductMetadata(ProductMetadataDto dto)
    {
        var index = _metaDataToUpload.IndexOf(_metaDataToUpload.FirstOrDefault(c => c.ProductId == dto.ProductId));
        _metaDataToUpload[index] = dto;
    }

    /// <summary>
    /// Removes the product metadata with the specified identifier from the collection.
    /// </summary>
    /// <remarks>If no metadata with the specified identifier exists in the collection, the method performs no
    /// action.</remarks>
    /// <param name="metadataId">The identifier of the product metadata to remove. Cannot be <see langword="null"/> or empty.</param>
    private void RemoveProductMetadata(string metadataId)
    {
        _metaDataToUpload.Remove(_metaDataToUpload.FirstOrDefault(c => c.ProductId == metadataId));
    }

    /// <summary>
    /// Asynchronously creates a new product by sending the product data to the server.
    /// </summary>
    /// <remarks>This method sends a request to create a product using the provided product data.  If the
    /// operation is successful, a success message is displayed in the snack bar.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAsync()
    {
        var result = await ServiceService.CreateAsync(_service.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var imageRequest = new Base64ImageUploadRequest()
                {
                    Name = $"{_service.Name} CoverImage",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(imageRequest);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await ServiceService.AddImage(new AddEntityImageRequest() { EntityId = _service.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }

                foreach (var galleryImage in _galleryImageToUpload)
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = Guid.NewGuid().ToString(),
                        ImageType = UploadType.Image,
                        Base64String = galleryImage
                    };
                    var galleryUploadResult = await ImageProcessingService.UploadImage(request);
                    if (galleryUploadResult.Succeeded)
                    {
                        var additionResult = await ServiceService.AddImage(new AddEntityImageRequest() { EntityId = _service.Id, ImageId = galleryUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                //foreach (var productVariant in _variantsToUpload)
                //{
                //    var createResult = await ProductService.CreateAsync(productVariant);
                //    if (!createResult.Succeeded) SnackBar.AddErrors(createResult.Messages);
                //}

                //foreach (var metaData in _metaDataToUpload)
                //{
                //    var createResult = await ServiceService.a Provider.PutAsync("services/metadata", metaData);
                //    if (!createResult.Succeeded) SnackBar.AddErrors(createResult.Messages);
                //}

                await Upload();
            }
            SnackBar.AddSuccess($"{_service.Name} was created successfully");
            NavigationManager.NavigateTo($"/services");
        });
    }

    /// <summary>
    /// Uploads a collection of video files asynchronously to the server.
    /// </summary>
    /// <remarks>This method processes the video files in parallel, uploading each file while tracking
    /// its progress.  If an upload succeeds, the video is added to the destination collection. If an upload fails,
    /// an error  message is displayed. The method ensures that the upload progress is updated in real-time and
    /// handles  large files efficiently by limiting the read stream size.</remarks>
    /// <returns></returns>
    private async Task Upload()
    {
        uploadInProgress = true;

        await Parallel.ForEachAsync(_videosToUpload.ToList(), async (file, cancellationToken) =>
        {
            var uploadResult = new UploadResult
            {
                Progress = 0,
                TotalBytes = file.Size
            };

            _filesBusyUploading.Add(uploadResult);

            await using var stream = file.OpenReadStream(500 * 1024 * 1024); // limit server-side too
            using var content = new MultipartFormDataContent();
            var streamContent = new ProgressableStreamContent(stream, 64 * 1024, (uploaded) => uploadResult.Progress = (int)(uploaded * 100 / uploadResult.TotalBytes));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.Name);

            var response = await VideoProcessingService.UploadVideoAsync(content, cancellationToken);
            if (response.Succeeded)
            {
                SnackBar.Add("Upload successful 🎉", Severity.Success);
                var additionResult = await ServiceService.AddVideo(new AddEntityVideoRequest() { EntityId = _service.Id, VideoId = response.Data.VideoId }, cancellationToken);
                if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                else _videosToUpload.Remove(file);
            }

            else
                SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);

            _filesBusyUploading.Remove(uploadResult);
        });

        uploadInProgress = false;
    }

    /// <summary>
    /// Cancels the current operation and navigates to the products page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/products" route. Ensure that the navigation context
    /// is appropriate for this action before calling the method.</remarks>
    public void Cancel()
    {
        NavigationManager.NavigateTo("/services");
    }

    /// <summary>
    /// Initializes the product creation page by setting a default image, fetching available categories from the server,
    /// and performing any additional base initialization logic.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        _imageSource = "_content/ProductsModule.Blazor/images/NoImage.jpg";

        await base.OnInitializedAsync();
    }
}
