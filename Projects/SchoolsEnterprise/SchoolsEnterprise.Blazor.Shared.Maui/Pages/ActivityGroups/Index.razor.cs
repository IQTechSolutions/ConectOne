using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.ActivityGroups
{
    /// <summary>
    /// Displays information about a specific <see cref="ActivityGroupDto"/>,
    /// including its team members (learners). Fetches data via injected
    /// <see cref="IActivityGroupProvider"/> and <see cref="ILearnersProvider"/>.
    /// </summary>
    public partial class Index
    {
        #region Injected Services

        /// <summary>
        /// A general HTTP provider used to submit or retrieve data to/from the server 
        /// (e.g., GET, POST requests for the parent entity).
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Assign a custom
        /// implementation of <see cref="ILearnerQueryService"/> to customize how learner data is retrieved.</remarks>
        [Inject] public ILearnerQueryService LearnersProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access details about the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Component Parameters

        /// <summary>
        /// The unique identifier of the activity group. This is passed from the parent component
        /// or via routing parameters. Used to fetch group details from the server.
        /// </summary>
        [Parameter] public string ActivityGroupId { get; set; } = null!;

        #endregion

        #region Private Fields and Properties

        /// <summary>
        /// The activity group details fetched from the server. Initially null,
        /// updated in <see cref="OnInitializedAsync"/>.
        /// </summary>
        private ActivityGroupDto ActivityGroup { get; set; } = null!;

        /// <summary>
        /// A collection of learners (team members) belonging to the specified activity group,
        /// fetched from the server using <see cref="ILearnersProvider"/>.
        /// </summary>
        public ICollection<LearnerDto> TeamMembers { get; set; } = [];

        /// <summary>
        /// Indicates whether the component has finished fetching initial data.
        /// Useful for showing a loading spinner or placeholder UI until data is ready.
        /// </summary>
        private bool _loaded;

        #endregion

        #region Component Lifecycle
        
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
                // 1. Attempt to fetch the activity group by ID.
                var activityGroupResult = await ActivityGroupQueryService.ActivityGroupAsync(ActivityGroupId);

                // 2. If successful, set the local ActivityGroup property.
                if (activityGroupResult.Succeeded)
                {
                    ActivityGroup = activityGroupResult.Data;

                    // 3. Fetch paginated learners for the group, with an upper limit of 100.
                    var teamsResult = await LearnersProvider.PagedLearnersAsync(new LearnerPageParameters { ActivityGroupId = ActivityGroupId, PageSize = 100 });

                    // 4. If successful, store the fetched learners in TeamMembers.
                    if (teamsResult.Succeeded)
                    {
                        TeamMembers = teamsResult.Data;
                    }
                }
                _loaded = true;

                StateHasChanged();
            }
        }

        #endregion

        #region Navigation Methods

        /// <summary>
        /// Navigates the user to a detailed page for a specific learner. Typically used
        /// when the user clicks on a learner item in the UI to view more information about them.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to view in detail.</param>
        public void NavigateToLearnerDetails(string learnerId)
        {
            // The route "/learners/{learnerId}" should be handled by a LearnerDetails component/page.
            NavigationManager.NavigateTo($"/learners/{learnerId}");
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// A sample nested class that represents a simple menu item, used as a demonstration
        /// of how this component might display or convert an object. The real usage is not shown here,
        /// but it illustrates how you can place small classes inside the same file.
        /// </summary>
        public class Menu
        {
            /// <summary>
            /// The display name of the menu item.
            /// </summary>
            public string Name { get; set; } = null!;
        }
        /// <summary>
        /// Demonstration of a second nested class that might be used to store
        /// product or item details, for example in a list or table. Unrelated
        /// to the main activity group logic, but shows how multiple classes
        /// can live in one file.
        /// </summary>
        public class ProductItemInfo
        {
            /// <summary>
            /// Unique identifier for the item (e.g., for database keys).
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// A URL or resource path to the item's image.
            /// </summary>
            public string Image { get; set; } = null!;

            /// <summary>
            /// The display name or title of the item.
            /// </summary>
            public string ItemName { get; set; } = null!;

            /// <summary>
            /// A more detailed textual description of the item.
            /// </summary>
            public string Descriptions { get; set; } = null!;

            /// <summary>
            /// The current price of the item, stored as a string for UI display.
            /// </summary>
            public string Price { get; set; } = null!;

            /// <summary>
            /// A text representing any discount or promotion (e.g., "-20%" or a special code).
            /// </summary>
            public string Discount { get; set; } = null!;
        }

        #endregion
    }
}
