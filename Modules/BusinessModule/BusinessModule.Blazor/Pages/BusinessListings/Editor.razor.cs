using BusinessModule.Application.ViewModel;
using BusinessModule.Blazor.Modals;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using System.Net.Http.Headers;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;

namespace BusinessModule.Blazor.Pages.BusinessListings;

/// <summary>
/// Represents an editor component for managing business listings, including their details, images,  services, and
/// products.
/// </summary>
/// <remarks>This class provides functionality for creating, updating, and managing business listings. It includes
/// features for uploading images, handling user interactions, and managing associated services and products.  The
/// component relies on dependency injection for services such as HTTP operations, navigation, and  notifications.
/// Ensure that all required dependencies are properly configured before using this component.</remarks>
public partial class Editor
{
    private BusinessListingViewModel _listing = new();
    private string _imageSource = "_content/BusinessModule.Blazor/background.png";
    private string? _coverImageToUpload;
    private ICollection<string> _galleryImageToUpload = new List<string>();

    private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
    private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
    private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
    private bool uploadInProgress;
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;

    private List<ListingTierDto> _availableListingTiers = [];
    private readonly Func<ListingTierDto?, string> _listingTierConverter = p => p?.Name;

    private List<CategoryDto> _availableCategories = [];
    private readonly Func<CategoryDto?, string> _categoryConverter = p => p?.Name ?? "";

