using BloggingModule.Domain.Entities;
using ConectOne.Blazor.Extensions;
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
    /// The Update component is responsible for updating an existing blog post category.
    /// It provides a form for editing category details and handles the update process.
    /// </summary>
    public partial class Update
    {
        private string? _iconToUpload;
        private string? _coverImageToUpload;
        private string _imageSource = "_content/Blogging.Blazor/images/NoImage.jpg";
        private CategoryViewModel _category = new();

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> BlogPostCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// The ID of the category to be updated.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

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
        /// <param name="value">The new URL of the icon image to be set.</param>
        private void IconChanged(string value)
        {
            _category.IconImageUrl = value;
            _iconToUpload = IconValueHelper.RequiresUpload(value) ? value : null;
        }

        /// <summary>
        /// Handles the update of the category.
        /// Sends a POST request to the server with the updated category data.
        /// </summary>
        private async Task UpdateAsync()
        {
            var result = await BlogPostCategoryService.UpdateCategoryAsync(_category.ToDto());

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
                            var removalResult = await BlogPostCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Icon).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BlogPostCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
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
                            var removalResult = await BlogPostCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BlogPostCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add($"Action Successful. Service \"{_category.Name}\" was successfully updated.", Severity.Success);
                NavigationManager.NavigateTo("/evers/categories");
            });
        }

        /// <summary>
        /// Cancels the update process and navigates back to the categories page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/evers/categories");
        }

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var result = await BlogPostCategoryService.CategoryAsync(CategoryId);
                _category = new CategoryViewModel(result.Data);

                if (_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) is not null)
                    _imageSource = _category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).RelativePath;

                StateHasChanged();
            }
        }
    }
}
