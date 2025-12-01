using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.ActivityGroups
{
    /// <summary>
    /// Represents the attendance component for managing and displaying activity group data.
    /// </summary>
    /// <remarks>This component is responsible for loading and displaying information about a specific
    /// activity group identified by the <see cref="ActivityGroupId"/>. It fetches the data asynchronously during
    /// initialization and processes the response for display.</remarks>
    public partial class CreateAttendance
    {
        private ActivityGroupDto? _activityGroup;
        private bool _loaded;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the activity group.
        /// </summary>
        [Parameter] public string ActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and loads the activity group data.
        /// </summary>
        /// <remarks>This method retrieves the activity group data from the specified provider using the
        /// <see cref="ActivityGroupId"/>.  If the data retrieval is successful, the activity group is set for display. 
        /// Additionally, this method ensures that the component's initialization state is updated.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await ActivityGroupQueryService.ActivityGroupAsync(ActivityGroupId);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                _activityGroup = result.Data;
            });
            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}
