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

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Advertising
{
    /// <summary>
    /// Represents a Blazor component for updating an advertisement, including its details and associated images.
    /// </summary>
    /// <remarks>This component provides functionality to update an advertisement's details, including its
    /// title, description, and cover image. It interacts with an HTTP provider to perform CRUD operations on the
    /// advertisement and its associated images.</remarks>
    public partial class AddEditor
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

        /// <summary>
        /// Gets or sets the service used to query advertisements.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="IAdvertisementQueryService"/> is provided before using this property.</remarks>
        [Inject] public IAdvertisementQueryService AdvertisementQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing advertisement-related commands.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and should not be null.
        /// Ensure that the service implementation is properly configured before use.</remarks>
        [Inject] public IAdvertisementCommandService AdvertisementCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing images.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation and respond to URI changes.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration is typically populated by the dependency injection system and
        /// provides access to key-value pairs for application settings. Modifying this property is not recommended
        /// unless you need to replace the configuration source.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the advertisement.
        /// </summary>
        [Parameter] public string AdvertisementId { get; set; } = null!;

        /// <summary>
        /// Handles changes to the cover image by updating the internal image source and preparing the image for upload.
        /// </summary>
        /// <param name="coverImage">The updated cover image, represented as a <see cref="MudCropperResponse"/> containing the image data in
        /// Base64 format.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Updates the advertisement and its associated cover image asynchronously.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>
        /// <description>Updates the advertisement details by sending a POST request.</description> </item> <item>
        /// <description>If a new cover image is provided, uploads the image, removes the existing cover image (if any),
        /// and associates the new image with the advertisement.</description> </item> <item> <description>Displays
        /// success or error messages in the snack bar based on the operation results.</description> </item>
        /// </list></remarks>
        /// <returns></returns>
        private async Task UpdateAsync()
        {
            var result = await AdvertisementCommandService.UpdateAsync(_advertisement.ToDto());
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
        /// <remarks>This method redirects the user to the "/advertisements" route. Ensure that the
        /// navigation context is appropriate for this operation before invoking the method.</remarks>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/advertisements");
        }

        /// <summary>
        /// Asynchronously initializes the component by retrieving advertisement data and setting up the view model.
        /// </summary>
        /// <remarks>This method fetches advertisement details from the API using the provided
        /// advertisement ID.  If the data retrieval is successful and the advertisement data is not null, the view
        /// model and image source are updated accordingly. Any error messages returned by the API are displayed using
        /// the snack bar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await AdvertisementQueryService.AdvertisementAsync(AdvertisementId);
            if (result.Succeeded && result.Data is not null)
            {
                _advertisement = new AdvertisementViewModel(result.Data);
                _imageSource = result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) != null ? $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{result.Data.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}" : _imageSource;
            }
            SnackBar.AddErrors(result.Messages);
            await base.OnInitializedAsync();
        }
    }
}
