using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Advertisements;

/// <summary>
/// Represents an editor component for managing advertisements, including updating advertisement details, handling cover
/// image uploads, and navigating between pages.
/// </summary>
/// <remarks>This component is designed to work within a Blazor application and relies on dependency injection for
/// services such as HTTP operations, dialog management, and navigation. It provides functionality for editing
/// advertisements, uploading images, and displaying user feedback through snack bar notifications.</remarks>
public partial class Editor
{
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Advertisements", href: "/advertisements", icon: Icons.Material.Filled.List),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private AdvertisementViewModel _advertisement = new();

    private ICollection<AdvertisementTierDto> _advertisementTiers = [];

    /// <summary>
    /// Gets or sets the service used to query advertisements.
    /// </summary>
    [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing advertisement-related commands.
    /// </summary>
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
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be manually set in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the advertisement.
    /// </summary>
    [Parameter] public string AdvertisementId { get; set; } = null!;

    /// <summary>
    /// Updates the cover image based on the provided cropper response.
    /// </summary>
    /// <remarks>This method updates the internal image source and prepares the cover image for upload. The
    /// <paramref name="coverImage"/> parameter must not be null and should contain a valid Base64 string.</remarks>
    /// <param name="coverImage">The response from the cropper containing the updated cover image in Base64 format.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Updates the advertisement asynchronously, including optional cover image upload and processing.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item> <description>Posts
    /// the advertisement data to the server for updating.</description> </item> <item> <description>If a cover image is
    /// provided, uploads the image, removes any existing cover image, and associates the new image with the
    /// advertisement.</description> </item> <item> <description>Displays success or error messages in the user
    /// interface via the <see cref="SnackBar"/>.</description> </item> </list></remarks>
    /// <returns></returns>
    private async Task UpdateAsync()
    {
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
                        if(!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await AdvertisementCommandService.AddImage(new AddEntityImageRequest() { EntityId = _advertisement.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_advertisement.Title} was updated successfully");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the advertisements page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/advertisements" route. Ensure that the navigation
    /// context is valid when calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/advertisements");
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves advertisement data based on the specified advertisement
    /// ID.
    /// </summary>
    /// <remarks>This method fetches advertisement details from the data provider and initializes the
    /// component's state with the retrieved data. If the operation succeeds and data is available, the advertisement
    /// details and cover image are set.  Any error messages from the operation are added to the snack bar for user
    /// feedback.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await AdvertisementQueryService.AdvertisementAsync(AdvertisementId);
        if (result.Succeeded && result.Data is not null)
        {
            _advertisement = new AdvertisementViewModel(result.Data);
            _imageSource = result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) != null ? $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}" : _imageSource;

            var advertisementTierResult = await AdvertisementTierQueryService.AllAdvertisementTiersAsync(_advertisement.AdvertisementType);
            if (advertisementTierResult.Succeeded)
            {
                _advertisementTiers = advertisementTierResult.Data.ToList();
            }
        }
        else
        {
            SnackBar.AddErrors(result.Messages);
        }
        await base.OnInitializedAsync();
    }
}
