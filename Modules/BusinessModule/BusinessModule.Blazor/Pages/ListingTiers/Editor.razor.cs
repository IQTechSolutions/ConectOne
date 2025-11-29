using BusinessModule.Application.ViewModel;
using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BusinessModule.Blazor.Pages.ListingTiers;

/// <summary>
/// Represents a Blazor component for managing and editing listing tiers, including their associated data and images.
/// </summary>
/// <remarks>The <see cref="Editor"/> component provides functionality for updating listing tier details, handling
/// image uploads, and navigating between different views. It relies on dependency-injected services for HTTP requests,
/// dialog management, notifications, and navigation. The component is designed to work within the context of a Blazor
/// application and interacts with the application's backend API to perform its operations.</remarks>
public partial class Editor
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Listing Tiers", href: "/listing_tiers", icon: Icons.Material.Filled.List),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];
    
    private ListingTierViewModel _listingTier = new();

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
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in a
    /// Blazor application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier for the listing tier.
    /// </summary>
    [Parameter] public string ListingTierId { get; set; } = null!;

    /// <summary>
    /// Updates the current listing tier asynchronously, including its associated data and images.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item> Posts the updated
    /// listing tier data to the server. </item> <item> If a new cover image is provided, uploads the image, removes the
    /// existing cover image (if any),  and associates the new image with the listing tier. </item> <item> Displays
    /// success or error messages in the user interface based on the operation results. </item> </list></remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateAsync()
    {
        var result = await ListingTierCommandService.UpdateAsync(_listingTier.ToDto());
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
                    var additionResult = await ListingTierCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listingTier.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_listingTier.Name} was updated successfully");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the listing tiers page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/listing_tiers" route. Ensure that the navigation
    /// context is valid and that the target route is accessible.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/listing_tiers");
    }

    /// <summary>
    /// Handles changes to the cover image by updating the internal image source and preparing the image for upload.
    /// </summary>
    /// <param name="coverImage">The updated cover image, represented as a <see cref="MudCropperResponse"/> containing the image data in Base64
    /// format.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Asynchronously initializes the component by retrieving data for the specified listing tier and setting up the
    /// view model.
    /// </summary>
    /// <remarks>This method fetches data for the listing tier identified by <see cref="ListingTierId"/> using
    /// the provided data source.  If the operation succeeds and data is returned, the view model is initialized with
    /// the retrieved data.  Any error messages from the operation are displayed using the snack bar.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await ListingTierQueryService.ListingTierAsync(ListingTierId);
        if (result.Succeeded && result.Data is not null)
        {
            _listingTier = new ListingTierViewModel(result.Data);
        }
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }
}