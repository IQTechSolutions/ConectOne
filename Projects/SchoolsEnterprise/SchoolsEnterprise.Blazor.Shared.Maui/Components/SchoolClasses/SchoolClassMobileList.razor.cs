using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.SchoolClasses
{
    /// <summary>
    /// The SchoolClassMobileList component is responsible for displaying a list of school classes
    /// for a specific grade. It allows the user to select a school class and triggers an event callback
    /// when a class is selected.
    /// </summary>
    public partial class SchoolClassMobileList
    {
        /// <summary>
        /// Gets or sets the service used to manage school class data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// The ID of the grade for which school classes are being displayed.
        /// </summary>
        [Parameter] public string GradeId { get; set; } = null!;

        /// <summary>
        /// Event callback for when a school class is selected.
        /// </summary>
        [Parameter] public EventCallback<string> SchoolClassSelected { get; set; }

        /// <summary>
        /// Collection of school class DTOs.
        /// </summary>
        private ICollection<SchoolClassDto> _schoolClasses = [];

        /// <summary>
        /// Handles the selection of a school class and triggers the event callback.
        /// </summary>
        /// <param name="classId">The ID of the selected school class.</param>
        private async Task OnSchoolClassSelected(string classId)
        {
            await SchoolClassSelected.InvokeAsync(classId);
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
               var schoolClassResult = await SchoolClassService.PagedSchoolClassesAsync(new SchoolClassPageParameters() { GradeId = GradeId, PageSize = 100 });

                if (schoolClassResult.Succeeded)
                {
                    _schoolClasses = schoolClassResult.Data;
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
