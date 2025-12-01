using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.SchoolGrades
{
    /// <summary>
    /// Represents a component for displaying a list of school grades on a mobile device.
    /// </summary>
    public partial class SchoolGradeMobileList
    {
        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback that is invoked when a school grade is selected.
        /// </summary>
        /// <remarks>The callback is triggered whenever a school grade is selected, passing the selected
        /// grade as a parameter. This can be used to handle grade selection events in a parent component.</remarks>
        [Parameter] public EventCallback<string> SchoolGradeSelected { get; set; }

        /// <summary>
        /// Represents a collection of school grade data transfer objects.
        /// </summary>
        /// <remarks>This collection is used to store and manage instances of <see
        /// cref="SchoolGradeDto"/>.  It is initialized as an empty list by default.</remarks>
        private ICollection<SchoolGradeDto> _schoolGrades = new List<SchoolGradeDto>();

        /// <summary>
        /// Invokes the SchoolGradeSelected event callback when a school grade is selected.
        /// </summary>
        /// <param name="gradeId">The ID of the selected school grade.</param>
        private void OnSchoolGradeSelected(string gradeId)
        {
            SchoolGradeSelected.InvokeAsync(gradeId);
        }

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var schoolGradeResult = await SchoolGradeService.PagedSchoolGradesAsync(new SchoolGradePageParameters() { PageSize = 100 });

                // If the request succeeded, update the list of school grades
                if (schoolGradeResult.Succeeded)
                {
                    _schoolGrades = schoolGradeResult.Data;
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}