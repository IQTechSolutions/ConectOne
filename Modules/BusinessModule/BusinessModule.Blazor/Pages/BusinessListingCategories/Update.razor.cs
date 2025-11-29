using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BusinessModule.Blazor.Pages.BusinessListingCategories;

/// <summary>
/// Represents a Blazor component for updating category information, including its metadata, icon, and cover image.
/// </summary>
/// <remarks>This component provides functionality to update category details, upload and manage associated
/// images,  and handle navigation within the application. It relies on dependency injection for services such as  <see
/// cref="ISnackbar"/> for displaying notifications and <see cref="NavigationManager"/> for navigation.</remarks>
public partial class Update
{
    private CategoryViewModel _category = new();
    private string _imageSource = "_content/BusinessModule.Blazor/images/NoImage.jpg";
    private string? _iconToUpload;
    private string? _coverImageToUpload;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications.
    /// </summary>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used for handling navigation and URI management in a Blazor
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most cases.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public IConfiguration Configuration { get; set; } = null;

    /// <summary>
    /// Gets or sets the service responsible for managing business directory categories.
    /// </summary>
    [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    [Parameter] public string CategoryId { get; set; } = null!;

    /// <summary>
    /// Updates the icon image URL for the category.    
    /// </summary>
    /// <param name="value">The new URL of the icon image to be set.</param>
    private void IconChanged(string value)
    {
        _category.IconImageUrl = value;
        _iconToUpload = IconValueHelper.RequiresUpload(value) ? value : null;
    }

    /// <summary>
    /// Updates the cover image source when the image is changed.
    /// </summary>
    /// <param name="coverImage">The new cover image URL.</param>
    private void CoverImageChanged(MudCropperResponse coverImage)
    {
        _imageSource = coverImage.Base64String;
        _coverImageToUpload = _imageSource;
    }

    /// <summary>
    /// Updates the current category by sending the necessary data to the server and processing the response.
    /// </summary>
    /// <remarks>This method performs the following operations: <list type="bullet"> <item> Sends the category
    /// data to the server for updating. </item> <item> If an icon image is specified, uploads the image, removes the
    /// existing icon (if any), and associates the new icon with the category. </item> <item> If a cover image is
    /// specified, uploads the image in Base64 format, removes the existing cover image (if any), and associates the new
    /// cover image with the category. </item> <item> Displays a success message upon successful completion and
    /// navigates to the category list page. </item> </list> Any errors encountered during the process are displayed
    /// using the SnackBar.</remarks>
    /// <returns></returns>
    private async Task UpdateAsync()
    {
        var result = await BusinessDirectoryCategoryService.UpdateCategoryAsync(_category.ToDto());
        result.ProcessResponseForDisplay(SnackBar, async () =>
        {
            if (!string.IsNullOrEmpty(_iconToUpload))
            {
                var fileName = _iconToUpload.Split("/");

                var image = new ImageDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _category.Name,
                    FileName = fileName.Last(),
                    ContentType = fileName.Last().Split(".").Last(),
                    Size = 0,
                    RelativePath = _iconToUpload,
                    ImageType = UploadType.Icon
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(image);
                if (imageUploadResult.Succeeded)
                {
                    if (_category.Images.Any(c => c.ImageType == UploadType.Icon))
                    {
                        var removalResult = await BusinessDirectoryCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Icon).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await BusinessDirectoryCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_category.Name} Cover Image",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    if (_category.Images.Any(c => c.ImageType == UploadType.Cover))
                    {
                        var removalResult = await BusinessDirectoryCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await BusinessDirectoryCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Service \"{_category.Name}\" was successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/businessdirectory/categories");
        });
    }

    /// <summary>
    /// Cancels the current creation process and navigates to the categories page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/businessdirectory/categories" route.  Ensure that the
    /// navigation context is valid when calling this method.</remarks>
    private void CancelCreation()
    {
        NavigationManager.NavigateTo("/businessdirectory/categories");
    }

    /// <summary>
    /// Asynchronously initializes the component and retrieves category data based on the specified category ID.
    /// </summary>
    /// <remarks>This method fetches category data from the provider and initializes the category view model. 
    /// It also invokes the base class's initialization logic.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await BusinessDirectoryCategoryService.CategoryAsync(CategoryId);
        _category = new CategoryViewModel(result.Data);
        _imageSource = _category.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath is not null ? $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_category.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}" : "_content/BusinessModule.Blazor/images/NoImage.jpg";
        await base.OnInitializedAsync();
    }
}
