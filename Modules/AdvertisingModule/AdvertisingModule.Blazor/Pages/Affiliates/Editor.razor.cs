using AdvertisingModule.Application.ViewModels;
using AdvertisingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace AdvertisingModule.Blazor.Pages.Affiliates;

/// <summary>
/// Represents an editor component for managing affiliate data, including updating affiliate details and uploading
/// associated images.
/// </summary>
/// <remarks>This component provides functionality to edit affiliate information, upload cover images, and handle
/// navigation and user notifications. It interacts with backend services for data retrieval and updates, and uses
/// dependency-injected services for HTTP requests, dialog management, and notifications.</remarks>
public partial class Editor
{
    private readonly List<BreadcrumbItem> _items =
    [
        new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
        new("Affiliates", href: "/affiliates", icon: Icons.Material.Filled.List),
        new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
    ];
    private string? _coverImageToUpload;
    private string _imageSource = "_content/AdvertisingModule.Blazor/background.png";
    private AffiliateViewModel _affiliate = new();

    /// <summary>
    /// Gets or sets the service used to query affiliate-related data.
    /// </summary>
    [Inject] public IAffiliateQueryService AffiliateQueryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for executing affiliate-related commands.
    /// </summary>
    [Inject] public IAffiliateCommandService AffiliateCommandService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="IDialogService"/> is provided before using this property.</remarks>
    [Inject] public IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
    /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
    /// Blazor application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the affiliate identifier associated with the current context.
    /// </summary>
    [Parameter] public string AffiliateId { get; set; } = null!;

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
    /// Updates the affiliate information and uploads a cover image if specified.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet">
    /// <item><description>Updates the affiliate details by sending a request to the "affiliates"
    /// endpoint.</description></item> <item><description>If a cover image is provided, uploads the image as a
    /// base64-encoded string to the "images/uploadbase64" endpoint.</description></item> <item><description>Replaces
    /// the existing cover image for the affiliate, if one exists, by removing the old image and associating the new
    /// one.</description></item> <item><description>Displays success or error messages in the UI using the provided
    /// <c>SnackBar</c> instance.</description></item> </list></remarks>
    /// <returns></returns>
    private async Task UpdateAsync()
    {
        var result = await AffiliateCommandService.UpdateAsync(_affiliate.ToDto());
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
                    if (Enumerable.Any<ImageDto>(_affiliate.Images, c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await AffiliateCommandService.RemoveImage(Enumerable.FirstOrDefault<ImageDto>(_affiliate.Images, c => c.ImageType == UploadType.Cover).Id);
                        if(!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await AffiliateCommandService.AddImage(new AddEntityImageRequest() { EntityId = _affiliate.Id, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }
            SnackBar.AddSuccess($"{_affiliate.Title} was updated successfully");
        });
    }

    /// <summary>
    /// Cancels the current operation and navigates to the affiliates page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/affiliates" route. Ensure that the navigation context
    /// is valid before calling this method.</remarks>
    private void Cancel()
    {
        NavigationManager.NavigateTo("/affiliates");
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
        var result = await AffiliateQueryService.AffiliateAsync(AffiliateId);
        if (result.Succeeded && result.Data is not null)
        {
            _affiliate = new AffiliateViewModel(result.Data);
            _imageSource = result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) != null ? $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}" : _imageSource;
        }
        SnackBar.AddErrors(result.Messages);
        await base.OnInitializedAsync();
    }
}
