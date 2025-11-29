using BusinessModule.Application.ViewModel;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessModule.Blazor.Pages.ListingTiers;

/// <summary>
/// Represents a component responsible for creating and managing advertisement tiers in the application.
/// </summary>
/// <remarks>This class provides functionality for creating advertisement tiers, managing associated images, and
/// navigating between pages. It relies on injected services for HTTP requests, dialog interactions, notifications, and
/// navigation.</remarks>
public partial class Creator
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly ListingTierViewModel _listingTier = new();
    private readonly List<BreadcrumbItem> _items = 
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Business Listing Tiers", href: "/listing_tiers", icon: Icons.Material.Filled.List),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];

    /// <summary>
    /// Gets or sets the service used to query listing tier information.
    /// </summary>
    [Inject] public IListingTierQueryService ListingTierQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing commands related to listing tiers.
    /// </summary>
    [Inject] public IListingTierCommandService ListingTierCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing images.
    /// </summary>
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
    /// Creates a new listing tier and uploads an associated cover image if provided.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item> Sends a request to
    /// create a new listing tier using the current data. </item> <item> If a cover image is provided, uploads the image
    /// and associates it with the newly created listing tier. </item> <item> Removes any existing cover image
    /// associated with the listing tier before adding the new one. </item> <item> Displays success or error messages
    /// based on the operation's outcome. </item> </list></remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var result = await ListingTierCommandService.CreateAsync(_listingTier.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_listingTier.Name} Logo",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_listingTier.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await ListingTierCommandService.RemoveImage(_listingTier.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await ListingTierCommandService.AddImage(new AddEntityImageRequest() { EntityId = result.Data.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.AddSuccess($"{_listingTier.Name} was created successfully");
            NavigationManager.NavigateTo("/listing_tiers");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the listing tiers page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/listing_tiers" route. Ensure that the navigation
    /// context is valid before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/listing_tiers");
    }
}
