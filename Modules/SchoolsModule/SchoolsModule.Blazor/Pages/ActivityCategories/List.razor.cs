using GroupingModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Blazor.Modals;

namespace SchoolsModule.Blazor.Pages.ActivityCategories
{
    /// <summary>
    /// The List component is responsible for displaying a list of activity categories.
    /// It provides functionality to create new events for activity categories.
    /// </summary>
    public partial class List
    {
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
        /// Injected configuration service for accessing application settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the parent category view model.
        /// </summary>
        [Parameter] public CategoryViewModel? ParenCategory { get; set; }

        /// <summary>
        /// Opens a dialog to create a new event for the activity category and navigates to the event creation page if confirmed.
        /// </summary>
        private async Task CreateNewEventForActivityEvent()
        {
            // Initialize dialog parameters with the parent category
            var parameters = new DialogParameters<AddEventToActivityCategoryModal>()
            {
                { x => x.ParentCategory, ParenCategory },
            };

            // Show the dialog and wait for the result
            var dialog = await DialogService.ShowAsync<AddEventToActivityCategoryModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the dialog was not canceled, navigate to the event creation page
            if (!result.Canceled)
            {
                var categoryViewModel = (CategoryViewModel)result.Data;
                NavigationManager.NavigateTo($"/activities/activitygroups/events/create/{categoryViewModel.CategoryId}");
            }
        }
    }
}