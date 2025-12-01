using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Reporting
{
    /// <summary>
    /// The LearnerReports component is responsible for displaying reports for a specific learner.
    /// It fetches the learner details from the server and displays them.
    /// </summary>
    public partial class LearnerReports
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// The ID of the learner for whom reports are being displayed.
        /// </summary>
        [Parameter] public string LearnerId { get; set; } = null!;

        /// <summary>
        /// The learner details.
        /// </summary>
        private LearnerDto _learner = null!;

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
                var learnerResult = await Provider.GetAsync<LearnerDto>($"learners/{LearnerId}");
                if (learnerResult.Succeeded)
                {
                    _learner = learnerResult.Data;
                }
                StateHasChanged();
            }
        }
    }
}