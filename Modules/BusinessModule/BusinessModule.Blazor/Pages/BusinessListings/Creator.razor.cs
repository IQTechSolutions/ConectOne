using BusinessModule.Application.ViewModel;
using BusinessModule.Blazor.Modals;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using System.Net.Http.Headers;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;

namespace BusinessModule.Blazor.Pages.BusinessListings;

/// <summary>
/// Provides functionality for creating and managing business listings, including uploading cover images and navigating
/// between related views.
/// </summary>
/// <remarks>This class is designed to handle the creation of business listings within the application. It
/// includes methods for managing cover images, interacting with HTTP services, and navigating to different parts of the
/// application. Dependencies such as HTTP providers, dialog services, and navigation are injected to facilitate these
/// operations.</remarks>
public partial class Creator
{
    private readonly BusinessListingViewModel _listing = new();
    private string _imageSource = "_content/BusinessModule.Blazor/background.png";
    private string? _coverImageToUpload;
    private readonly ICollection<IBrowserFile> _galleryImagesToUpload = [];
    private readonly ICollection<string> _galleryImagesToUploadForDisplay = [];
    private readonly ICollection<UploadResult> _galleryImagesBusyUploading = [];
    private bool _galleryImageUploadInProgress;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private List<CategoryDto> _availableCategories = [];
    private string _value = "Nothing selected";

    private List<ListingTierDto> _availableListingTiers = [];
    private readonly Func<ListingTierDto?, string> _listingTierConverter = p => p?.Name;

    private readonly Func<CategoryDto?, string> _categoryConverter = p => p?.Name;

    private ICollection<string> _galleryImageToUpload = new List<string>();

    private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
    private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
    private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
    private bool uploadInProgress;
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;

    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Listings", href: "/businesslistings", icon: Icons.Material.Filled.List),
        new("Create Listing", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
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
    /// Gets or sets the service used to query listing tier information.
    /// </summary>
    [Inject] public IListingTierQueryService ListingTierQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing commands related to the business directory.
    /// </summary>
    [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing business directory categories.
    /// </summary>
    [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Updates the cover image based on the provided cropper response.
    /// </summary>
    /// <remarks>This method updates the internal state with the new cover image data, preparing it for
    /// further processing or upload.</remarks>
    /// <param name="coverImage">The response from the cropper containing the updated cover image data. The <see
    /// cref="MudCropperResponse.Base64String"/> property must contain the image data in Base64 format.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Creates a new business listing and uploads associated images if provided.
    /// </summary>
    /// <remarks>This method performs the following actions: <list type="bullet"> <item> Posts the business
    /// listing data to the "businessdirectory" endpoint. </item> <item> If a cover image is provided, uploads the image
    /// to the "images/uploadbase64" endpoint and associates it with the listing. </item> <item> Removes any existing
    /// cover image for the listing before adding the new one. </item> <item> Displays success or error messages using
    /// the provided <c>SnackBar</c> component. </item> </list> After successful creation, the user is redirected to the
    /// business listings page.</remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var result = await BusinessDirectoryCommandService.CreateAsync(_listing.ToDto(), _cancellationToken);
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_listing.Heading} Logo",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
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
                var galleryUploadResult = await ImageProcessingService.UploadImage(request, _cancellationToken);
                if (galleryUploadResult.Succeeded)
                {
                    var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = galleryUploadResult.Data.Id }, _cancellationToken);
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            foreach (var product in _listing.Products)
            {
                if (string.IsNullOrEmpty(product.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = Guid.NewGuid().ToString(),
                        ImageType = UploadType.Image,
                        Base64String = product.CoverImageToUpload
                    };
                    var galleryUploadResult = await ImageProcessingService.UploadImage(request, _cancellationToken);
                    if (galleryUploadResult.Succeeded)
                    {
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = galleryUploadResult.Data.Id }, _cancellationToken);
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }

            foreach (var service in _listing.Services)
            {
                if (string.IsNullOrEmpty(service.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = Guid.NewGuid().ToString(),
                        ImageType = UploadType.Image,
                        Base64String = service.CoverImageToUpload
                    };
                    var galleryUploadResult = await ImageProcessingService.UploadImage(request, _cancellationToken);
                    if (galleryUploadResult.Succeeded)
                    {
                        var additionResult = await BusinessDirectoryCommandService.AddListingServiceImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = galleryUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }

            await Upload();

            SnackBar.AddSuccess($"{_listing.Heading} was created successfully");
            NavigationManager.NavigateTo("/businesslistings");
        });
    }

