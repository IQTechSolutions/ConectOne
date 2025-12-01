using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.SchoolClasses
{
    /// <summary>
    /// The Index component is responsible for displaying the details of a specific school grade
    /// and navigating to the learners in a selected school class.
    /// </summary>
    public partial class Index
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically injected by the Blazor
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the grade to be displayed.
        /// </summary>
        [Parameter] public string GradeId { get; set; } = null!;

        /// <summary>
        /// The school grade details.
        /// </summary>
        private SchoolGradeDto _schoolGrade = null!;

        /// <summary>
        /// Navigates to the learners page for the specified class ID.
        /// </summary>
        /// <param name="classId">The ID of the class.</param>
        private void NavigateToLearners(string classId)
        {
            NavigationManager.NavigateTo($"/schoolclasses/{classId}/learners");
        }

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var gradeResult = await Provider.GetAsync<SchoolGradeDto>($"schoolgrades/{GradeId}");
                if (gradeResult.Succeeded)
                {
                    _schoolGrade = gradeResult.Data;
                }
            }
        }
    }
}
