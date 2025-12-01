using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Learners
{
    /// <summary>
    /// Represents a component for displaying medical information of a learner.
    /// </summary>
    public partial class MedicalInformation
    {
        private LearnerDto _learner = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// be set before making any HTTP requests. Ensure that a valid implementation is provided to avoid runtime
        /// errors.</remarks>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        [Parameter] public string LearnerId { get; set; } = null!;

        /// <summary>
        /// Gets the title for the medical information page.
        /// </summary>
        private string Title => $"{_learner.FirstName} {_learner.LastName} Medical Info";

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