using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Interfaces;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.ActivityGroups
{
    /// <summary>
    /// The Update component is responsible for updating an existing Activity Group.
    /// </summary>
    public partial class Update
    {
        private LearnerPageParameters _args = new LearnerPageParameters();
        private ActivityGroupViewModel _activityGroup = new ActivityGroupViewModel();
        private string _imageSource = "_content/SchoolsModule.Blazor/images/NoImage.jpg";
        private IEnumerable<TeacherViewModel> _teachers = null!;
        private readonly Func<TeacherViewModel, string> teacherConverter = p => p?.FirstName != null ? $"{p?.FirstName} {p?.LastName}" : "";

        #region Injections and Parameters

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query activity group data.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="ITeacherService"/> to enable teacher management features.</remarks>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupCommandService ActivityGroupCommandService { get; set; } = null!;

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
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupNotificationService ActivityGroupNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications to clients.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;

        /// <summary>
        /// The ID of the Activity Group to be updated.
        /// </summary>
        [Parameter] public string ActivityGroupId { get; set; } = null!;

        #endregion
        
        #region Actions

        /// <summary>
        /// Sends a push notification regarding a <see cref="MessageDto"/>.
        /// </summary>
        /// <param name="message">The message DTO containing notification details.</param>
        private async Task SendPushNotification(MessageDto message)
        {
            var notification = new NotificationDto
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

            var userListResult = await ActivityGroupNotificationService.ActivityGroupCategoryNotificationList(message.EntityId);

            var result = await PushNotificationService.SendNotifications(userListResult.Data, notification);
            if (result.Succeeded)
                SnackBar.AddSuccess("Messages notification was resent successfully");
            else
                SnackBar.AddErrors(result.Messages);
        }

        /// <summary>
        /// Updates the cover image URL when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Updates the Activity Group by sending the updated data to the server.
        /// </summary>
        private async Task UpdateAsync()
        {
            // Ensure that an age group is selected before proceeding
            if (_activityGroup.SelectedAgeGroup is null)
            {
                SnackBar.AddError("Age group required!");
                return;
            }

            // Send the updated Activity Group data to the server
            var result = await ActivityGroupCommandService.UpdateAsync(_activityGroup.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.Add($"Action Successful. Activity Group \"{_activityGroup.Name}\" was successfully updated.", Severity.Success);
                NavigationManager.NavigateTo($"/activities/activitygroups/update/{_activityGroup.ActivityGroupId}");
            });
        }

        /// <summary>
        /// Handles the event when the selected items in the learner table change.
        /// </summary>
        /// <param name="args">The event arguments containing the selected learner.</param>
        private void LearnerTableSelectedItemsChanged(CustomCheckValueChangedEventArgs<LearnerDto> args)
        {
            if (args.IsChecked)
            {
                _activityGroup.SelectedTeamMembers.Add(args.Item);
            }
            else
            {
                _activityGroup.SelectedTeamMembers.Remove(_activityGroup.SelectedTeamMembers.FirstOrDefault(c => c.LearnerId == args.Item.LearnerId));
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the update page.
        /// </summary>
        private void CancelCreation()
        {
            NavigationManager.NavigateTo($"/activities/categories/update/{_activityGroup.ActivityGroupId}");
        }

        /// <summary>
        /// Updates the selected gender.
        /// </summary>
        /// <param name="gender">The selected gender.</param>
        public async Task GenderChanged(Gender gender)
        {
            _activityGroup.Gender = gender;
            _args.Gender = gender;

            // Reload the table data to reflect the changes
        }

        /// <summary>
        /// Updates the selected teacher.
        /// </summary>
        /// <param name="teacher">The selected teacher.</param>
        public void TeacherChanged(TeacherViewModel? teacher)
        {
            _activityGroup.SelectedTeacher = teacher;
        }

        /// <summary>
        /// Handles the search event for team members.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async void OnTeamMemberSearh(string text)
        {
            _args.SearchText = text;
        }

        /// <summary>
        /// Updates the selected age group.
        /// </summary>
        /// <param name="ageGroup">The selected age group.</param>
        public async Task AgeGroupChanged(AgeGroupViewModel? ageGroup)
        {
            _activityGroup.SelectedAgeGroup = ageGroup;
            _args.MaxAge = _activityGroup.SelectedAgeGroup is null ? 100 : _activityGroup.SelectedAgeGroup.MaxAge;
            _args.MinAge = _activityGroup.SelectedAgeGroup is null ? 0 : _activityGroup.SelectedAgeGroup.MinAge;

            // Reload the table data to reflect the changes
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Lifecycle method called when the component is initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await ActivityGroupQueryService.ActivityGroupAsync(ActivityGroupId);
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {               

                _activityGroup = new ActivityGroupViewModel(result.Data);
                _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_activityGroup.CoverImageUrl.TrimStart('/')}";

                _args.ActivityGroupId = ActivityGroupId;
                _args.Gender = _activityGroup.Gender;
                _args.MaxAge = _activityGroup.SelectedAgeGroup is null ? 100 : _activityGroup.SelectedAgeGroup.MaxAge;
                _args.MinAge = _activityGroup.SelectedAgeGroup is null ? 0 : _activityGroup.SelectedAgeGroup.MinAge;

                var amenityResult = await TeacherService.AllTeachersAsync(default);
                if (amenityResult.Succeeded)
                {
                    _teachers = amenityResult.Data.Select(c => new TeacherViewModel(c));
                }
            });

            await base.OnInitializedAsync();
        }

        #endregion

        /// <summary>
        /// Performs a search for teachers whose first or last names contain the specified value.
        /// </summary>
        /// <remarks>The search is case-insensitive and uses <see
        /// cref="StringComparison.InvariantCultureIgnoreCase"/> for comparisons.</remarks>
        /// <param name="value">The search term to match against teacher names. If null or empty, all teachers are returned.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A collection of <see cref="TeacherViewModel"/> objects representing the teachers whose first or last names
        /// match the search term. If no matches are found, an empty collection is returned.</returns>
        private async Task<IEnumerable<TeacherViewModel>> AutoCompleteSearch(string value, CancellationToken token)
        {
            await Task.Delay(1, token);

            if (string.IsNullOrEmpty(value))
                return _teachers;

            return _teachers.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
