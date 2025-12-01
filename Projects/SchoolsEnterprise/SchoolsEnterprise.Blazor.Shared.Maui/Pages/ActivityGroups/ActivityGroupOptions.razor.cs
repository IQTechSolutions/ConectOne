using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.ActivityGroups
{
    /// <summary>
    /// Represents the options and state for managing activity groups within the application.
    /// </summary>
    /// <remarks>This class provides functionality for initializing and managing activity group data,
    /// including retrieving activity groups for the authenticated user and navigating to specific activity group
    /// details. It relies on dependency injection for HTTP communication and user notifications.</remarks>
    public partial class ActivityGroupOptions
    {
        private bool _loaded;
        private bool _busySending;
        private ClaimsPrincipal _user;
        private string _userDisplayName;
        private List<ActivityGroupDto> _activityGroups = new();

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider must be set before making any HTTP requests. Dependency injection is
        /// used to supply the implementation.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the service used to query activity groups.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; }

        /// <summary>
        /// Navigates to the attendance page for the specified activity group.
        /// </summary>
        /// <remarks>The navigation is performed immediately, and the browser's history is updated to
        /// reflect the new URL.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group. This value cannot be null or empty.</param>
        /// <returns></returns>
        private async Task NavigateToAttendance(string activityGroupId)
        {
            NavigationManager.NavigateTo($"/activityGroups/{activityGroupId}", true);
        }

        #region Lifecycle

        /// <summary>
        /// Asynchronously initializes the component, retrieving the user's authentication state and loading activity
        /// group data.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and initializes
        /// user-specific data, such as the display name and email.  It also fetches a paginated list of activity groups
        /// associated with the user and processes the response for display.  Once the data is loaded, the component's
        /// state is updated to reflect that initialization is complete.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userDisplayName = _user.GetUserDisplayName() ?? "Unknown User";

            var _activityGroupsResult = await ActivityGroupQueryService.PagedActivityGroupsAsync(new ActivityGroupPageParameters() { CoachEmail = _user.GetUserEmail(), PageSize = 50});
            _activityGroupsResult.ProcessResponseForDisplay(Snackbar, () =>
            {
                _activityGroups = _activityGroupsResult.Data.ToList();
            });

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
