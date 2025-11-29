using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.SchoolGrades
{
    /// <summary>
    /// Represents the page for editing an existing school grade.
    /// This component provides a form to update school grade details and save them to the server.
    /// </summary>
    public partial class Editor
    {
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";
        private SchoolGradeViewModel _schoolGrade = new SchoolGradeViewModel();
        private readonly List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Grades", href: "/schoolgrades", icon: Icons.Material.Filled.People),
            new("Update Grade", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };
        private int _totalTeachers;
        private int _teacherPage;
        private List<TeacherDto> _teachers;

        /// <summary>
        /// Gets or sets the service used to manage school grade data.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations within the application.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications to clients.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications to learners.
        /// </summary>
        [Inject] public ILearnerNotificationService LearnerNotificationService { get; set; } = null!;

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
        /// Gets or sets the identifier for the school grade.
        /// </summary>
        [Parameter] public string SchoolGradeId { get; set; } = null!;

        private TeacherPageParameters _teacherTableArgs = new();
        private async Task<TableData<TeacherDto>> TeacherTableLoad(TableState state, CancellationToken token)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<TeacherDto> { TotalItems = _totalTeachers, Items = _teachers };
        }
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            _teacherTableArgs.PageSize = pageSize;
            _teacherTableArgs.PageNr = pageNumber + 1;

            var request = await TeacherService.PagedTeachersAsync(_teacherTableArgs);
            if (request.Succeeded)
            {
                _totalTeachers = request.TotalCount;
                _teacherPage = request.CurrentPage;
                _teachers = request.Data;
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Handles the event when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Sends a push notification associated with a specific message.
        /// </summary>
        /// <param name="message">The source message, from which the notification details are extracted.</param>
        private async Task SendPushNotification(MessageDto message)
        {
            var notification = new NotificationDto()
            {
                MessageType = message.MessageType,
                EntityId = message.EntityId,
                ReceiverId = message.ReceiverId,
                ReceiverName = message.ReceiverName,
                ReceiverImageUrl = message.ReceiverImageUrl,
                Title = message.Subject,
                ShortDescription = message.ShortDescription,
                Message = message.Message,
                NotificationUrl = $"/messages/{message.MessageId}",
                Created = DateTime.Now
            };

            var userListResult = await LearnerNotificationService.LearnersNotificationList(new LearnerPageParameters
                {
                    Gender = message.Gender,
                    GradeId = message.EntityId
                }
            );

            var result = await PushNotificationService.SendNotifications(userListResult.Data, notification);
            if (result.Succeeded)
                SnackBar.AddSuccess("Messages notification was resent successfully");
            else
                SnackBar.AddErrors(result.Messages);
        }

        /// <summary>
        /// Updates the school grade by sending the updated details to the server.
        /// </summary>
        public async Task UpdateAsync()
        {
            var result = await SchoolGradeService.UpdateAsync(_schoolGrade.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"{_schoolGrade.SchoolGrade} was updated successfully");
            });
        }

        /// <summary>
        /// Cancels the update process and navigates back to the school grades list.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/schoolgrades");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the existing school grade details from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolGradeService.SchoolGradeAsync(SchoolGradeId);
            if (result.Succeeded)
            {
                _schoolGrade = new SchoolGradeViewModel(result.Data);
                _teacherTableArgs.GradeId = _schoolGrade?.SchoolGradeId;
            }
            SnackBar.AddErrors(result.Messages);
            await base.OnInitializedAsync();
        }
    }
}
