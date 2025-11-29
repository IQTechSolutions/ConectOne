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
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Blazor.Modals;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.ActivityCategories
{
    /// <summary>
    /// The Update component is responsible for updating existing activity categories.
    /// It provides a form for editing category details and handles the update process.
    /// </summary>
    public partial class Update
    {
        private ActivityGroupPageParameters _args = new ActivityGroupPageParameters() { PageSize = 100 };
        private HashSet<ActivityGroupDto> _selectedActivityGroups = new();
        private CategoryViewModel _category = new();
        private string _imageSource;
        private string? _iconToUpload;
        private string? _coverImageToUpload;

        #region Injections & Parameters

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage categories for activity groups.
        /// </summary>
        [Inject] public ICategoryService<ActivityGroup> ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the category to be updated.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

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
        /// Updates the category by sending a POST request to the API.
        /// Displays a success message upon successful update and navigates to the appropriate page.
        /// </summary>
        private async Task UpdateAsync()
        {
            var result = await ActivityGroupCategoryService.UpdateCategoryAsync(_category.ToDto());
            
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
                            var removalResult = await ActivityGroupCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Icon).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await ActivityGroupCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = image.Id });
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
                            var removalResult = await ActivityGroupCategoryService.RemoveImage(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await ActivityGroupCategoryService.AddImage(new AddEntityImageRequest() { EntityId = _category.CategoryId, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                SnackBar.Add($"Action Successful. Service \"{_category.Name}\" was successfully updated.", Severity.Success);

                NavigationManager.NavigateTo("/activitycategories");
            });
        }

        /// <summary>
        /// Cancels the update process and navigates back to the appropriate page.
        /// </summary>
        private void CancelCreation()
        {
            if (string.IsNullOrWhiteSpace(_category.ParentCategoryId))
            {
                NavigationManager.NavigateTo("/activitycategories");
            }
            else
            {
                NavigationManager.NavigateTo($"/activities/categories/update/{_category.ParentCategoryId}");
            }
        }

        /// <summary>
        /// Creates a new event for the activity category.
        /// </summary>
        private async Task CreateNewEventForActivityEvent()
        {
            if (_category.EntityCount == 0)
            {
                var parameters = new DialogParameters<AddEventToActivityCategoryModal>()
                {
                    { x => x.ParentCategory, _category },
                };

                var dialog = await DialogService.ShowAsync<AddEventToActivityCategoryModal>("Confirm", parameters);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    NavigationManager.NavigateTo($"/activities/activitygroups/events/create/{result}");
                }
            }
            else
            {
                var parameters = new DialogParameters<AddEventToActivityGroupModal>()
                {
                    { x => x.ParentCategory, _category },
                };

                var dialog = await DialogService.ShowAsync<AddEventToActivityGroupModal>("Confirm", parameters);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    NavigationManager.NavigateTo($"/activities/activitygroups/events/create/{result}");
                }
            }
        }

        #endregion

        #region Activity Groups Table Setup

        /// <summary>
        /// Loads activity groups data from the server for the table.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The table data.</returns>
        private async Task<TableData<ActivityGroupDto>> ActivityGroupsServerData(TableState state, CancellationToken token)
        {
            // If multi-selection is on, fetch learners from the server, applying the table's paging/sorting.
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => string.Empty
                };

                _args.OrderBy = state.SortDirection != SortDirection.None
                    ? $"{state.SortLabel} {sortDirection}"
                    : string.Empty;
            }

            _args.PageSize = state.PageSize;
            _args.PageNr = state.Page + 1;
            _args.CategoryIds = CategoryId;

            var request = await ActivityGroupQueryService.PagedActivityGroupsAsync(_args);

            if (!request.Succeeded)
            {
                // Display any error messages if the request fails
                SnackBar.AddErrors(request.Messages);
                return new TableData<ActivityGroupDto> { TotalItems = 0, Items = new List<ActivityGroupDto>() };
            }

            // Re-sync any items that were previously selected in the local set,
            // so the table references the exact instances from the newly fetched data.
            foreach (var item in request.Data)
            {
                if (_selectedActivityGroups.Any(c => c.ActivityGroupId == item.ActivityGroupId))
                {
                    _selectedActivityGroups.RemoveWhere(c => c.ActivityGroupId == item.ActivityGroupId);
                    _selectedActivityGroups.Add(item);
                }
            }

            return new TableData<ActivityGroupDto>
            {
                TotalItems = request.TotalCount,
                Items = request.Data
            };
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the category details from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await ActivityGroupCategoryService.CategoryAsync(CategoryId);
            _category = new CategoryViewModel(result.Data);

            if(_category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover) is not null)
                _imageSource = _category.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).RelativePath;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
