using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Blazor.Pages.ActivityGroups
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
        /// Gets or sets the <see cref="Snackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected and used to display transient messages, such as
        /// alerts or status updates, in the user interface. Ensure that the dependency injection container is properly
        /// configured to provide an instance of <see cref="Snackbar"/>.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; }

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