    private readonly List<BreadcrumbItem> _items = new()
    {
        new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new BreadcrumbItem("Listings", href: "/businesslistings", icon: Icons.Material.Filled.List),
        new BreadcrumbItem("Update Listing", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    };

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
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the framework and should not be manually set in most
    /// scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to query the business directory.
    /// </summary>
    [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to execute commands related to the business directory.
    /// </summary>
    [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing business directory categories.
    /// </summary>
    [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing commands related to listing tiers.
    /// </summary>
    [Inject] public IListingTierQueryService ListingTierQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the listing.
    /// </summary>
    [Parameter] public string ListingId { get; set; } = null!;

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
    /// Updates the current business listing asynchronously, including its details, cover image, and gallery images.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item>
    /// <description>Updates the business listing details on the server.</description> </item> <item>
    /// <description>Uploads a new cover image if specified, replacing the existing one if necessary.</description>
    /// </item> <item> <description>Uploads additional gallery images associated with the listing.</description> </item>
    /// </list> If the update is successful, a success message is displayed, and the user is navigated back to the
    /// business listings page.</remarks>
    /// <returns></returns>
    private async Task UpdateAsync()
    {
        var result = await BusinessDirectoryCommandService.UpdateAsync(_listing.ToDto());
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
                    if (_listing.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await BusinessDirectoryCommandService.RemoveImage(_listing.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
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
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            foreach (var product in _listing.Products)
            {
                if (string.IsNullOrEmpty(product.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{product.Name} Cover",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (product.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingProduct(product.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
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
                        Name = $"{service.Name} Cover",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (service.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingServiceImage(service.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingServiceImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }

            await Upload();

            SnackBar.AddSuccess($"{_listing.Heading} was updated successfully");
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
            var deleteResult = await ImageProcessingService.DeleteImageAsync(image.Id);
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
        var dialog = await DialogService.ShowAsync<AddBusinessListingServiceModal>("Create Listing Service", options);
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
            var addResult = await BusinessDirectoryCommandService.AddListingService(model);
            if (addResult.Succeeded)
            {
                _listing.Services.Add(model);

                if (!string.IsNullOrEmpty(vm.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{vm.Name} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = vm.CoverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (vm.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(vm.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = vm.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }
            else
            {
                SnackBar.AddErrors(addResult.Messages);
            }
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
            var model = ((ListingServiceViewModel)result.Data!).ToDto();
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                model = model with { Id = Guid.NewGuid().ToString() };
            }

            if (string.IsNullOrWhiteSpace(model.ListingId))
            {
                model = model with { ListingId = _listing.Id };
            }

            var updateResult = await BusinessDirectoryCommandService.UpdateListingService(model);
            if (updateResult.Succeeded)
            {
                _listing.Services[index] = model;

                if (!string.IsNullOrEmpty(model.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{model.Name} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = model.CoverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (model.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(model.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = model.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }
            else
            {
                SnackBar.AddErrors(updateResult.Messages);
            }
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
            var deleteResult = await BusinessDirectoryCommandService.RemoveListingService(listingServiceId);
            if (deleteResult.Succeeded)
            {
                _listing.Services.Remove(_listing.Services.FirstOrDefault(c => c.Id == listingServiceId));
            }
            else
            {
                SnackBar.AddErrors(deleteResult.Messages);
            }
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
        var parameters = new DialogParameters<AddBusinessListingProductModal>
        {
            { x => x.ListingProduct, new ListingProductViewModel() }
        };

        var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Create Listing Product", parameters, options);
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
            var addResult = await BusinessDirectoryCommandService.AddListingProduct(model);
            if (addResult.Succeeded)
            {
                _listing.Products.Add(model);

                if (!string.IsNullOrEmpty(vm.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{vm.Name} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = vm.CoverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (vm.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(vm.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = vm.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }
            else
            {
                SnackBar.AddErrors(addResult.Messages);
            }
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

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };

        var parameters = new DialogParameters<AddBusinessListingProductModal>
        {
            { x => x.ListingProduct, new ListingProductViewModel(listingProduct) }
        };

        var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            var vm = (ListingProductViewModel)result.Data!;

            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                vm.Id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(vm.ListingId))
            {
                vm.ListingId = _listing.Id;
            }

            var model = vm.ToDto();
            var updateResult = await BusinessDirectoryCommandService.UpdateListingProduct(model);
            if (updateResult.Succeeded)
            {
                _listing.Products[index] = model;

                if (!string.IsNullOrEmpty(vm.CoverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{vm.Name} Cover Image",
                        ImageType = UploadType.Cover,
                        Base64String = vm.CoverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (vm.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(vm.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = vm.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }
            }
            else
            {
                SnackBar.AddErrors(updateResult.Messages);
            }
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
            var deleteResult = await BusinessDirectoryCommandService.RemoveListingProduct(listingProductId);
            if (deleteResult.Succeeded)
            {
                _listing.Products.Remove(_listing.Products.FirstOrDefault(c => c.Id == listingProductId));
            }
            else
            {
                SnackBar.AddErrors(deleteResult.Messages);
            }
        }
    }

    #endregion

    

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
            var imageRemovalResult = await BusinessDirectoryCommandService.RemoveImage(imageId);
            imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _listing.Images.Remove(_listing.Images.FirstOrDefault(c => c.Id == imageId));
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

                var additionResult = await BusinessDirectoryCommandService.AddVideo(new AddEntityVideoRequest() { EntityId = _listing.Id, VideoId = response.Data.VideoId });
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
            var imageRemovalResult = await BusinessDirectoryCommandService.RemoveImage(videoId);
            imageRemovalResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _listing.Videos.Remove(_listing.Videos.FirstOrDefault(c => c.Id == videoId));
                SnackBar.AddSuccess("Video successfully removed");
            });
        }
    }

    #region LifeCycle Methods

    /// <summary>
    /// Asynchronously initializes the component by retrieving and processing business listing and category data.
    /// </summary>
    /// <remarks>This method fetches active business listings and categories from the data provider.  If a
    /// listing matching the specified <see cref="ListingId"/> is found, it is converted into a  <see
    /// cref="BusinessListingViewModel"/> and stored. Additionally, available categories are loaded  into a local
    /// collection. Any errors encountered during the data retrieval process are displayed  using the <see
    /// cref="SnackBar"/>.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await BusinessDirectoryQueryService.ListingAsync(ListingId);
        if (result.Succeeded)
        {
            var categoryListResult = await BusinessDirectoryCategoryService.CategoriesAsync();
            if (categoryListResult.Succeeded)
            {
                _availableCategories = categoryListResult.Data.ToList();
            }

            _availableListingTiers.Add(new ListingTierDto() { Id = "-1", Name = "Free Tier" });
            var listingTierResult = await ListingTierQueryService.AllListingTiersAsync();
            if (listingTierResult.Succeeded)
            {
                _availableListingTiers = listingTierResult.Data.ToList();
            }
            if (result.Data != null) _listing = new BusinessListingViewModel(result.Data);

            _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_listing?.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}";
        }
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }

    #endregion
}
