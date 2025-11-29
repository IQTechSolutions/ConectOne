using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.SchoolGrades
{
    /// <summary>
    /// The Creator component is responsible for creating new school grades.
    /// It provides a form for inputting school grade details and handles the submission process.
    /// </summary>
    public partial class Creator
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
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
        /// List of breadcrumb items for navigation.
        /// </summary>
        private readonly List<BreadcrumbItem> _items = new()
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Grades", href: "/schoolgrades", icon: Icons.Material.Filled.People),
            new("Create Grade", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Source URL for the cover image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// ViewModel for the school grade being created.
        /// </summary>
        private readonly SchoolGradeViewModel _schoolGrade = new();

        /// <summary>
        /// Updates the cover image source when the image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Creates a new school grade by sending a PUT request to the API.
        /// Displays a success message upon successful creation.
        /// </summary>
        public async Task CreateAsync()
        {
            var result = await SchoolGradeService.CreateAsync(_schoolGrade.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess($"{_schoolGrade.SchoolGrade} was created successfully");
                NavigationManager.NavigateTo($"/schoolgrades/update/{_schoolGrade.SchoolGradeId}");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the school classes page.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/schoolgrades");
        }
    }
}
