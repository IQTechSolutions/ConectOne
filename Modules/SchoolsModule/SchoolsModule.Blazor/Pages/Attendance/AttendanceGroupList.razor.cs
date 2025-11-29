using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Enums;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Attendance
{
    /// <summary>
    /// Represents a component that manages and displays a list of attendance groups,  allowing for operations such as
    /// loading attendance data and exporting attendance records.
    /// </summary>
    /// <remarks>This class is designed to interact with attendance-related APIs and provide functionality 
    /// for managing attendance data grouped by date. It includes support for downloading attendance  records as files
    /// and displaying notifications for user feedback.</remarks>
    public partial class AttendanceGroupList
    {
        /// <summary>
        /// Represents a collection of attendance group data for learners.
        /// </summary>
        /// <remarks>This field holds a list of <see cref="AttendanceGroupListDto"/> objects,  which
        /// provide details about attendance groups associated with learners.</remarks>
        private IEnumerable<AttendanceGroupListDto> _learners = [];

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IAttendanceService AttendanceService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Service used to trigger file downloads in the browser.
        /// </summary>
        [Inject] public IBlazorDownloadFileManager BlazorDownloadFileManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        [Parameter] public string GroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of attendance for the event or session.
        /// </summary>
        [Parameter] public AttendanceType AttendanceType { get; set; }

        /// <summary>
        /// Asynchronously loads the list of learners and their attendance details for the specified group and
        /// attendance type.
        /// </summary>
        /// <remarks>This method retrieves attendance data from the API and processes it for display. The
        /// data is grouped by date and prepared for further use in the application. Any errors encountered during the
        /// API call are displayed using the provided snack bar.</remarks>
        private async Task LoadLearners()
        {
            var groupResult = await AttendanceService.GetAttendanceListToCompleteAsync(new AttendanceListRequest(GroupId, AttendanceType));
            groupResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                var learners = groupResult.Data.Select(l => new LearnerAttendanceDto
                {
                    GroupId = l.GroupId,
                    LearnerId = l.LearnerId,
                    FullName = l.FullName,
                    SelectedStatus = AttendanceStatus.Present,
                    Date = l.Date,
                    Notes = l.Notes
                }).GroupBy(c => c.Date!.Value.Date).Select(g => new AttendanceGroupListDto() { Date = g.Key, Results = g.ToList()});
            });
        }

        /// <summary>
        /// Exports the attendance list for a specified activity group on a given date.
        /// </summary>
        /// <remarks>This method sends a request to export the attendance list for the specified group and
        /// downloads the resulting file. The downloaded file is named using the format "Class Attendance List_<paramref
        /// name="date"/>.xlsx".</remarks>
        /// <param name="groupId">The unique identifier of the activity group to export attendance for. Cannot be null or empty.</param>
        /// <param name="date">The date for which the attendance list is being exported.</param>
        /// <returns></returns>
        private async Task ExportActivityGroup(string groupId, DateTime date)
        {
            var response = await AttendanceService.ExportAttendanceGroup(new ExportAttendanceRequest() { GroupId = groupId});
            if (!response.Succeeded && response.Messages != null)
                SnackBar.AddErrors(response.Messages);

            await BlazorDownloadFileManager.DownloadFile($"Class Attendace List_{date:ddMMyyyy}.xlsx", Convert.FromBase64String(response.Data),
                "application/octet-stream");
            SnackBar.Add("Class Attendace List Exported", Severity.Success);
        }
    }

    /// <summary>
    /// Represents a data transfer object containing a list of attendance groups and related metadata.
    /// </summary>
    /// <remarks>This class is used to encapsulate attendance group information, including whether detailed
    /// data  should be displayed, an optional date associated with the attendance, and a collection of  attendance
    /// results.</remarks>
    public class AttendanceGroupListDto()
    {
        /// <summary>
        /// Gets or sets a value indicating whether detailed information should be displayed.
        /// </summary>
        public bool ShowDetails { get; set; }

        /// <summary>
        /// Gets or sets the date associated with the current instance.
        /// </summary>
        public DateTime? Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the collection of learner attendance records.
        /// </summary>
        public List<LearnerAttendanceDto> Results { get; set; } = [];
    }
}
