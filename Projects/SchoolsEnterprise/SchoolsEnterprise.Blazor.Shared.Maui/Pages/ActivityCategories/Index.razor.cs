using ConectOne.Blazor.Extensions;
using GroupingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.ActivityCategories
{
    /// <summary>
    /// The Index component is responsible for displaying a list of activity categories and their associated activity groups.
    /// It provides navigation to different categories, groups, events, communication center, and settings.
    /// </summary>
    public partial class Index
    {
        private bool _loaded;
        private CategoryDto _category = null!;
        private List<CategoryDto> _categories = new();
        private List<ActivityGroupDto> _teams = new();

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate within the application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage activity group categories.
        /// </summary>
        [Inject] public IActivityGroupCategoryService ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query activity groups.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;
        
        /// <summary>
        /// Provides localized strings for the activity categories page.
        /// </summary>
        [Inject] public IStringLocalizer<Index> Localizer { get; set; } = null!;

        /// <summary>
        /// The ID of the category to be displayed.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

        /// <summary>
        /// Navigates to the specified activity category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to navigate to.</param>
        public void NavigateToActivityCategory(string categoryId)
        {
            NavigationManager.NavigateTo($"/activityCategories/{categoryId}", true);
        }

        /// <summary>
        /// Navigates to the specified activity group.
        /// </summary>
        /// <param name="groupId">The ID of the group to navigate to.</param>
        public void NavigateToActivityGroup(string groupId)
        {
            NavigationManager.NavigateTo($"/activityGroups/{groupId}");
        }

        /// <summary>
        /// Navigates to the events page.
        /// </summary>
        public void NavigateToEvents()
        {
            NavigationManager.NavigateTo($"/events", true);
        }

        /// <summary>
        /// Navigates to the communication center page.
        /// </summary>
        public void NavigateToCommsCenter()
        {
            NavigationManager.NavigateTo($"/commscenter", true);
        }

        /// <summary>
        /// Navigates to the settings page.
        /// </summary>
        public void NavigateToSettings()
        {
            NavigationManager.NavigateTo($"/settings", true);
        }

        /// <summary>
        /// Gets the icon URL for the specified category name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The URL of the icon for the category.</returns>
        private string GetCategoryIcon(string name)
        {
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
            if (name.ToLower().Contains("mountain"))
                return "/images/static/icons2/musicicon.svg";
            return "/images/static/icons2/homeicon.svg";
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
                if (string.IsNullOrEmpty(CategoryId))
                {
                    // Load all categories if no specific category ID is provided
                    var result = await ActivityGroupCategoryService.CategoriesAsync();
                    if (result.Succeeded)
                    {
                        _categories = result.Data.ToList();
                        StateHasChanged();
                    }
                }
                else
                {
                    // Load the specific category and its subcategories and activity groups
                    var result = await ActivityGroupCategoryService.CategoryAsync(CategoryId);

                    async void SuccessAction()
                    {
                        _category = result.Data;

                        if (_category.HasSubCategories)
                        {
                            var categoriesResult = await ActivityGroupCategoryService.CategoriesAsync(); 

                            if (categoriesResult.Succeeded)
                            {
                                _categories = categoriesResult.Data.ToList();
                                StateHasChanged();
                            }
                        }

                        if (_category.HasEntities)
                        {
                            var pageParams = new ActivityGroupPageParameters() { CategoryIds = _category.CategoryId, PageSize = 100 };
                            var teamsResult = await ActivityGroupQueryService.PagedActivityGroupsAsync(pageParams);

                            if (teamsResult.Succeeded)
                            {
                                _teams = teamsResult.Data.ToList();
                                StateHasChanged();
                            }
                        }
                    }

                    result.ProcessResponseForDisplay(SnackBar, SuccessAction);
                }
                _loaded = true;
                StateHasChanged();
            }
        }
    }
}
