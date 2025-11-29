using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.SchoolClasses
{
    /// <summary>
    /// The Creator component is responsible for creating new school classes.
    /// It provides a form for inputting school class details and handles the submission process.
    /// </summary>
    public partial class Creator
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

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
        /// List of breadcrumb items for navigation.
        /// </summary>
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new BreadcrumbItem("Classes", href: "/learners", icon: Icons.Material.Filled.People),
            new BreadcrumbItem("Create Class", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Source URL for the cover image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// ViewModel for the school class being created.
        /// </summary>
        private readonly SchoolClassViewModel _schoolClass = new();

        /// <summary>
        /// The ID of the grade to which the class belongs.
        /// </summary>
        [Parameter] public string GradeId { get; set; } = null!;

        /// <summary>
        /// Updates the cover image source when the image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Converter function for displaying school grade names.
        /// </summary>
        private readonly Func<SchoolGradeViewModel, string> _schoolGradeConverter = p => p?.SchoolGrade != null ? p.SchoolGrade : "";

        /// <summary>
        /// Creates a new school class by sending a PUT request to the API.
        /// Displays a success message upon successful creation and navigates to the update page for the grade.
        /// </summary>
        public async Task CreateAsync()
        {
            var result = await SchoolClassService.CreateAsync(_schoolClass.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"{_schoolClass.SchoolClass} was created successfully");
                NavigationManager.NavigateTo($"/schoolgrades/update/{GradeId}");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the school classes page.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo($"/schoolgrades/update/{GradeId}");
        }

        /// <summary>
        /// Initializes the component by setting the grade ID for the school class.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _schoolClass.GradeId = GradeId;
            await base.OnInitializedAsync();
        }
    }
}
