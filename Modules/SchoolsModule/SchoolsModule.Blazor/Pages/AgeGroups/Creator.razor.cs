using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Pages.AgeGroups
{
    /// <summary>
    /// The Creator component is responsible for creating new age groups.
    /// It provides a form for inputting age group details and handles the submission process.
    /// </summary>
    public partial class Creator
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IAgeGroupService AgeGroupService { get; set; } = null!;

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
            new("Age Groups", href: "/agegroups", icon: Icons.Material.Filled.People),
            new("Create Age Group", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// Source URL for the cover image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// ViewModel for the age group being created.
        /// </summary>
        private readonly AgeGroupViewModel _ageGroup = new();

        /// <summary>
        /// Updates the cover image source when the image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Creates a new age group by sending a PUT request to the API.
        /// Displays a success message upon successful creation and navigates to the age groups page.
        /// </summary>
        public async Task CreateAsync()
        {
            if (_ageGroup.MinAge > _ageGroup.MaxAge)
            {
                SnackBar.AddError("Min age has to be smaller than max age");
            }
            else
            {
                var result = await AgeGroupService.CreateAsync(_ageGroup.ToDto());
                result.ProcessResponseForDisplay(SnackBar, () =>
                {
                    SnackBar.AddSuccess($"{_ageGroup.Name} was created successfully");
                    NavigationManager.NavigateTo("agegroups");
                });
            }
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the age groups page.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/agegroups");
        }
    }
}
