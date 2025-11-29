using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Affiliates;

/// <summary>
/// Represents a component for creating and managing affiliate entities within the application.
/// </summary>
/// <remarks>The <see cref="AdvertisementTiers.Creator"/> class provides functionality for creating new affiliates, uploading cover
/// images,  and navigating between different views. It interacts with various services such as HTTP providers, dialog
/// services,  and snack bar notifications to facilitate these operations.</remarks>
public partial class Creator
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly AffiliateViewModel _affiliate = new();
    private readonly List<BreadcrumbItem> _items = 
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Affiliates", href: "/affiliates", icon: Icons.Material.Filled.List),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];

    /// <summary>
    /// Gets or sets the service responsible for executing affiliate-related commands.
    /// </summary>
    [Inject] public IAffiliateCommandService AffiliateCommandService { get; set; } = null!;

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
    /// Handles changes to the cover image by updating the image source and preparing the image for upload.
    /// </summary>
    /// <param name="coverImage">The updated cover image, represented as a <see cref="MudCropperResponse"/> containing the image data in Base64
    /// format.</param>
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
        var result = await AffiliateCommandService.CreateAsync(_affiliate.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_affiliate.Title} Logo",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_affiliate.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await AffiliateCommandService.RemoveImage(_affiliate.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await AffiliateCommandService.AddImage(new AddEntityImageRequest() { EntityId = result.Data.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_affiliate.Title} was created successfully");
            NavigationManager.NavigateTo("/affiliates");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the affiliates page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/affiliates" route. Ensure that the navigation context
    /// is valid when calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/affiliates");
    }
}
