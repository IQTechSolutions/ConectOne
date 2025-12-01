using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents a Blazor component that displays a menu of event tiles.
    /// The component fetches event categories dynamically and renders icons based on the category type.
    /// </summary>
    public partial class EventTileMenu
    {
        private readonly CategoryPageParameters _args = new(); 
        private string _userId = null!;                        
        private List<CategoryDto> _categories = new();        
        private bool _loaded;                                  

        /// <summary>
        /// Cascading parameter to get the current user's authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making secure API calls.
        /// </summary>
        [Inject] public IActivityGroupCategoryService ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation and respond to URI changes.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected Logger for logging errors and information.
        /// </summary>
        [Inject] public ILogger<Home> Logger { get; set; } = null!;

        /// <summary>
        /// Allows child content to be rendered inside this component.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;
        
        /// <summary>
        /// Gets the appropriate icon URL for a given category name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>A string representing the URL of the icon associated with the category.</returns>
        private string GetCategoryIcon(string name)
        {
            // Determine the icon URL based on category name keywords
            if (name.ToLower().Contains("sport"))
                return "/images/static/icons2/sporticon.svg";
            if (name.ToLower().Contains("academic"))
                return "/images/static/icons2/academicicon.svg";
            if (name.ToLower().Contains("cultural"))
                return "/images/static/icons2/cultureicon.svg";
            if (name.ToLower().Contains("rugby"))
                return "/images/static/icons2/rugby.svg";
            if (name.ToLower().Contains("cricket"))
                return "/images/static/icons2/cricketicon.svg";
            if (name.ToLower().Contains("hockey"))
                return "/images/static/icons2/hockeyicon.svg";
            if (name.ToLower().Contains("tennis"))
                return "/images/static/icons2/tennisicon.svg";
            if (name.ToLower().Contains("netbal"))
                return "/images/static/icons2/netbalicon.svg";
            if (name.ToLower().Contains("athletics"))
                return "/images/static/icons2/athleticsicon.svg";
            if (name.ToLower().Contains("chess"))
                return "/images/static/icons2/chessicon.svg";
            if (name.ToLower().Contains("swimming"))
                return "/images/static/icons2/swimmingicon.svg";
            if (name.ToLower().Contains("cross country"))
                return "/images/static/icons2/crosscountryicon.svg";
            if (name.ToLower().Contains("choir"))
                return "/images/static/icons2/Choir1.svg";
            if (name.ToLower().Contains("debat"))
                return "/images/static/icons2/talkicon.svg";
            if (name.ToLower().Contains("music"))
                return "/images/static/icons2/musicicon.svg";
            return "/images/static/icons2/homeicon.svg"; // Default icon
        }

        /// <summary>
        /// Navigates to the event categories page for a given category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to navigate to.</param>
        public void NavigateToEventCategories(string categoryId)
        {
            // Use NavigationManager to redirect the user to the event categories page
            NavigationManager.NavigateTo($"/eventCategories/{categoryId}");
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
                try
                {
                    // Get the current user's authentication state
                    var authState = await AuthenticationStateTask;
                    var user = authState.User;

                    // Retrieve the user ID from claims
                    _userId = authState.User.GetUserId();

                    var categoryListResult = await ActivityGroupCategoryService.PagedCategoriesAsync(_args);

                    if (!categoryListResult.Succeeded) return;

                    // Populate the category list with the fetched data
                    foreach (var category in categoryListResult.Data)
                    {
                        _categories.Add(category);
                    }

                    // Set the loaded flag to true and refresh the UI
                    _loaded = true;
                    StateHasChanged();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "An error occurred while fetching featured category count.");
                    SnackBar.Add("An error occurred while fetching featured category count.");
                }

                
            }
        }
    }
}