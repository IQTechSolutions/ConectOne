
using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.SchoolClasses
{
    /// <summary>
    /// The ClassLearners component is responsible for displaying the learners in a specific school class.
    /// It fetches the class details and the list of learners from the server.
    /// </summary>
    public partial class ClassLearners
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. This property is typically set by the Blazor
        /// framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the class for which learners are being displayed.
        /// </summary>
        [Parameter] public string ClassId { get; set; } = null!;

        /// <summary>
        /// The school class details.
        /// </summary>
        private SchoolClassDto _schoolClass = null!;

        /// <summary>
        /// Collection of learners in the school class.
        /// </summary>
        private ICollection<LearnerDto> _learners = [];

        /// <summary>
        /// Navigates to the learner details page for the specified learner ID.
        /// </summary>
        /// <param name="learnerId">The ID of the learner.</param>
        public void NavigateToLearnerDetails(string learnerId)
        {
            NavigationManager.NavigateTo($"/learners/{learnerId}");
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
                var schoolClassResult = await SchoolClassService.SchoolClassAsync(ClassId);
                if (schoolClassResult.Succeeded)
                {
                    _schoolClass = schoolClassResult.Data;

                    var learnersResult = await LearnerQueryService.PagedLearnersAsync(new LearnerPageParameters() { SchoolClassId = ClassId });
                    if (learnersResult.Succeeded)
                    {
                        _learners = learnersResult.Data;
                    }
                }
                StateHasChanged();
            }
        }
    }
}
