using BloggingModule.Domain.Entities;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BloggingModule.Blazor.Pages.BlogPostCategories
{
    /// <summary>
    /// The Create component is responsible for creating a new blog post category.
    /// It provides a form for entering category details and handles the creation process.
    /// </summary>
    public partial class Create
    {
        private string? _iconToUpload;
        private string? _coverImageToUpload;
        private CategoryViewModel _category = new();
        private string _imageSource = "_content/Blogging.Blazor/images/NoImage.jpg";

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use this service to
        /// show transient messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> BlogCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically set by the Blazor
        /// framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// The ID of the parent category, if any.
        /// </summary>
        [Parameter] public string? ParentId { get; set; }

        /// <summary>
        /// Handles the change event for the cover image.
        /// Updates the image source and the cover image URL in the category view model.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Updates the icon image URL for the category.
        /// </summary>
        /// <remarks>This method updates the internal state of the category's icon image URL. Ensure that
        /// the provided URL is valid and accessible.</remarks>
        /// <param name="value">The new URL of the icon image to be set for the category. Cannot be null or empty.</param>
        private void IconChanged(string value)
        {
            _category.IconImageUrl = value;
            _iconToUpload = IconValueHelper.RequiresUpload(value) ? value : null;
        }

        /// <summary>
        /// Handles the creation of the new category.
        /// Sends a PUT request to the server with the category data.
        /// </summary>
        private async Task CreateAsync()
        {
            var creationResult = await BlogCategoryService.CreateCategoryAsync(_category.ToDto());
            if (creationResult.Succeeded)
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
                    var imageUploadResult = await ImageProcessingService.UploadImage(new Base64ImageUploadRequest() { Name = image.Name, ImageType = UploadType.Icon, Base64String = _iconToUpload });
                    if (imageUploadResult.Succeeded)
                    {
                        var additionResult = await BlogCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
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
                        var additionResult = await BlogCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add($"Action Successful. Service \"{_category.Name}\" was successfully created.", Severity.Success);
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
                    NavigationManager.NavigateTo(_category.ParentCategoryId == null ? "/evers/categories" : $"/evers/categories/update/{ParentId}");
                }
                else
                {
                    _category = new CategoryViewModel() { ParentCategoryId = ParentId };
                }
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the categories page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/evers/categories");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the parent category ID if provided.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ParentId))
            {
                _category.ParentCategoryId = ParentId;
            }

            await base.OnInitializedAsync();
        }
    }
}
