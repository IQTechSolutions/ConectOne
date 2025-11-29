using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NeuralTech.Shared.Extensions;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Teachers
{
    /// <summary>
    /// Represents the page for creating a new teacher.
    /// This component provides a form to input teacher details and save them to the server.
    /// </summary>
    public partial class Creator
    {
        private string? _coverImageToUpload;

        /// <summary>
        /// Gets or sets the service used to manage school class data.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="ISchoolClassService"/> to enable operations related to school
        /// classes.</remarks>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve school grade information.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

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

        // Breadcrumb items for navigation
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new ("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new ("Teachers", href: "/teachers", icon: Icons.Material.Filled.People),
            new ("Create New Teacher", href: null, disabled: true, icon: Icons.Material.Filled.Create)
        };

        // Default image source for the teacher's profile picture
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        // ViewModel for the teacher being created
        private TeacherViewModel Teacher { get; set; } = new()
        {
            ContactNumber = new ContactNumberViewModel() { InternationalCode = "27", Default = true },
            EmailAddress = new EmailAddressViewModel() { Default = true }
        };

        // Collection of available school grades
        public ICollection<SchoolGradeViewModel>? AvailableGrades { get; set; } = [];
        readonly Func<SchoolGradeViewModel, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        // Collection of available school classes
        public ICollection<SchoolClassViewModel>? AvailableSchoolClasses { get; set; } = [];
        readonly Func<SchoolClassViewModel, string> _availableSchoolConverter = p => p?.SchoolClass != null ? p.SchoolClass : "";

        /// <summary>
        /// Handles the event when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Handles the event when the selected grade is changed.
        /// Updates the available school classes based on the selected grade.
        /// </summary>
        /// <param name="schoolGrade">The selected school grade.</param>
        private async Task SelectedGradeChanged(SchoolGradeViewModel schoolGrade)
        {
            if (Teacher.Grade?.SchoolGradeId != schoolGrade.SchoolGradeId || Teacher.Grade == null)
            {
                Teacher.Grade = schoolGrade;

                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = schoolGrade.SchoolGradeId });
                AvailableSchoolClasses = schoolClassResult.Data.Select(c => new SchoolClassViewModel(c)).ToList();

                Teacher.Class = AvailableSchoolClasses.FirstOrDefault();
            }

            StateHasChanged();
        }

        /// <summary>
        /// Creates a new teacher by sending the teacher details to the server.
        /// </summary>
        private async Task CreateAsync()
        {
            var result = await TeacherService.CreateAsync(Teacher.ToDto());
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                SnackBar.Add("Teacher created successfully", Severity.Success);
                NavigationManager.NavigateTo("teachers");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the teachers list.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/teachers");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the available school grades and classes from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
            AvailableGrades = (schoolGradeResult.Data ?? Enumerable.Empty<SchoolGradeDto>()).Select(c => new SchoolGradeViewModel(c)).OrderByNumericText(static grade => grade.SchoolGrade).ToList();

            var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100 });
            AvailableSchoolClasses = (schoolClassResult.Data ?? Enumerable.Empty<SchoolClassDto>()).Select(c => new SchoolClassViewModel(c)).OrderByNumericText(static schoolClass => schoolClass.SchoolClass).ToList();

            await base.OnInitializedAsync();
        }
    }
}
