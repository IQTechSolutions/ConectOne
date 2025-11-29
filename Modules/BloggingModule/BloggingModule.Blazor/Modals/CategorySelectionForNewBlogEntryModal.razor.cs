using BloggingModule.Domain.Entities;
using ConectOne.Blazor.Extensions;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BloggingModule.Blazor.Modals
{
    /// <summary>
    /// The SchoolClassNotificationSelection component is responsible for selecting a school class
    /// and navigating to the message creation page for the selected class.
    /// </summary>
    public partial class CategorySelectionForNewBlogEntryModal
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> CategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts to users.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// The selected school class.
        /// </summary>
        private CategoryDto _selectedCategory = null!;

        /// <summary>
        /// Collection of school class view models.
        /// </summary>
        private IEnumerable<CategoryDto> _availableCategories = [];

        /// <summary>
        /// Converter function for displaying school class names.
        /// </summary>
        private readonly Func<CategoryDto, string> _categoryConverter = p => p?.Name != null ? p.Name : "";

        /// <summary>
        /// Submits the selected school class and navigates to the message creation page.
        /// </summary>
        private void SubmitAsync()
        {
            if(_selectedCategory is null)
            {
                SnackBar.AddError("Please select the category first");
                return;
            }

            MudDialog.Close();
            NavigationManager.NavigateTo($"/blog/create/{_selectedCategory.CategoryId}");
        }

        /// <summary>
        /// Cancels the selection process and closes the dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the school classes from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await CategoryService.CategoriesAsync();
            result.ProcessResponseForDisplay(SnackBar, () => {
                _availableCategories = result.Data;
            });
            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}
