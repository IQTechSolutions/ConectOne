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
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.Products;

/// <summary>
/// Represents a component for editing product details, including updating product information and managing navigation
/// within the product management workflow.
/// </summary>
/// <remarks>The <see cref="Editor"/> component provides functionality for loading, editing, and updating product
/// details. It interacts with HTTP services to fetch and update product data, and it integrates with UI services such
/// as dialogs and notifications to enhance the user experience.</remarks>
public partial class Editor
{
    private string _imageSource = "_content/ProductsModule.Blazor/background.png";
    private string? _coverImageToUpload;
    private ProductViewModel _product = new();
    private ICollection<string> _galleryImageToUpload = new List<string>();

    private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
    private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
    private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
    private bool uploadInProgress;
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;

    private List<CategoryDto> _availableCategories = [];
    private readonly Func<CategoryDto?, string> _categoryConverter = p => p?.Name ?? "";

    private readonly List<BreadcrumbItem> _items = new()
    {
        new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new BreadcrumbItem("Products", href: "/products", icon: Icons.Material.Filled.List),
        new BreadcrumbItem("Update Product", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

    /// <summary>
    /// Gets or sets the service responsible for managing product-related operations.
    /// </summary>
    [Inject] public IProductService ProductService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing product categories.
    /// </summary>
    [Inject] public IProductCategoryService ProductCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages to the user.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the framework and should not be manually set in most
    /// scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [Parameter] public string ProductId { get; set; } = null!;

    /// <summary>
    /// Removes an uploaded image after confirmation.
    /// </summary>
    /// <param name="url">The URL of the image to remove.</param>
    private async Task RemoveOnUploadedImage(string url)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
        {
            var toRemove = _galleryImageToUpload.FirstOrDefault(c => c == url.Replace(Configuration["ApiConfiguration:BaseApiAddress"], ""));
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
    /// Updates the cover image for the product.
    /// </summary>
    /// <remarks>This method updates both the internal image source and the product's cover image URL. Ensure
    /// that the provided <paramref name="coverImage"/> is a valid and accessible path or URL.</remarks>
    /// <param name="coverImage">The URL or path of the new cover image. Cannot be null or empty.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Handles the event triggered when a file is selected or changed in the input.
    /// </summary>
    /// <remarks>This method validates the size of the selected file and adds it to the upload queue if it
    /// meets the size requirements. Files larger than 500 MB are rejected, and an error message is displayed.</remarks>
    /// <param name="e">The event arguments containing information about the selected file.</param>
    /// <returns></returns>
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
    /// Opens a dialog to allow the user to crop an image and uploads the cropped image if the operation is not
    /// canceled.
    /// </summary>
    /// <remarks>This method displays a modal dialog for image cropping using the <see
    /// cref="MudCropperModal"/> component.  If the user confirms the operation, the cropped image is added to the
    /// collection of images to upload.</remarks>
    /// <returns></returns>
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
    /// Removes a gallery image after confirmation.
    /// </summary>
    /// <param name="url">The URL of the image to remove.</param>
    private async Task RemoveGalleryImage(string imageId)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
        {
            var imageRemovalResult = await ProductService.RemoveImage(imageId);
            imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _product.Images.Remove(_product.Images.FirstOrDefault(c => c.Id == imageId));
                SnackBar.AddSuccess("Image successfully removed");
            });
        }
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
    /// Uploads a collection of video files asynchronously, reporting progress for each file and handling the upload
    /// results.
    /// </summary>
    /// <remarks>This method processes the files in parallel, uploading each file to the server and updating
    /// the progress for each upload.  If an upload succeeds, the video is associated with the specified entity. If an
    /// upload fails, an error message is displayed. The method ensures that the upload process is tracked and updates
    /// the state of the application accordingly.</remarks>
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

            var response = await VideoProcessingService.UploadVideoAsync(content);
            if (response.Succeeded)
            {
                SnackBar.Add("Upload successful 🎉", Severity.Success);

                var additionResult = await ProductService.AddVideo(new AddEntityVideoRequest() { EntityId = _product.ProductId, VideoId = response.Data.VideoId });
                if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                _videosToUpload.Remove(file);
            }

            else
                SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);

            _filesBusyUploading.Remove(uploadResult);
        });

        uploadInProgress = false;
    }

    /// <summary>
    /// Removes a gallery image after confirmation.
    /// </summary>
    /// <param name="url">The URL of the image to remove.</param>
    private async Task RemoveVideo(string videoId)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this Video from this lodging?"))
        {
            var imageRemovalResult = await ProductService.DeleteAsync(videoId);
            imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _product.Videos.Remove(_product.Videos.FirstOrDefault(c => c.Id == videoId));
                SnackBar.AddSuccess("Video successfully removed");
            });
        }
    }

    /// <summary>
    /// Updates the current product asynchronously by sending its data to the server.
    /// </summary>
    /// <remarks>This method sends the product data to the server using a POST request.  If the update is
    /// successful, a success message is displayed using the provided snack bar.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateAsync()
    {
        var result = await ProductService.UpdateAsync(_product.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_product.Details.Name} Cover Image",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_product.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await ProductService.RemoveImage(_product.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await ProductService.AddImage(new AddEntityImageRequest() { EntityId = _product.ProductId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            foreach (var galleryImage in _galleryImageToUpload)
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = Guid.NewGuid().ToString(),
                    ImageType = UploadType.Image,
                    Base64String = galleryImage
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await ProductService.AddImage(new AddEntityImageRequest() { EntityId = _product.ProductId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            await Upload();

            SnackBar.AddSuccess($"{_product.Details.Name} was updated successfully");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates the user to the products page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/products" route. Ensure that the navigation context
    /// is properly configured to handle this route.</remarks>
    public void Cancel()
    {
        NavigationManager.NavigateTo("/products");
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves product data.
    /// </summary>
    /// <remarks>This method fetches product details using the provided product ID and initializes the 
    /// product view model. If the operation succeeds, the product's cover image URL is used  as the image source. Any
    /// error messages from the operation are displayed in the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _imageSource = _product.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover) is null 
            ? "_content/ProductsModule.Blazor/images/NoImage.jpg" : 
            $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_product.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath?.TrimStart('/')}";
        var categoriesResult = await ProductCategoryService.CategoriesAsync();
        if (categoriesResult.Succeeded)
            _availableCategories = categoriesResult.Data.ToList();

        var result = await ProductService.PricedProductAsync(ProductId);
        if (result.Succeeded)
        {
            _product = new ProductViewModel(result.Data);
            _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_product?.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}";
        }
        else
        {
            SnackBar.AddErrors(result.Messages);
        }
            
        await base.OnInitializedAsync();
    }
}
