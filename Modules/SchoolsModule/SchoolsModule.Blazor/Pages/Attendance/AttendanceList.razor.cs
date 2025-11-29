using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Enums;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Attendance
{
    /// <summary>
    /// Represents a component for managing and saving attendance data for a specific group and date.
    /// </summary>
    /// <remarks>The <see cref="AttendanceList"/> class provides functionality to load, display, and save
    /// attendance data  for learners in a specified group. It integrates with dependency-injected services such as 
    /// <see cref="IConfiguration"/>, <see cref="IBaseHttpProvider"/>, and <see cref="ISnackbar"/> to perform  HTTP
    /// operations, retrieve configuration settings, and display notifications.  This component is designed to be used
    /// in Blazor applications and supports asynchronous initialization  and operations. It relies on parameters such as
    /// <see cref="GroupId"/>, <see cref="GroupName"/>,  <see cref="AttendanceType"/>, and <see cref="Date"/> to define
    /// the context for attendance management.</remarks>
    public partial class AttendanceList
    {
        private List<LearnerAttendanceDto> _learners = [];

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The <see cref="Configuration"/> property is typically used to retrieve application
        /// settings and configuration values, such as connection strings, API keys, or other environment-specific
        /// settings.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IAttendanceService AttendanceService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation operations within the
        /// application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        [Parameter] public string GroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the group associated with this parameter.
        /// </summary>
        [Parameter] public string GroupName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of attendance associated with the current context.
        /// </summary>
        [Parameter] public AttendanceType AttendanceType { get; set; }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        [Parameter] public DateTime? Date { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Asynchronously initializes the component and performs any required setup.
        /// </summary>
        /// <remarks>This method is called automatically by the Blazor framework during the component's
        /// initialization phase. It ensures that necessary data is loaded and invokes the base implementation to
        /// complete the initialization.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await LoadLearners();
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Asynchronously loads the list of learners required for attendance and initializes their attendance status.
        /// </summary>
        /// <remarks>This method retrieves learner attendance data from the specified API endpoint using
        /// the current group ID and attendance type. The retrieved data is processed to initialize each learner's
        /// attendance status to <see cref="AttendanceStatus.Present"/>  and sets default values for other
        /// attendance-related properties.</remarks>
        /// <returns></returns>
        private async Task LoadLearners()
        {
            var groupResult = await AttendanceService.GetAttendanceListToCompleteAsync(new AttendanceListRequest(GroupId, AttendanceType));
            groupResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _learners = groupResult.Data.Select(l => new LearnerAttendanceDto
                {
                    LearnerId = l.LearnerId,
                    FullName = l.FullName,
                    SelectedStatus = AttendanceStatus.Present,
                    Notes = string.Empty,
                    GroupId = GroupId,
                    Date = Date
                }).ToList();
            });
        }

        /// <summary>
        /// Saves the attendance data for the specified group and date.
        /// </summary>
        /// <remarks>This method sends the attendance information to the server using the configured
        /// provider. If the operation is successful, a confirmation message is displayed.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        private async Task SaveAttendance()
        {
            var model = new AttendanceResultListRequest(GroupId, GroupName, AttendanceType, Date!.Value, _learners);
            var result = await AttendanceService.CreateAttendanceGroupAsync(model);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.Add("Attendance saved successfully.", Severity.Success);
                NavigationManager.NavigateTo($"attendancegroups/{GroupId}");
            });
        }
    }
}
