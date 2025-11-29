using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.SchoolClasses
{
    /// <summary>
    /// The Editor component is responsible for editing school class details.
    /// It provides a form for updating class information and handles the update process.
    /// </summary>
    public partial class Editor
    {
        private int _totalTeachers;
        private int _teacherPage;
        private List<TeacherDto> _teachers;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations within the application.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage school class data within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework to provide access to operations
        /// related to school classes, such as retrieval, creation, or modification. Ensure that the service is properly
        /// configured in the dependency injection container before use.</remarks>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve school grade information.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

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
        /// The ID of the school class to be edited.
        /// </summary>
        [Parameter] public string SchoolClassId { get; set; } = null!;

        /// <summary>
        /// Reference to the EditForm for address details.
        /// </summary>
        private EditForm _addressDetailsForm = null!;

        /// <summary>
        /// The type of view for the editor.
        /// </summary>
        private string _viewType = null!;

        /// <summary>
        /// List of breadcrumb items for navigation.
        /// </summary>
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Classes", href: "/learners", icon: Icons.Material.Filled.People),
            new("Update Class", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

        /// <summary>
        /// Source URL for the cover image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// ViewModel for the school class being edited.
        /// </summary>
        private SchoolClassViewModel _schoolClass = new SchoolClassViewModel();

        /// <summary>
        /// Collection of available school grades.
        /// </summary>
        [Parameter] public IEnumerable<SchoolGradeViewModel> AvailableSchoolGrades { get; set; } = [];

        /// <summary>
        /// Updates the cover image source when the image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        private TeacherPageParameters _teacherTableArgs = new ();
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
        /// Converter function for displaying school grade names.
        /// </summary>
        private Func<SchoolGradeViewModel, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        /// <summary>
        /// Updates the school class by sending a POST request to the API.
        /// Displays a success message upon successful update.
        /// </summary>
        public async Task UpdateAsync()
        {
            var result = await SchoolClassService.UpdateAsync(_schoolClass.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"{_schoolClass.SchoolClass} was updated successfully");
            });
        }

        /// <summary>
        /// Cancels the update process and navigates back to the school classes page.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo($"/schoolgrades/update/{_schoolClass.GradeId}");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the school class details and available school grades from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolClassService.SchoolClassAsync(SchoolClassId);
            if (result.Succeeded)
            {
                _schoolClass = new SchoolClassViewModel(result.Data);
                _teacherTableArgs.ClassId = _schoolClass?.SchoolClassId;

                var schoolGradesResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
                if (schoolGradesResult.Succeeded)
                {
                    AvailableSchoolGrades = schoolGradesResult.Data.Select(c => new SchoolGradeViewModel(c));
                }
            }
            SnackBar.AddErrors(result.Messages);
            await base.OnInitializedAsync();
        }
    }
}
