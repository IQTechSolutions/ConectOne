using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a vacation interval.
    /// This component allows users to input details for a vacation interval and save or cancel the operation.
    /// </summary>
    public partial class GolferPackageModal
    {
        private List<GolfCourseDto> _availableGolfCourses = [];
        private string? _golfCourseId;

        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public VacationDto? Vacation { get; set; } 

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public GolferPackageViewModel? GolferPackage { get; set; }

        /// <summary>
        /// Gets or sets the service used to manage and retrieve golf course data.
        /// </summary>
        [Inject] public IGolfCoursesService GolfCoursesService { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            GolferPackage.GolfCourse = _availableGolfCourses.FirstOrDefault(c => c.GolfCourseId == _golfCourseId);
            MudDialog.Close(GolferPackage);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (GolferPackage == null)
            {
                GolferPackage = new GolferPackageViewModel() { VacationId = Vacation.VacationId};
            }

            _golfCourseId = string.IsNullOrEmpty(GolferPackage.GolfCourse?.GolfCourseId) ? "" : GolferPackage.GolfCourse.GolfCourseId;

            var golfCoursesResult = await GolfCoursesService.PagedGolfCoursesAsync(new RequestParameters(){ PageSize = 100 });
            if (golfCoursesResult.Succeeded)
                _availableGolfCourses = golfCoursesResult.Data;

            await base.OnInitializedAsync();
        }
    }
}
