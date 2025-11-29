using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Advertisements;

/// <summary>
/// Represents a component for creating advertisements within the application.
/// </summary>
/// <remarks>This component provides functionality for creating new advertisements, including uploading cover
/// images and managing associated data. It integrates with various services such as HTTP providers, dialog services,
/// and navigation management to facilitate the creation process.</remarks>
public partial class Creator
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly AdvertisementViewModel _advertisement = new();
    private ICollection<AdvertisementTierDto> _advertisementTiers = [];
    private readonly List<BreadcrumbItem> _items = 
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Advertisements", href: "/advertisements", icon: Icons.Material.Filled.List),
        new("Create", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];

    
    /// <summary>
    /// Gets or sets the service used to query advertisements.
    /// </summary>
    [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing advertisement-related commands.
    /// </summary>
    /// <remarks>This property is automatically injected and should be configured in the dependency injection
    /// container.</remarks>
    [Inject] public IAdvertisementCommandService AdvertisementCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to query advertisement tiers.
    /// </summary>
    [Inject] public IAdvertisementTierQueryService AdvertisementTierQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

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
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of advertisement to be displayed.
    /// </summary>
    [Parameter] public AdvertisementType AdvertisementType { get; set; } 

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
    /// Creates a new advertisement asynchronously and uploads a cover image if provided.
    /// </summary>
    /// <remarks>This method performs the following actions: <list type="bullet"> <item> <description>Creates
    /// a new advertisement by sending the advertisement data to the server.</description> </item> <item>
    /// <description>If a cover image is provided, uploads the image and associates it with the
    /// advertisement.</description> </item> <item> <description>Removes any existing cover image associated with the
    /// advertisement before adding the new one.</description> </item> <item> <description>Displays success or error
    /// messages in the UI and navigates to the advertisements page upon successful creation.</description> </item>
    /// </list></remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        _advertisement.AdvertisementType = AdvertisementType;

        var result = await AdvertisementCommandService.CreateAsync(_advertisement.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_advertisement.Title} Logo",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_advertisement.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await AdvertisementCommandService.RemoveImage(_advertisement.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await AdvertisementCommandService.AddImage(new AddEntityImageRequest() { EntityId = result.Data.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_advertisement.Title} was created successfully");
            NavigationManager.NavigateTo("/advertisements");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates the user to the advertisements page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/advertisements" route. Ensure that the  <see
    /// cref="NavigationManager"/> is properly initialized before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/advertisements");
    }

    protected override async Task OnInitializedAsync()
    {
        var advertisementTierResult = await AdvertisementTierQueryService.AllAdvertisementTiersAsync(_advertisement.AdvertisementType);
        if (advertisementTierResult.Succeeded)
        {
            _advertisementTiers = advertisementTierResult.Data.ToList();
        }

        await base.OnInitializedAsync();
    }
}
