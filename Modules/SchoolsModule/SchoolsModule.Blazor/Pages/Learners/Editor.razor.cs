using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Blazor.Components.Parents;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Learners
{
    /// <summary>
    /// The Editor component is responsible for editing learner information.
    /// </summary>
    public partial class Editor
    {
        private readonly Func<ParentDto, string> _parentConverter = p => p?.FirstName != null ? $"{p?.FirstName} {p?.LastName}" : "";

        #region Injected Services

        /// <summary>
        /// Gets or sets the service used to manage school class data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage school grade data.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="ISchoolGradeService"/> to enable grade-related operations within the
        /// component.</remarks>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute learner-related commands.
        /// </summary>
        [Inject] public ILearnerCommandService LearnerCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be set manually
        /// in most scenarios.</remarks>
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

        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Parameter to receive the learner ID from the parent component or route.
        /// </summary>
        [Parameter] public string LearnerId { get; set; } = null!;

        #endregion

        #region Fields and Properties

        /// <summary>
        /// Breadcrumb items for navigation.
        /// </summary>
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Learners", href: "/learners", icon: Icons.Material.Filled.People),
            new("Update Learner", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

        /// <summary>
        /// Default image source for the learner's profile picture.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// ViewModel to hold learner data.
        /// </summary>
        private LearnerViewModel _learner = new();

        /// <summary>
        /// User information.
        /// </summary>
        private UserInfoDto? _userInfo;

        /// <summary>
        /// Reference to the ParentsTable component.
        /// </summary>
        private ParentsTable _parentsTable = null!;

        /// <summary>
        /// List of selected parents.
        /// </summary>
        private List<ParentDto>? _selectedParents = new();

        /// <summary>
        /// Collection of available grades.
        /// </summary>
        public ICollection<SchoolGradeDto> AvailableGrades { get; set; } = new List<SchoolGradeDto>();

        /// <summary>
        /// Converter function for displaying school grades.
        /// </summary>
        readonly Func<SchoolGradeDto?, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        /// <summary>
        /// Collection of available school classes.
        /// </summary>
        public ICollection<SchoolClassDto> AvailableSchoolClasses { get; set; } = new List<SchoolClassDto>();

        /// <summary>
        /// Converter function for displaying school classes.
        /// </summary>
        readonly Func<SchoolClassDto?, string> _availableSchoolConverter = p => p?.SchoolClass != null ? p.SchoolClass : "";

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the event when the value selection is changed for parents.
        /// </summary>
        /// <param name="args">The event arguments containing the selected parent.</param>
        private async Task OnValueSelectionChanged(CustomCheckValueChangedEventArgs<ParentDto> args)
        {
            //if (args.IsChecked)
            //{
            //    _selectedParents?.Add(args.Item!);
            //    if (string.IsNullOrEmpty(_learner.MedicalAidParentId))
            //    {
            //        _learner.MedicalAidParentId = args.Item.ParentId;
            //    }
            //}
            //else
            //{
            //    var toRemove = _selectedParents?.FirstOrDefault(c => c.ParentId == args.Item?.ParentId);
            //    if (toRemove is not null)
            //    {
            //        _selectedParents?.Remove(toRemove);
            //    }

            //    if (_learner.MedicalAidParentId == args.Item?.ParentId)
            //    {
            //        _learner.MedicalAidParentId = null;
            //    }
            //}

            //await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Updates the cover image when a new image is selected.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Handles the event when the selected grade is changed.
        /// </summary>
        /// <param name="schoolGrade">The selected school grade.</param>
        public async Task SelectedGradeChanged(SchoolGradeDto schoolGrade)
        {
            if (_learner.SelectedGrade?.SchoolGradeId != schoolGrade.SchoolGradeId)
            {
                _learner.SelectedGrade = schoolGrade;

                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = schoolGrade.SchoolGradeId });
                AvailableSchoolClasses = schoolClassResult.Data;

                _learner.SelectedSchoolClass = AvailableSchoolClasses.FirstOrDefault();
            }

            StateHasChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the learner information by making an API call.
        /// </summary>
        public async Task UpdateAsync()
        {
            var learner = _learner.ToDto() with { SelectedParents = _selectedParents! };
            var result = await LearnerCommandService.UpdateAsync(learner);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.Add("Learner updated successfully", Severity.Success);
            });
        }

        /// <summary>
        /// Cancels the operation and navigates back to the learners list.
        /// </summary>
        public void Cancel() => NavigationManager.NavigateTo("/learners", true);

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and loads the necessary data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Load learner data
            var result = await LearnerQueryService.LearnerAsync(LearnerId);
            if (result.Succeeded)
            {
                _learner = new LearnerViewModel(result.Data);

                _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_learner.CoverImageUrl?.TrimStart('/')}";

                // Load learner parents data
                var learnerParentsResult = await LearnerQueryService.LearnerParentsAsync(LearnerId);
                if (learnerParentsResult.Succeeded)
                {
                    foreach (var item in learnerParentsResult.Data)
                    {
                        if (_selectedParents!.All(c => c.ParentId != item.ParentId))
                        {
                            _selectedParents!.Add(item);
                        }
                    }
                }

                // Load user information
                var userInfoResult = await UserService.GetUserInfoAsync(LearnerId);
                if (userInfoResult is { Succeeded: true, Data: not null })
                {
                    _userInfo = userInfoResult.Data;
                }
            }
            SnackBar.AddErrors(result.Messages);

            // Load available grades
            var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
            AvailableGrades = schoolGradeResult.Data;

            // Load available school classes based on the selected grade
            var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = _learner.SelectedGrade?.SchoolGradeId });
            AvailableSchoolClasses = schoolClassResult.Data;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}

