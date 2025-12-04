using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using NeuralTech.Base.Enums;
using System.Security.Claims;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;

namespace SchoolsEnterprise.Blazor.Shared.Layout
{
    /// <summary>
    /// A Blazor NavMenu component that displays a side or top navigation menu.
    /// Provides navigation options to different parts of the application
    /// and handles user identity through <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public partial class NavMenu
    {
        private bool _canViewMessages;
        private bool _canViewChats;
        private bool _canViewCalendar;
        private bool _canViewEvents;
        private bool _canViewLearners;
        private bool _canViewAgeGroups;
        private bool _canViewschoolGrades;
        private bool _canViewDisciplinaryActions;
        private bool _canViewDiciplanarySeverityScale;
        private bool _canViewParents;
        private bool _canViewStaff;
        private bool _canViewActivities;
        private bool _canViewActivityCategories;
        private bool _canViewBlogEntries;
        private bool _canViewBlogEntryCategories;
        private bool _canCreateBlogEntry;
        private bool _canViewProductCatgories;
        private bool _canViewProducts;
        private bool _canViewServices;
        private bool _canViewAdvertisements;
        private bool _canViewAdvertisementReviews;
        private bool _canViewAdvertisementTiers;
        private bool _canViewBusinessListings;
        private bool _canViewBusinessListingReviews;
        private bool _canViewBusinessListingTiers;
        private bool _canViewBusinessListingCategories;
        private bool _canViewUserRples;
        private bool _canViewUsers;
        private bool _canViewAffiliates;
        private ClaimsPrincipal _user = null!;
        private List<RoleDto> _shopOwners = new();
        private string _userId = null!;

        #region Cascading Parameters and Injected Services

        /// <summary>
        /// The authentication state for the current user, cascaded from a higher-level component
        /// (e.g., MainLayout). Used here to fetch and store user information when the component initializes.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] private IDialogService DialogService { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be
        /// manually set in most cases.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage user roles within the application.
        /// </summary>
        [Inject] public IRoleService RoleService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the text displayed for the "Connects" label.
        /// </summary>
        [Parameter] public string ConnectsText { get; set; } = "Connects";

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether the user has permission to view any advertising-related menu options.
        /// </summary>
        private bool CanViewAdvertisingMenu => _canViewAdvertisements || _canViewAdvertisementReviews || _canViewAdvertisementTiers;

        /// <summary>
        /// Gets a value indicating whether the user has permission to view any part of the business listing menu.
        /// </summary>
        /// <remarks>This property returns <see langword="true"/> if the user can view business listings,
        /// reviews, tiers, or categories. Use this property to determine whether to display the business listing menu
        /// in the user interface.</remarks>
        private bool CanViewBusinessListingMenu => _canViewBusinessListings || _canViewBusinessListingReviews || _canViewBusinessListingTiers || _canViewBusinessListingCategories;

        /// <summary>
        /// Navigates to a page listing all activity categories within the application.
        /// </summary>
        private void NavigateToAllCategories()
        {
            // Forces browser navigation to the specified route.
            NavigationManager.NavigateTo("/activitycategories", forceLoad: true);
        }

        /// <summary>
        /// Navigates to a page for importing new learners, presumably from an external file or source.
        /// </summary>
        private async Task ImportNewLearners()
        {
            // Navigates to the "imported-learners" route.
            // The 'true' parameter forces a full page load instead of a client-side routing transition.
            NavigationManager.NavigateTo("imported-learners", true);
        }

        /// <summary>
        /// Navigates to the global messages page, showing messages of type <see cref="MessageType.Global"/>.
        /// </summary>
        private void NavigateToGlobalMessages()
        {
            // Converts MessageType.Global to an integer and passes it as part of the route.
            NavigationManager.NavigateTo($"/messages/bytype/{(int)MessageType.Global}", true);
        }

        /// <summary>
        /// Sets the permissions for the current user based on their authentication state and authorization rules.
        /// </summary>
        /// <remarks>This method evaluates the user's permissions for various modules and features by
        /// performing asynchronous authorization checks. The results of these checks determine whether the user has
        /// access to specific functionalities, such as viewing messages, chats, calendars, and other domain-specific
        /// resources.</remarks>
        /// <param name="authState">The authentication state of the current user, which includes user information and claims.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task SetPermissions(AuthenticationState authState)
        {
            _canViewMessages = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.MessagePermissions.View)).Succeeded;
            _canViewChats = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.ChatPermissions.View)).Succeeded;
            _canViewCalendar = (await AuthorizationService.AuthorizeAsync(authState.User, CalendarModule.Domain.Constants.Permissions.CalendarPermissions.View)).Succeeded;

            _canViewEvents = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.View)).Succeeded;

            _canViewLearners = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.LearnerPermissions.View)).Succeeded;
            _canViewAgeGroups = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.SystemDataPermissions.View)).Succeeded;
            _canViewschoolGrades = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.SystemDataPermissions.View)).Succeeded;
            _canViewDisciplinaryActions = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.SystemDataPermissions.View)).Succeeded;
            _canViewDiciplanarySeverityScale = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.SystemDataPermissions.View)).Succeeded;

            _canViewParents = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.ParentPermissions.View)).Succeeded;
            _canViewStaff = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.TeacherPermissions.View)).Succeeded;
            _canViewActivities = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.ActivityPermissions.View)).Succeeded;
            _canViewActivityCategories = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.ActivityCategoryPermissions.View)).Succeeded;
            _canViewBlogEntries = (await AuthorizationService.AuthorizeAsync(authState.User, BloggingModule.Domain.Constants.Permissions.BlogPermissions.View)).Succeeded;
            _canViewBlogEntryCategories = (await AuthorizationService.AuthorizeAsync(authState.User, BloggingModule.Domain.Constants.Permissions.BlogCategoryPermissions.View)).Succeeded;
            _canCreateBlogEntry = (await AuthorizationService.AuthorizeAsync(authState.User, BloggingModule.Domain.Constants.Permissions.BlogPermissions.Create)).Succeeded;

            _canViewUserRples = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Roles.View)).Succeeded;
            _canViewUsers = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.View)).Succeeded;
            _canViewAffiliates = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.AffiliatePermissions.View)).Succeeded;

            _canViewProductCatgories = (await AuthorizationService.AuthorizeAsync(authState.User, ProductsModule.Domain.Constants.Permissions.ProductCategories.View)).Succeeded
                                       && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");
            _canViewProducts = (await AuthorizationService.AuthorizeAsync(authState.User, ProductsModule.Domain.Constants.Permissions.Products.View)).Succeeded
                               && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");
            _canViewServices = (await AuthorizationService.AuthorizeAsync(authState.User, ProductsModule.Domain.Constants.Permissions.Services.View)).Succeeded
                               && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");

            _canViewAdvertisements = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.AdvertisementPermissions.View)).Succeeded
                                     && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("SellAdvertisements");
            _canViewAdvertisementReviews = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.AdvertisementReviewPermissions.View)).Succeeded
                                           && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("SellAdvertisements");
            _canViewAdvertisementTiers = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.AdvertisementTierPermissions.View)).Succeeded
                                         && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("SellAdvertisements");
            _canViewBusinessListings = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.BusinessListingPermissions.View)).Succeeded
                                       && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("SellAdvertisements");

            _canViewBusinessListingReviews = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.BusinessReviewPermissions.View)).Succeeded
                                             && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");
            _canViewBusinessListingTiers = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.BusinessReviewPermissions.View)).Succeeded
                                           && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");
            _canViewBusinessListingCategories = (await AuthorizationService.AuthorizeAsync(authState.User, BusinessModule.Domain.Constants.Permissions.BusinessCategoryPermissions.View)).Succeeded
                                                && Configuration.GetSection("ApplicationConfiguration").GetValue<bool>("UseEcommerceModule");
        }

        #endregion

        #region LifeCycle Methods

        /// <summary>
        /// Lifecycle method that runs after the component is initialized.
        /// Fetches user details (e.g., userId) from the authentication state.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Retrieve the authentication state, including user claims.
            var authState = await AuthenticationStateTask;

            // Extract user and userId from claims (e.g., the NameIdentifier claim).
            _user = authState.User;
            _userId = _user.GetUserId();

            await SetPermissions(authState);

            var productManagers = await RoleService.ProductManagers();
            if (productManagers.Succeeded && productManagers.Data.Any())
            {
                foreach (var manager in productManagers.Data)
                {
                    if (manager.ChildRoles.Any())
                    {
                        _shopOwners.AddRange(manager.ChildRoles);
                    }
                    else
                    {
                        _shopOwners.Add(manager);
                    }
                }
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
