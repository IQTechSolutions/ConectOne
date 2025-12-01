using GroupingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents a component for displaying a list of school events.
    /// </summary>
    public partial class List
    {
        private CategoryDto? _category;
        private readonly SchoolEventPageParameters _args = new();
        private bool _loaded;
        private List<SchoolEventDto> _eventList = new();

        /// <summary>
        /// Navigation manager for navigating between pages.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Navigation manager for navigating between pages.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage activity group categories.
        /// </summary>
        [Inject] public IActivityGroupCategoryService ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables programmatic navigation and access to the current URI.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Navigation manager for navigating between pages.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }
        
        /// <summary>
        /// Gets or sets the navigation manager for navigating between pages.
        /// </summary>
        private string PageTitle => string.IsNullOrEmpty(CategoryId) ? "Events" : $"{_category!.Name} Events";

        /// <summary>
        /// Navigates to the event details page.
        /// </summary>
        /// <param name="eventId">The ID of the event to navigate to.</param>
        public void NavigateToEventDetails(string eventId)
        {
            NavigationManager.NavigateTo($"/events/{eventId}");
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
                // Get the authentication state
                var authState = await AuthenticationStateTask;
                var user = authState.User;

                // Set parameters based on user roles
                if (user.IsInRole(RoleConstants.Parent) && !user.IsInRole(RoleConstants.SuperUser))
                {
                    //_args.ParentId = user.GetUserId();
                    //_args.UserEmailAddress = user.GetUserEmail();
                }

                if (user.IsInRole(RoleConstants.Learner) && !user.IsInRole(RoleConstants.SuperUser))
                {
                    _args.LearnerId = user.GetUserId();
                    _args.UserEmailAddress = user.GetUserEmail();
                }

                if (user.IsInRole(RoleConstants.Teacher) && !user.IsInRole(RoleConstants.SuperUser))
                {
                    //_args.TeacherId = user.GetUserId();
                    //_args.UserEmailAddress = user.GetUserEmail();
                }

                // Fetch the category details if CategoryId is provided
                if (!string.IsNullOrEmpty(CategoryId))
                {
                    var categoryResult = await ActivityGroupCategoryService.CategoryAsync(CategoryId);
                    if (categoryResult.Succeeded)
                    {
                        _category = categoryResult.Data;
                        _args.CategoryId = CategoryId;
                    }
                }

                // Fetch the list of events
                var eventListResult = await SchoolEventQueryService.PagedSchoolEventsAsync(_args);

                if (eventListResult.Succeeded)
                {
                    _eventList = eventListResult.Data;
                }

                // Mark the component as loaded
                _loaded = true;

                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
