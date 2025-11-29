using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.ProductCategories;

/// <summary>
/// Represents a component for creating a new category within the product management module.
/// </summary>
/// <remarks>This component provides functionality for creating a new category, including handling user input,
/// managing state, and interacting with backend services. It supports optional parent category association and provides
/// user feedback through dialogs and notifications.</remarks>
public partial class Create
{
    private string _imageSource = "_content/ProductsModule.Blazor/images/NoImage.jpg";
    private CategoryViewModel _category = new();
    private CategoryDto _parentCategory;
    private string? _iconToUpload;
    private string? _coverImageToUpload;

    #region Injections & Parameters

    /// <summary>
    /// Gets or sets the service used to display dialogs in the application.
    /// </summary>
    [Inject] public IDialogService DialogService { get; set; } = null!;

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
    /// most cases.</remarks>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for managing product categories.
    /// </summary>
    [Inject] public IProductCategoryService ProductCategoryService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing images.
    /// </summary>
    [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service responsible for processing video-related operations.
    /// </summary>
    [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the parent entity.
    /// </summary>
    [Parameter] public string? ParentId { get; set; }

    #endregion

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
    /// Creates a new product category asynchronously and handles the result of the creation process.
    /// </summary>
    /// <remarks>This method sends a request to create a new product category using the provided category
    /// data.  If the creation is successful, a confirmation dialog is displayed to the user, allowing them to  choose
    /// whether to add another category or navigate to the appropriate category management page.</remarks>
    /// <returns></returns>
    private async Task CreateAsync()
    {
        var creationResult = await ProductCategoryService.CreateCategoryAsync(_category.ToDto());
        creationResult.ProcessResponseForDisplay(SnackBar, async () =>
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
                    var additionResult = await ProductCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            if (!string.IsNullOrEmpty(_coverImageToUpload))
            {
                var request = new Base64ImageUploadRequest()
                {
                    Name = $"{_category.Name} CoverImage",
                    ImageType = UploadType.Cover,
                    Base64String = _coverImageToUpload
                };
                var imageUploadResult = await ImageProcessingService.UploadImage(request);
                if (imageUploadResult.Succeeded)
                {
                    var additionResult = await ProductCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                }
            }

            SnackBar.Add($"Action Successful. Category \"{_category.Name}\" was successfully created.", Severity.Success);
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Would you like to add another service?" },
                { x => x.ButtonText, "Yes" },
                { x => x.CancelButtonText, "No" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (result!.Canceled)
            {
                if (string.IsNullOrEmpty(ParentId))
                {
                    NavigationManager.NavigateTo("/products/categories");
                }
                else
                {
                    NavigationManager.NavigateTo($"/products/categories/update/{ParentId}");
                }
            }
            else
            {
                _category = new CategoryViewModel() { ParentCategoryId = ParentId };
            }
        });
    }

    /// <summary>
    /// Cancels the current creation process and navigates to the product categories page.
    /// </summary>
    /// <remarks>This method redirects the user to the "/products/categories" route.  It is typically used to
    /// abort the creation of a new product or category.</remarks>
    private void CancelCreation()
    {
        NavigationManager.NavigateTo("/products/categories");
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Asynchronously initializes the component and sets the parent category ID if a parent ID is provided.
    /// </summary>
    /// <remarks>If the <see cref="ParentId"/> property is not null or empty, its value is assigned to the
    /// parent category ID of the current category. This method also invokes the base class's asynchronous
    /// initialization logic.</remarks>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(ParentId))
        {
            var parentCategoryResult = await ProductCategoryService.CategoryAsync(ParentId);
            if (parentCategoryResult.Succeeded)
            {
                _parentCategory = parentCategoryResult.Data;
            }

            _category.ParentCategoryId = ParentId;
        }
        await base.OnInitializedAsync();
    }

    #endregion
}
