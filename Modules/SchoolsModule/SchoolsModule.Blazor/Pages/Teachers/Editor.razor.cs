using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Teachers
{
    /// <summary>
    /// The Editor component is responsible for editing teacher details.
    /// </summary>
    public partial class Editor
    {
        /// <summary>
        /// Gets or sets the service used to manage school class data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage school grade data.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations within the application.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the accounts provider for interacting with user account-related data.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

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
        /// Gets or sets the ID of the teacher to be edited.
        /// </summary>
        [Parameter] public string TeacherId { get; set; } = null!;

        private EditForm _addressDetailsForm = null!;
        private string _viewType = null!;

        /// <summary>
        /// Breadcrumb items for navigation.
        /// </summary>
        private List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Learners", href: "/learners", icon: Icons.Material.Filled.People),
            new("Update Learner", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";
        private TeacherViewModel _teacher = new();
        private UserInfoDto? _userInfo;

        // Collection of available school grades
        public ICollection<SchoolGradeViewModel>? AvailableGrades { get; set; } = [];
        readonly Func<SchoolGradeViewModel, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        // Collection of available school classes
        public ICollection<SchoolClassViewModel>? AvailableSchoolClasses { get; set; } = [];
        readonly Func<SchoolClassViewModel, string> _availableSchoolConverter = p => p?.SchoolClass != null ? p.SchoolClass : "";

        /// <summary>
        /// Handles the event when the selected grade is changed.
        /// Updates the available school classes based on the selected grade.
        /// </summary>
        /// <param name="schoolGrade">The selected school grade.</param>
        private async Task SelectedGradeChanged(SchoolGradeViewModel schoolGrade)
        {
            if (_teacher.Grade?.SchoolGradeId != schoolGrade.SchoolGradeId || _teacher.Grade == null)
            {
                _teacher.Grade = schoolGrade;

                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = schoolGrade.SchoolGradeId });
                AvailableSchoolClasses = schoolClassResult.Data.Select(c => new SchoolClassViewModel(c)).ToList();

                _teacher.Class = AvailableSchoolClasses.FirstOrDefault();
            }

            StateHasChanged();
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
        /// Updates the teacher details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAsync()
        {
            var result = await TeacherService.UpdateAsync(_teacher.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () => { SnackBar.AddSuccess("Team member successfully updated"); });
        }

        /// <summary>
        /// Cancels the edit operation and navigates back to the teachers list.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CancelAsync()
        {
            NavigationManager.NavigateTo("teachers");
        }

        /// <summary>
        /// Initializes the component and loads the teacher details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await TeacherService.TeacherAsync(TeacherId);
            if (result.Succeeded)
            {
                _teacher = new TeacherViewModel(result.Data);

                var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
                AvailableGrades = (schoolGradeResult.Data ?? Enumerable.Empty<SchoolGradeDto>()).Select(c => new SchoolGradeViewModel(c))
                    .OrderByNumericText(static grade => grade.SchoolGrade).ToList();

                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100 });
                AvailableSchoolClasses = (schoolClassResult.Data ?? Enumerable.Empty<SchoolClassDto>()).Select(c => new SchoolClassViewModel(c))
                    .OrderByNumericText(static schoolClass => schoolClass.SchoolClass).ToList();

                var userInfoResult = await UserService.GetUserInfoAsync(TeacherId);
                if (userInfoResult is { Succeeded: true, Data: not null })
                {
                    _userInfo = userInfoResult.Data;
                }
            }
            else
            {
                SnackBar.AddErrors(result.Messages);
                await base.OnInitializedAsync();
            }
                
        }
    }
}
