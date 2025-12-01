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
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Blazor.Pages.ActivityCategories
{
    /// <summary>
    /// The Create component is responsible for creating new activity categories.
    /// It provides a form for inputting category details and handles the creation process.
    /// </summary>
    public partial class Create
    {
        private string _imageSource = "_content/SchoolsModule.Blazor/images/NoImage.jpg";
        private CategoryViewModel _category = new();
        private string? _iconToUpload;
        private string? _coverImageToUpload;

        #region Injections & Parameters

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage categories for activity groups.
        /// </summary>
        [Inject] public IActivityGroupCategoryService ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the parent category, if any.
        /// </summary>
        [Parameter] public string? ParentId { get; set; }

        #endregion

        #region Methods

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
        /// Creates a new category by sending a PUT request to the API.
        /// Displays a success message upon successful creation and prompts the user to add another category.
        /// </summary>
        private async Task CreateAsync()
        {
            var creationResult = await ActivityGroupCategoryService.CreateCategoryAsync(_category.ToDto());

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
                    var imageUploadResult = await ImageProcessingService.UploadImage(new Base64ImageUploadRequest(){ Name = image.Name, ImageType = UploadType.Icon, Base64String = _iconToUpload });
                    if (imageUploadResult.Succeeded)
                    {
                        var additionResult = await ActivityGroupCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
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
                        var additionResult = await ActivityGroupCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
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
                    if (_category.ParentCategoryId == null)
                    {
                        NavigationManager.NavigateTo("/activitycategories");
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"/activities/categories/update/{_category.ParentCategoryId}");
                    }
                }
                else
                {
                    _category = new CategoryViewModel() { ParentCategoryId = ParentId };
                }
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the activity categories page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo("/activitycategories");
        }

        #endregion

        #region Overrides

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

        #endregion
    }
}
