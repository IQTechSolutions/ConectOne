using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Learners
{
    /// <summary>
    /// The Creator component is responsible for creating a new learner.
    /// </summary>
    public partial class Creator
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
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute learner-related commands.
        /// </summary>
        [Inject] public ILearnerCommandService LearnerCommandService { get; set; } = null!;

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

        #endregion

        #region Fields and Properties

        /// <summary>
        /// Breadcrumb items for navigation.
        /// </summary>
        private List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Learners", href: "/learners", icon: Icons.Material.Filled.People),
            new("Create New Learner", href: null, disabled: true, icon: Icons.Material.Filled.Create)
        ];

        /// <summary>
        /// Default image source for the learner's profile picture.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// List of selected parents for the learner.
        /// </summary>
        private List<ParentDto>? _selectedParents = new();

        /// <summary>
        /// ViewModel for the learner being created.
        /// </summary>
        private readonly LearnerViewModel _learner = new LearnerViewModel();

        /// <summary>
        /// Collection of available grades.
        /// </summary>
        public ICollection<SchoolGradeDto>? AvailableGrades { get; set; } = new List<SchoolGradeDto>();

        /// <summary>
        /// Converter function for displaying school grades.
        /// </summary>
        readonly Func<SchoolGradeDto?, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        /// <summary>
        /// Collection of available school classes.
        /// </summary>
        public ICollection<SchoolClassDto>? AvailableSchoolClasses { get; set; } = new List<SchoolClassDto>();

        /// <summary>
        /// Converter function for displaying school classes.
        /// </summary>
        readonly Func<SchoolClassDto?, string> _availableSchoolConverter = p => p?.SchoolClass != null ? p.SchoolClass : "";

        #endregion

        #region Event Handlers

        /// <summary>
        /// Updates the cover image when a new image is selected.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Handles the event when the value selection is changed for parents.
        /// </summary>
        /// <param name="args">The event arguments containing the selected parent.</param>
        private async Task OnValueSelectionChanged(CustomCheckValueChangedEventArgs<ParentDto> args)
        {
            if (args.IsChecked)
            {
                _selectedParents?.Add(args.Item);
                if (_learner.MedicalAidParent is null)
                {
                    _learner.MedicalAidParent = args.Item;
                }
            }
            else
            {
                _selectedParents?.Remove(_selectedParents.FirstOrDefault(c => c.ParentId == args.Item.ParentId));
                if (_learner.MedicalAidParent == args.Item)
                {
                    _learner.MedicalAidParent = null;
                }
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles the event when the selected grade is changed.
        /// </summary>
        /// <param name="schoolGrade">The selected school grade.</param>
        public async Task SelectedGradeChanged(SchoolGradeDto schoolGrade)
        {
            if (_learner.SelectedGrade?.SchoolGradeId != schoolGrade.SchoolGradeId || _learner.SelectedGrade == null)
            {
                _learner.SelectedGrade = schoolGrade;

                // Fetch available school classes for the selected grade
                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = schoolGrade.SchoolGradeId });
                AvailableSchoolClasses = schoolClassResult.Data;

                // Set the first available school class as the selected one
                _learner.SelectedSchoolClass = AvailableSchoolClasses.FirstOrDefault();
            }

            StateHasChanged();
        }

        /// <summary>
        /// Handles the event when the identity number is changed.
        /// </summary>
        /// <param name="value">The new identity number value.</param>
        public async Task IdentityNrChanged(string value)
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new learner by making an API call.
        /// </summary>
        public async Task CreateAsync()
        {
            try
            {
                var result = await LearnerCommandService.CreateAsync(_learner.ToDto());
                result.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    var parentResult = await LearnerCommandService.UpdateLearnerParentsAsync(_learner.LearnerId, _selectedParents);
                    if (!parentResult.Succeeded) SnackBar.AddErrors(parentResult.Messages);

                    SnackBar.Add("Learner created successfully", Severity.Success);
                    NavigationManager.NavigateTo("learners");
                });
            }
            catch (Exception ex)
            {
                SnackBar.Add($"An error occurred while creating the learner: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Cancels the operation and navigates back to the learners list.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/learners", true);
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and loads the necessary data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Load available grades
                var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
                AvailableGrades = schoolGradeResult.Data;

                // Load available school classes
                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100 });
                AvailableSchoolClasses = schoolClassResult.Data;

                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                SnackBar.Add($"An error occurred while initializing the component: {ex.Message}", Severity.Error);
            }
        }

        #endregion
    }
}
