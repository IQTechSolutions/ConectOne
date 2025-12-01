using ConectOne.Domain.Interfaces;
using GroupingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents a component for displaying categories and their subcategories.
    /// </summary>
    public partial class Categories
    {
        private List<CategoryDto> _subCategories = new();
        private CategoryDto _category = null!;
        private bool _loaded;

        /// <summary>
        /// The navigation manager for handling navigation within the application.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// The navigation manager for handling navigation within the application.
        /// </summary>
        [Inject] public IActivityGroupCategoryService CategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage school event categories.
        /// </summary>
        [Inject] public ISchoolEventCategoryService SchoolEventCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Parameter to get the category ID from the parent component or page
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;
        
        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        private void NavigateToPage(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }

        /// <summary>
        /// Gets the icon URL for a given category name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The URL of the icon.</returns>
        private string GetCategoryIcon(string name)
        {
            if (name.ToLower().Contains("sport"))
                return "/images/static/icons2/sporticon.svg";
            if (name.ToLower().Contains("academic"))
                return "/images/static/icons2/academicicon.svg";
            if (name.ToLower().Contains("culture"))
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
                var authState = await AuthenticationStateTask;
                var _userId = authState.User.GetUserId();
                var _email = authState.User.GetUserEmail();

                // Fetch the category details
                var categoryResult = await CategoryService.CategoryAsync(CategoryId);
                if (categoryResult.Succeeded)
                {
                    _category = categoryResult.Data;
                }

                // Fetch the subcategories
                var subCategoriesResult = await SchoolEventCategoryService.PagedSchoolEventActivityGroupCategoriesForAppAsync(new SchoolEventPageParameters() {  CategoryId = _category.CategoryId, UserEmailAddress = _email});
                if (subCategoriesResult.Succeeded)
                {
                    _subCategories = subCategoriesResult.Data;
                }

                // Mark the component as loaded
                _loaded = true;

                StateHasChanged();
            }
        }
    }
}
