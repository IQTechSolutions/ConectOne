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
    /// Represents the options and state for managing attendance lists within the application.
    /// </summary>
    /// <remarks>This class is used to manage the lifecycle and state of attendance-related data, including
    /// user authentication, activity groups, and navigation to attendance pages. It integrates with dependency-injected
    /// services for HTTP communication and user notifications.</remarks>
    public partial class AttendanceListOptions 
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
        /// 
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query activity groups.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Navigates to the attendance page for the specified activity group.
        /// </summary>
        /// <remarks>This method performs a navigation operation to the attendance page using the provided
        /// activity group ID.  The navigation forces a reload of the target page.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group whose attendance page should be displayed. Cannot be null or
        /// empty.</param>
        /// <returns></returns>
        private async Task NavigateToAttendance(string activityGroupId)
        {
            NavigationManager.NavigateTo($"/activities/activitygroups/attendance/{activityGroupId}", true);
        }

        #region Lifecycle

        /// <summary>
        /// Asynchronously initializes the component, retrieving the current user's authentication state  and loading
        /// activity group data for display.
        /// </summary>
        /// <remarks>This method retrieves the authenticated user's information and uses it to fetch paged
        /// activity  group data. The data is processed and displayed, and the component's state is updated to indicate 
        /// that initialization is complete. This method also invokes the base class's initialization logic.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userDisplayName = _user.GetUserDisplayName() ?? "Unknown User";

            var _activityGroupsResult = await ActivityGroupQueryService.PagedActivityGroupsAsync(new ActivityGroupPageParameters() { CoachEmail = _user.GetUserEmail()});
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
