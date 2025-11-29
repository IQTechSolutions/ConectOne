using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for creating a new learner. 
    /// Allows the user to input essential information such as name, grade, class, 
    /// cover image, and ID number. 
    /// On submission, returns a <see cref="LearnerViewModel"/> with the user's inputs 
    /// to the calling context.
    /// 
    /// <para>
    /// The modal fetches a list of available grades and classes from the server, 
    /// enabling dynamic dropdowns for user selection. 
    /// If the user confirms creation, the modal returns the filled <see cref="LearnerViewModel"/>, 
    /// otherwise it closes without saving.
    /// </para>
    /// </summary>
    public partial class LearnerCreatorModal
    {
        private readonly Func<ParentDto, string> _parentConverter = p => p?.FirstName != null ? $"{p?.FirstName} {p?.LastName}" : "";

        #region Cascading & Injected Services

        /// <summary>
        /// A reference to the <see cref="MudDialogInstance"/> managing this modal.
        /// Provides methods for closing or canceling the dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// An abstraction for sending HTTP requests (GET, POST, PUT, DELETE) to the server.
        /// Used to fetch grade/class info and, potentially, to submit learner data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage school grade data.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        #endregion

        #region Fields & Properties

        /// <summary>
        /// Represents the local state of the learner being created.
        /// This object gathers user inputs (first name, last name, grade, class, etc.).
        /// When the user confirms creation, this model is returned to the caller.
        /// </summary>
        private readonly LearnerViewModel _learner = new();

        /// <summary>
        /// Holds parents selected during creation so that the appropriate medical aid parent can be chosen.
        /// </summary>
        private readonly List<ParentDto> _selectedParents = new();

        /// <summary>
        /// The path or URL for the learner's cover image. Defaults to a placeholder 
        /// until the user selects or uploads a new image. 
        /// Updates the learner's <c>CoverImageUrl</c> when changed.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// Contains all available grades that a learner can be assigned to. 
        /// Populated during initialization (see <see cref="OnInitializedAsync"/>).
        /// </summary>
        public ICollection<SchoolGradeDto>? AvailableGrades { get; set; } = [];

        /// <summary>
        /// Function to display a school grade in a dropdown list or other selection control. 
        /// Converts a <see cref="SchoolGradeDto"/> to its displayed name.
        /// </summary>
        private readonly Func<SchoolGradeDto?, string> _schoolGradeConverter = p => p?.SchoolGrade ?? "";

        /// <summary>
        /// Contains all available classes (within a grade) that a learner can be placed into.
        /// Updated dynamically when the user selects a grade (see <see cref="SelectedGradeChanged"/>).
        /// </summary>
        public ICollection<SchoolClassDto>? AvailableSchoolClasses { get; set; } = [];

        /// <summary>
        /// Function to display a school class in a dropdown list or other selection control.
        /// Converts a <see cref="SchoolClassDto"/> to its displayed name.
        /// </summary>
        private readonly Func<SchoolClassDto?, string> _availableSchoolConverter = p => p?.SchoolClass ?? "";

        #endregion

        #region Event Handlers & Methods

        /// <summary>
        /// Invoked when the user confirms the creation of the learner. 
        /// Closes the modal and returns the filled <see cref="LearnerViewModel"/> to the caller.
        /// </summary>
        public void CreateAsync()
        {
            // Closes the MudDialog and passes _learner as the data payload.
            MudDialog.Close(_learner);
        }

        /// <summary>
        /// Triggered when the user updates their selection for the cover image. 
        /// Syncs the new image source with both <see cref="_imageSource"/> and the learner's <c>CoverImageUrl</c>.
        /// </summary>
        /// <param name="coverImage">The new image path or base64-encoded string chosen by the user.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Called when the user selects a different grade. Fetches the corresponding classes from the server,
        /// updates <see cref="AvailableSchoolClasses"/>, and assigns the first class to the learner by default.
        /// </summary>
        /// <param name="schoolGrade">
        /// The <see cref="SchoolGradeDto"/> representing the newly selected grade.
        /// </param>
        public async Task SelectedGradeChanged(SchoolGradeDto schoolGrade)
        {
            // Only change if the user has picked a new or different grade
            if (_learner.SelectedGrade?.SchoolGradeId != schoolGrade.SchoolGradeId)
            {
                // Update the local learner's selected grade
                _learner.SelectedGrade = schoolGrade;

                // Fetch all classes for the new grade
                var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100, GradeId = schoolGrade.SchoolGradeId });

                // Update the local list of available school classes
                AvailableSchoolClasses = schoolClassResult.Data;

                // Optionally auto-select the first available class
                _learner.SelectedSchoolClass = AvailableSchoolClasses.FirstOrDefault();
            }

            StateHasChanged();
        }

        /// <summary>
        /// Invoked when the user modifies the ID number field (potentially used to recalc age or birthdate).
        /// </summary>
        /// <param name="value">The new ID number entered by the user.</param>
        public async Task IdentityNrChanged(string value)
        {
            // Future logic could handle validation, auto-calculation of age, etc.
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Closes the modal without saving changes.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        #region Lifecycle

        /// <summary>
        /// Runs when the component is first initialized. Fetches available grades and classes 
        /// from the server and populates <see cref="AvailableGrades"/> and <see cref="AvailableSchoolClasses"/>.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch all grades
            var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters { PageSize = 100 });
            AvailableGrades = schoolGradeResult.Data!.ToList();

            // Fetch all classes
            var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters { PageSize = 100 });
            AvailableSchoolClasses = schoolClassResult.Data!.ToList();

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
