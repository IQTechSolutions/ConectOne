using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.SchoolClasses
{
    /// <summary>
    /// Represents the options and state for managing an attendance list within a school class context.
    /// </summary>
    /// <remarks>This class is responsible for handling the initialization and navigation logic related to
    /// attendance lists. It interacts with authentication and HTTP services to retrieve and display school class
    /// data.</remarks>
    public partial class AttendanceListOptions 
    {
        private bool _loaded;
        private bool _busySending;
        private ClaimsPrincipal _user;
        private string _userDisplayName;
        private List<SchoolClassDto> _schoolClasses = new();

        /// <summary>
        /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication
        /// state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and manage URIs within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation actions and access information about the current URI.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snackbars in the application.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Navigates to the attendance page for a specified school class.
        /// </summary>
        /// <param name="schoolClassId">The unique identifier of the school class whose attendance page is to be displayed. Cannot be null or empty.</param>
        private async Task NavigateToAttendance(string schoolClassId)
        {
            NavigationManager.NavigateTo($"/schoolclasses/attendance/{schoolClassId}", true);
        }

        #region Lifecycle

        /// <summary>
        /// Asynchronously initializes the component, setting up user and school class data.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and display name. It
        /// also fetches a paginated list of school classes associated with the user's email and processes the response
        /// for display. The component's loaded state is set to true upon completion.</remarks>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userDisplayName = _user.GetUserDisplayName() ?? "Unknown User";

            var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters() { TeacherEmail = _user.GetUserEmail()});
            schoolClassResult.ProcessResponseForDisplay(Snackbar, () =>
            {
                _schoolClasses = schoolClassResult.Data.ToList();
            });

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
