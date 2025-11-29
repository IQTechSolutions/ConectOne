using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.AdvertisementTiers;

/// <summary>
/// Represents the editor component for managing advertisement tiers.
/// </summary>
/// <remarks>This component provides functionality for creating, updating, and managing advertisement tiers. It
/// includes dependency-injected services for HTTP communication, dialog management, notifications, and navigation. The
/// component is initialized by fetching affiliate data and setting up the view model.</remarks>
public partial class Editor
{
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Advertisement Tiers", href: "/advertisement_tiers", icon: Icons.Material.Filled.List),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];
    
    private AdvertisementTierViewModel _advertisementTier = new();

    /// <summary>
    /// Gets or sets the service used to query advertisements.
    /// </summary>
    [Inject] public IAdvertisementTierQueryService AdvertisementTierQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing advertisement-related commands.
    /// </summary>
    [Inject] public IAdvertisementTierCommandService AdvertisementTierCommandService { get; set; } = null!;

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
    /// Gets or sets the affiliate identifier associated with the current entity.
    /// </summary>
    [Parameter] public string AdvertisementTierId { get; set; } = null!;

    /// <summary>
    /// Updates the advertisement tier asynchronously by sending the updated data to the server.
    /// </summary>
    /// <remarks>This method sends the updated advertisement tier data to the server using a POST request. If
    /// the operation is successful, a success message is displayed in the snack bar.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateAsync()
    {
        var result = await AdvertisementTierCommandService.UpdateAsync(_advertisementTier.ToDto());
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
                    var additionResult = await AdvertisementTierCommandService.AddImage(new AddEntityImageRequest() { EntityId = _advertisementTier.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_advertisementTier.Name} was updated successfully");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the affiliates page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/affiliates" route. Ensure that the navigation context
    /// is valid before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/advertisement_tiers");
    }

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
    /// Asynchronously initializes the component by retrieving affiliate data and setting up the view model and image
    /// source.
    /// </summary>
    /// <remarks>This method fetches affiliate data using the provided <see cref="Provider"/> and initializes
    /// the affiliate view model  and image source based on the retrieved data. If the operation fails, error messages
    /// are added to the <see cref="SnackBar"/>.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await AdvertisementTierQueryService.AdvertisementTierAsync(AdvertisementTierId);
        if (result.Succeeded && result.Data is not null)
        {
            _advertisementTier = new AdvertisementTierViewModel(result.Data);
        }
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }
}
