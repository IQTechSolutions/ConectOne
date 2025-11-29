using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.AdvertisementTiers;

/// <summary>
/// Represents a component for creating and managing affiliate entities within the application.
/// </summary>
/// <remarks>The <see cref="Creator"/> class provides functionality for creating new affiliates, uploading cover
/// images,  and navigating between different views. It interacts with various services such as HTTP providers, dialog
/// services,  and snack bar notifications to facilitate these operations.</remarks>
public partial class Creator
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly AdvertisementTierViewModel _advertisementTier = new();
    private readonly List<BreadcrumbItem> _items = 
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Advertisement Tiers", href: "/advertisement_tiers", icon: Icons.Material.Filled.List),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    [Inject] public IAdvertisementTierCommandService AdvertisementTierCommandService { get; set; } = null!;

    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used to manage URI navigation and interaction in a Blazor
    /// application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Updates the cover image based on the provided cropper response.
    /// </summary>
    /// <remarks>This method updates the internal image source and prepares the cover image for
    /// upload.</remarks>
    /// <param name="coverImage">The response from the cropper containing the updated cover image data. Must include a valid Base64-encoded
    /// string representing the image.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Creates a new affiliate entity asynchronously and uploads an associated cover image if provided.
    /// </summary>
    /// <remarks>This method sends a request to create a new affiliate entity using the provided affiliate
    /// data.  If a cover image is specified, it uploads the image, associates it with the created affiliate,  and
    /// removes any existing cover image for the affiliate. Upon successful creation, a success  message is displayed,
    /// and the user is navigated to the affiliates page.</remarks>
    private async Task CreateAsync()
    {
        var result = await AdvertisementTierCommandService.CreateAsync(_advertisementTier.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_advertisementTier.Name} Logo",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_advertisementTier.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await AdvertisementTierCommandService.RemoveImage(_advertisementTier.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await AdvertisementTierCommandService.AddImage(new AddEntityImageRequest() { EntityId = result.Data.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.AddSuccess($"{_advertisementTier.Name} was created successfully");
            NavigationManager.NavigateTo("/advertisement_tiers");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the affiliates page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/affiliates" route. Ensure that the navigation context
    /// is valid when calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/advertisement_tiers");
    }
}
