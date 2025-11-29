using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Teachers
{
    /// <summary>
    /// The TeachersSelectionDropdownBox component is responsible for displaying a dropdown box
    /// for selecting a teacher. It loads the list of teachers from the server and handles the selection process.
    /// </summary>
    public partial class TeachersSelectionDropdownBox : ComponentBase
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
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

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// Collection of teacher view models.
        /// </summary>
        private IEnumerable<TeacherViewModel> _teachers = null!;

        /// <summary>
        /// The ID of the parent entity, if any.
        /// </summary>
        [Parameter] public string ParentId { get; set; } = null!;

        /// <summary>
        /// The selected teacher.
        /// </summary>
        [Parameter] public TeacherViewModel Teacher { get; set; } = null!;

        /// <summary>
        /// Event callback for when the selected teacher changes.
        /// </summary>
        [Parameter] public EventCallback<TeacherViewModel> TeacherChanged { get; set; }

        /// <summary>
        /// Handles the change in selected teacher value.
        /// </summary>
        /// <param name="value">The new selected teacher value.</param>
        public async Task SelectedValueChanged(TeacherViewModel value)
        {
            Teacher = value;
            await TeacherChanged.InvokeAsync(value);
        }

        /// <summary>
        /// The variant of the dropdown box.
        /// </summary>
        [Parameter] public Variant Variant { get; set; } = Variant.Text;

        /// <summary>
        /// Converter function for displaying teacher names.
        /// </summary>
        private readonly Func<TeacherViewModel, string> teacherConverter = p => p?.FirstName != null ? $"{p?.FirstName} {p?.LastName}" : "";

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the list of teachers from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var amenityResult = await TeacherService.PagedTeachersAsync(new TeacherPageParameters { PageSize = 100 });
            if (amenityResult.Succeeded)
            {
                _teachers = amenityResult.Data.Select(c => new TeacherViewModel(c));

                await base.OnInitializedAsync();
                _loaded = true;
            }
            else
            {
                foreach (var error in amenityResult.Messages)
                {
                    SnackBar.Add(error, Severity.Error);
                }
            }
        }
    }
}

