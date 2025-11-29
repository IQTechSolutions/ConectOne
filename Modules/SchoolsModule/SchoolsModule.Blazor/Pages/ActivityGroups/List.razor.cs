using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Blazor.Modals;

namespace SchoolsModule.Blazor.Pages.ActivityGroups
{
    /// <summary>
    /// The List component is responsible for displaying a list of activity groups.
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
        /// Gets or sets the ID of the category to which the activity groups belong.
        /// </summary>
        public string CategoryId { get; set; } = null!;

        /// <summary>
        /// Opens a dialog to create a new event for the activity group and navigates to the event creation page if confirmed.
        /// </summary>
        private async Task CreateNewEventForActivityEvent()
        {
            // Initialize dialog parameters for the AddEventToActivityCategoryModal
            var parameters = new DialogParameters<AddEventToActivityCategoryModal>();

            // Show the dialog and wait for the result
            var dialog = await DialogService.ShowAsync<AddEventToActivityCategoryModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the dialog was not canceled, navigate to the event creation page
            if (!result!.Canceled)
            {
                NavigationManager.NavigateTo($"/activities/activitygroups/events/create/{result}");
            }
        }
    }
}
