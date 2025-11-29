using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.ProductCategories;

/// <summary>
/// Represents a component for updating an existing product category.
/// </summary>
/// <remarks>This component allows users to update the details of a product category, including its name and cover
/// image. It interacts with a backend service to fetch the current category details and submit updates.</remarks>
public partial class Update
{
    private CategoryViewModel _category = new();
    private string _imageSource = "_content/ProductsModule.Blazor/images/NoImage.jpg";
    private string? _iconToUpload;
    private CategoryDto _parentCategory;
    private string? _coverImageToUpload;

    /// <summary>
    /// Gets or sets the service used to display snack bar notifications in the application.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework and should not be
    /// set manually in most cases.</remarks>
    [Inject] public ISnackbar SnackBar { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
    /// application.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be manually set in
    /// most scenarios.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; }

    /// <summary>
    /// Gets or sets the service responsible for managing product-related operations.
    /// </summary>
    [Inject] private IProductService ProductService { get; set; }

    /// <summary>
    /// Gets or sets the service responsible for managing product categories.
    /// </summary>
    [Inject] private IProductCategoryService ProductCategoryService { get; set; }

    /// <summary>
    /// Gets or sets the service responsible for performing image processing operations.
    /// </summary>
    [Inject] private IImageProcessingService ImageProcessingService { get; set; }

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] private IVideoProcessingService VideoProcessingService { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    [Parameter] public string CategoryId { get; set; } = null!;

    #region Methods

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
    /// Updates the current category by sending its data to the server asynchronously.
    /// </summary>
    /// <remarks>This method sends the category data to the server using a POST request and processes the
    /// server's response. If the update is successful, a success message is displayed, and the user is redirected to
    /// the categories page.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateAsync()
    {
        var result = await ProductCategoryService.UpdateCategoryAsync(_category.ToDto());
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
                        var removalResult = await ProductCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Icon).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await ProductCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
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
                        var removalResult = await ProductCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                    }
                    var additionResult = await ProductCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Service \"{_category.Name}\" was successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/products/categories");
        });
    }

    /// <summary>
    /// Cancels the current creation process and navigates to the product categories page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/products/categories" route. Ensure that the
    /// navigation context is valid when calling this method.</remarks>
    private void CancelCreation()
    {
        NavigationManager.NavigateTo("/products/categories");
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Asynchronously initializes the component and retrieves category data.
    /// </summary>
    /// <remarks>This method fetches category information based on the <see cref="CategoryId"/>  and
    /// initializes the category view model. It also invokes the base class's  initialization logic.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var result = await ProductCategoryService.CategoryAsync(CategoryId);
        _category = new CategoryViewModel(result.Data);

        _imageSource = _category.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover) is null ? "_content/ProductsModule.Blazor/images/NoImage.jpg" : _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_category?.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}"; 

        await base.OnInitializedAsync();
    }

    #endregion
}