    /// <summary>
    /// Permanently removes an image from the server after explicit user confirmation.
    /// </summary>
    private async Task DeleteImage(ImageDto image)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this image from this application?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var deleteResult = await BusinessDirectoryCommandService.RemoveImage(image.Id);
            if (deleteResult.Succeeded)
            {
                _listing.Images.Remove(_listing.Images.FirstOrDefault(c => c.Id == image.Id));
            }
            else
                SnackBar.Add("Failed to delete image", Severity.Error);
        }
    }

    /// <summary>
    /// Cancels the current operation and navigates to the business listings page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/businesslistings" route. Ensure that the  <see
    /// cref="NavigationManager"/> is properly initialized before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/businesslistings");
    }

    #region Listing Services

    /// <summary>
    /// Displays a modal dialog for creating a new listing service and adds the resulting service to the current
    /// listing.
    /// </summary>
    /// <remarks>This method opens a modal dialog to allow the user to input details for a new listing
    /// service.  If the dialog is confirmed, the new service is added to the current listing's collection of
    /// services.</remarks>
    /// <returns></returns>
    private async Task AddListingService()
    {
        var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
        var parameters = new DialogParameters<AddBusinessListingServiceModal>
        {
            { x => x.ListingService, new ListingServiceViewModel() }
        };

        var dialog = await DialogService.ShowAsync<AddBusinessListingServiceModal>("Create Listing Service", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var vm = (ListingServiceViewModel)result.Data;

            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                vm.Id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(vm.ListingId))
            {
                vm.ListingId = _listing.Id;
            }

            var model = vm.ToDto();
            _listing.Services.Add(model);
        }
    }

    /// <summary>
    /// Opens a dialog to edit the details of a specified listing service.
    /// </summary>
    /// <remarks>If the dialog is confirmed, the updated service details will replace the original  service in
    /// the listing. If the dialog is canceled, no changes will be made.</remarks>
    /// <param name="listingService">The <see cref="ListingServiceDto"/> representing the service to be edited.  This parameter cannot be <see
    /// langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private async Task EditListingService(ListingServiceDto listingService)
    {
        var index = _listing.Services.IndexOf(listingService);

        var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
        var parameters = new DialogParameters<AddBusinessListingServiceModal>
        {
            { x => x.ListingService, new ListingServiceViewModel(listingService) }
        };

        var dialog = await DialogService.ShowAsync<AddBusinessListingServiceModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var vm = (ListingServiceViewModel)result.Data;
            if (string.IsNullOrWhiteSpace(vm.ListingId))
            {
                vm.ListingId = _listing.Id;
            }

            var model = vm.ToDto();
            _listing.Services[index] = model;
        }
    }

    /// <summary>
    /// Prompts the user for confirmation and, if confirmed, removes a service from the listing by its identifier.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user. If the user confirms the action, the
    /// specified service is removed from the listing. If the user cancels the action, no changes are made. The method
    /// assumes that the service with the specified identifier exists in the listing.</remarks>
    /// <param name="listingServiceId">The unique identifier of the service to be removed from the listing. Cannot be null or empty.</param>
    /// <returns></returns>
    private async Task RemoveListingService(string listingServiceId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this service from this listing?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            _listing.Services.Remove(_listing.Services.FirstOrDefault(c => c.Id == listingServiceId));
        }
    }

    #endregion

    #region Listing Products

    /// <summary>
    /// Displays a modal dialog for creating a new listing product and adds the created product to the current listing.
    /// </summary>
    /// <remarks>The method opens a modal dialog to allow the user to input details for a new listing product.
    /// If the dialog is not canceled, the resulting product is added to the current listing's product
    /// collection.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task AddListingProduct()
    {
        var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Create Listing Product", options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var vm = (ListingProductViewModel)result.Data;

            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                vm.Id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(vm.ListingId))
            {
                vm.ListingId = _listing.Id;
            }

            var model = vm.ToDto();
            _listing.Products.Add(model);
        }
    }

    /// <summary>
    /// Opens a dialog to edit the details of a specified listing product.
    /// </summary>
    /// <remarks>This method displays a modal dialog where the user can modify the details of the specified
    /// product.  If the user confirms the changes, the product in the listing's collection is updated with the new
    /// details. If the user cancels the dialog, no changes are made.</remarks>
    /// <param name="listingProduct">The product to be edited, represented as a <see cref="ListingProductDto"/>.  This parameter cannot be null and
    /// must exist in the current listing's product collection.</param>
    /// <returns></returns>
    private async Task EditListingProduct(ListingProductDto listingProduct)
    {
        var index = _listing.Products.IndexOf(listingProduct);

        var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
        var parameters = new DialogParameters<AddBusinessListingProductModal>
        {
            { x => x.ListingProduct, new ListingProductViewModel(listingProduct) }
        };

        var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var vm = (ListingProductViewModel)result.Data;
            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                vm.Id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(vm.ListingId))
            {
                vm.ListingId = _listing.Id;
            }

            var model = vm.ToDto();
            _listing.Products[index] = model;
        }
    }

    /// <summary>
    /// Removes a product from the current listing after user confirmation.
    /// </summary>
    /// <remarks>This method displays a confirmation dialog to the user before removing the product.  If the
    /// user cancels the dialog, the product is not removed. </remarks>
    /// <param name="listingProductId">The unique identifier of the product to be removed from the listing.</param>
    private async Task RemoveListingProduct(string listingProductId)
    {
        var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this product from this listing?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

        var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            _listing.Products.Remove(_listing.Products.FirstOrDefault(c => c.Id == listingProductId));
        }
    }

    #endregion

    /// <summary>
    /// Asynchronously initializes the component and retrieves the list of available categories.
    /// </summary>
    /// <remarks>This method fetches the list of categories from the specified provider endpoint and populates
    /// the local category collection.  It also invokes the base class's initialization logic.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var categoryListResult = await BusinessDirectoryCategoryService.CategoriesAsync(cancellationToken: _cancellationToken);
        if (categoryListResult.Succeeded)
        {
            _availableCategories = categoryListResult.Data.ToList();
        }

        _availableListingTiers.Add(new ListingTierDto() { Id = "-1", Name = "Free Tier" });
        var listingTierResult = await ListingTierQueryService.AllListingTiersAsync(_cancellationToken);
        if (listingTierResult.Succeeded)
        {
            _availableListingTiers = listingTierResult.Data.ToList();
        }
        _listing.Tier = _availableListingTiers.FirstOrDefault();

        await base.OnInitializedAsync();
    }

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
    /// Removes a gallery image from the lodging if the user confirms the action.
    /// </summary>
    /// <remarks>This method prompts the user for confirmation before attempting to remove the
    /// specified image. If the image is not found in the lodging's gallery, an error message is displayed. Upon
    /// successful removal, the image is deleted from the server and the local gallery, and a success message is
    /// shown.</remarks>
    /// <param name="url">The URL of the image to be removed.</param>
    ///  private async Task RemoveGalleryImage(string url)
    private async Task RemoveGalleryImage(string url)
    {
        if (await DialogService.ConfirmAction("Are you sure you want to remove this lodging from this application?"))
        {
            var toRemove = _galleryImageToUpload.FirstOrDefault(c => c == url);
            if (toRemove == null)
            {
                SnackBar.AddError("No image found");
                return;
            }

            var imageRemovalResult = await BusinessDirectoryCommandService.RemoveImage(toRemove);
            imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _galleryImageToUpload.Remove(toRemove);
                SnackBar.AddSuccess("Image successfully removed");
            });
        }
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
                var additionResult = await BusinessDirectoryCommandService.AddVideo( new AddEntityVideoRequest() { EntityId = _listing.Id, VideoId = response.Data.VideoId });
                if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                else _videosToUpload.Remove(file);
            }

            else
                SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);

            _filesBusyUploading.Remove(uploadResult);
        });

        uploadInProgress = false;
    }
}
