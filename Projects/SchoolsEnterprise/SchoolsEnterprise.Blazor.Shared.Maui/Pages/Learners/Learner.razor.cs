using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Learners
{
    /// <summary>
    /// Represents a page or view component responsible for displaying 
    /// detailed information about a specific learner. It uses an 
    /// <see cref="ILearnersProvider"/> to fetch learner details from the server.
    /// </summary>
    public partial class Learner
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use this service to
        /// show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The unique identifier of the learner to display. 
        /// This parameter is typically passed in via a route, 
        /// e.g., <c>/learners/{LearnerId}</c>.
        /// </summary>
        [Parameter] public string LearnerId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// Holds the retrieved learner data. 
        /// Once loaded, the UI displays this information (e.g., name, grade, etc.).
        /// </summary>
        private LearnerDto _learner = null!;

        /// <summary>
        /// Indicates whether the component has finished initializing. 
        /// Used, for example, to control the display of loading indicators.
        /// </summary>
        private bool _loaded;

        #endregion

        #region Lifecycle Methods

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
                // Fetch the learner data from the server using the Learner's ID.
                var learnerResult = await LearnerQueryService.LearnerAsync(LearnerId);

                // Process the server response, showing any error or success notifications. 
                // If successful, store the data in _learner for UI display.
                learnerResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _learner = learnerResult.Data;
                });

                // Continue with standard base-class initialization.
                await base.OnInitializedAsync();

                // Indicate that the loading process has completed.
                _loaded = true;

                StateHasChanged();
            }
        }

        #endregion
    }
}
