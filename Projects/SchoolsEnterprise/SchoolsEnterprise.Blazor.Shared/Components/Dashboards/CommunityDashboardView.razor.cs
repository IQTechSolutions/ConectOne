using System.Security.Claims;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProductsModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Components.Dashboards;

/// <summary>
/// Represents the community dashboard view component, which provides an overview of managed roles, users, registration
/// tiers, and services for the authenticated user.
/// </summary>
/// <remarks>This component retrieves and displays metrics related to the roles managed by the authenticated user.
/// It interacts with the authentication state and HTTP provider to fetch and aggregate data about roles, users,
/// registration tiers, and services. The component is designed to be used within a Blazor application.</remarks>
public partial class CommunityDashboardView : ComponentBase
{
    private readonly Dictionary<string, ManagedRoleSummary> _managedRoleSummaries = new(StringComparer.OrdinalIgnoreCase);
    private readonly IReadOnlyList<MemberRosterEntry> _memberRoster = new List<MemberRosterEntry>
    {
        new("Aaron", "Samols", "M", "25 years", "R250", "R100 (actual amount)", "Shows up when he wants"),
        new("Timothy", "Green", "M", "5 years", "R200", "R100", string.Empty),
        new("Art", "White", "M", "10 years", "R100", "R100", "Helps with camps")
    };
    private readonly string[] _salesLabels = { "Uniform Shop", "Stationery", "Merchandise" };
    private readonly double[] _salesValues = { 45, 35, 20 };
    private readonly string[] _businessCategoryLabels = { "Retail", "Services", "Agriculture", "Other" };
    private readonly double[] _businessCategoryValues = { 40, 30, 20, 10 };
    private string _userId;

    /// <summary>
    /// Gets the estimated number of business directory clients based on the total number of displayed members.
    /// </summary>
    /// <remarks>The value is calculated as 12% of the total displayed members, rounded to the nearest
    /// integer, with a minimum of 1,000. This estimate is intended for display or reporting purposes and may not
    /// reflect the exact client count.</remarks>
    private int BusinessDirectoryClients => Math.Max((int)Math.Round(DisplayTotalMembers * 0.12), 1000);

    /// <summary>
    /// Gets the profit value for the business directory.
    /// </summary>
    private string BusinessDirectoryProfit => "R25,000";

    /// <summary>
    /// Gets the current list of members in the roster.
    /// </summary>
    private IReadOnlyList<MemberRosterEntry> MemberRoster => _memberRoster;

    /// <summary>
    /// Gets the collection of labels associated with sales data.
    /// </summary>
    private string[] SalesLabels => _salesLabels;

    /// <summary>
    /// Gets the sequence of sales values for the current instance.
    /// </summary>
    private double[]? SalesSeries => _salesValues;

    /// <summary>
    /// Gets the set of labels associated with the business category.
    /// </summary>
    private string[] BusinessCategoryLabels => _businessCategoryLabels;

    /// <summary>
    /// Gets the series of values associated with the business category, or null if no values are available.
    /// </summary>
    private double[]? BusinessCategorySeries => _businessCategoryValues;

    /// <summary>
    /// Gets the total number of managed users to display.
    /// </summary>
    private int DisplayTotalMembers => TotalManagedUserCount > 0 ? TotalManagedUserCount : 7000;

    /// <summary>
    /// Gets the estimated number of paying members to display, based on the total number of members.
    /// </summary>
    private int DisplayPayingMembers => Math.Max((int)Math.Round(DisplayTotalMembers * 0.4), 2700);

    /// <summary>
    /// Gets or sets the task that provides the current authentication state.
    /// </summary>
    [CascadingParameter] public Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    /// <summary>
    /// Gets or sets the HTTP provider used for making HTTP requests.
    /// </summary>
    /// <remarks>This property is typically injected via dependency injection and must be set before
    /// use.</remarks>
    [Inject] public IRoleService RoleService { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the user service used to manage user-related operations within the component.
    /// </summary>
    [Inject] public IUserService UserService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service used to manage and retrieve service tier information.
    /// </summary>
    [Inject] public IServiceTierService ServiceTierService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
    /// Blazor application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets a read-only collection of summaries for the managed roles.
    /// </summary>
    private IReadOnlyCollection<ManagedRoleSummary> ManagedRoleSummaries => _managedRoleSummaries.Values;

    /// <summary>
    /// Gets or sets the total number of managed roles.
    /// </summary>
    private int TotalManagedRoleCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of managed users.
    /// </summary>
    private int TotalManagedUserCount { get; set; }

    /// <summary>
    /// Gets or sets the total count of managed registration tiers.
    /// </summary>
    private int TotalManagedRegistrationTierCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of managed services.
    /// </summary>
    private int TotalManagedServiceCount { get; set; }

    /// <summary>
    /// Asynchronously initializes the component's state by retrieving and processing user roles and their associated
    /// data.
    /// </summary>
    /// <remarks>This method retrieves the current user's authentication state and processes their roles to
    /// populate managed role summaries.  It ensures that only authenticated users with valid roles are processed. For
    /// each role, additional role-related data  is fetched and aggregated, including metrics for managed roles. The
    /// method updates the total counts for managed users,  registration tiers, services, and roles based on the
    /// retrieved data.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationStateTask is null)
        {
            return;
        }

        var authState = await AuthenticationStateTask.ConfigureAwait(false);
        var user = authState.User;
        _userId = user.GetUserId();

        if (user.Identity?.IsAuthenticated is not true)
        {
            return;
        }

        var roleNames = user.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var roleName in roleNames)
        {
            var roleInfo = await RoleService.RoleAsync(Uri.EscapeDataString(roleName));
            if (roleInfo.Succeeded is not true || roleInfo.Data is null || string.IsNullOrWhiteSpace(roleInfo.Data.Id))
            {
                continue;
            }

            var managedRoles = await RoleService.ChildrenAsync(roleInfo.Data.Id);
            if (managedRoles.Succeeded is not true || managedRoles.Data is null)
            {
                continue;
            }

            foreach (var managedRole in managedRoles.Data)
            {
                if (managedRole is null || string.IsNullOrWhiteSpace(managedRole.Id) || string.IsNullOrWhiteSpace(managedRole.Name))
                {
                    continue;
                }

                if (!_managedRoleSummaries.TryGetValue(managedRole.Id, out var summary))
                {
                    summary = new ManagedRoleSummary(managedRole.Id, managedRole.Name!, managedRole.Description ?? string.Empty);
                    _managedRoleSummaries.Add(managedRole.Id, summary);
                }
                else if (string.IsNullOrWhiteSpace(summary.Description) && !string.IsNullOrWhiteSpace(managedRole.Description))
                {
                    summary.Description = managedRole.Description!;
                }

                if (!summary.HasMetricsLoaded)
                {
                    await PopulateMetricsAsync(summary).ConfigureAwait(false);
                }
            }

            TotalManagedUserCount = ManagedRoleSummaries.Sum(summary => summary.UserCount);
            TotalManagedRegistrationTierCount = ManagedRoleSummaries.Sum(summary => summary.RegistrationTierCount);
            TotalManagedServiceCount = ManagedRoleSummaries.Sum(summary => summary.ServiceCount);
            TotalManagedRoleCount = ManagedRoleSummaries.Count;
        }
    }

    /// <summary>
    /// Asynchronously populates the metrics for the specified <see cref="ManagedRoleSummary"/> instance, including user
    /// count, registration tier count, and service count.
    /// </summary>
    /// <remarks>This method retrieves user and service tier data asynchronously and updates the provided <see
    /// cref="ManagedRoleSummary"/> instance with the calculated metrics. If an error occurs during data retrieval, the
    /// metrics are set to their default values (0) to ensure consistency.</remarks>
    /// <param name="summary">The <see cref="ManagedRoleSummary"/> instance to populate with metrics. This parameter must not be <c>null</c>.</param>
    /// <returns></returns>
    private async Task PopulateMetricsAsync(ManagedRoleSummary summary)
    {
        try
        {
            var userParameters = new UserPageParameters
            {
                Role = summary.RoleName,
                PageNr = 1,
                PageSize = 1
            };

            var userTask = UserService.PagedUsers(userParameters);
            var tierTask = ServiceTierService.AllEntityServiceTiersAsync(summary.RoleId);

            await Task.WhenAll(userTask, tierTask).ConfigureAwait(false);

            var userResult = await userTask;
            summary.UserCount = userResult?.TotalCount ?? 0;

            var tierResult = await tierTask.ConfigureAwait(false);
            if (tierResult.Succeeded && tierResult.Data is not null)
            {
                var tiers = tierResult.Data.ToList();
                summary.RegistrationTierCount = tiers.Count;
                summary.ServiceCount = tiers.Sum(tier => tier.Services?.Count ?? 0);
            }
        }
        catch
        {
            summary.UserCount = Math.Max(summary.UserCount, 0);
            summary.RegistrationTierCount = Math.Max(summary.RegistrationTierCount, 0);
            summary.ServiceCount = Math.Max(summary.ServiceCount, 0);
        }
        finally
        {
            summary.HasMetricsLoaded = true;
        }
    }

    /// <summary>
    /// Represents a summary of a managed role, including its identifier, name, description, and associated metrics.
    /// </summary>
    /// <remarks>This class provides a high-level overview of a managed role, including its metadata and
    /// related counts for users,  registration tiers, and services. It is designed to encapsulate role-related
    /// information for display or reporting purposes.</remarks>
    private sealed class ManagedRoleSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedRoleSummary"/> class with the specified role ID, role
        /// name, and description.
        /// </summary>
        /// <param name="roleId">The unique identifier for the role. Cannot be null or empty.</param>
        /// <param name="roleName">The name of the role. Cannot be null or empty.</param>
        /// <param name="description">A brief description of the role. Can be null or empty.</param>
        public ManagedRoleSummary(string roleId, string roleName, string description)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
        }

        /// <summary>
        /// Gets the unique identifier for the role.
        /// </summary>
        public string RoleId { get; }

        /// <summary>
        /// Gets the name of the role associated with the current user or entity.
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the number of users currently active in the system.
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// Gets or sets the number of registration tiers available.
        /// </summary>
        public int RegistrationTierCount { get; set; }

        /// <summary>
        /// Gets or sets the number of services currently available.
        /// </summary>
        /// <remarks>Use this property to track or configure the number of services in the system. 
        /// Setting a negative value will result in an <see cref="ArgumentOutOfRangeException"/>.</remarks>
        public int ServiceCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the metrics have been successfully loaded.
        /// </summary>
        public bool HasMetricsLoaded { get; set; }
    }

    /// <summary>
    /// Represents a single entry in a member roster, containing identifying and membership details for an individual
    /// member.
    /// </summary>
    /// <param name="FirstName">The first name of the member. Cannot be null.</param>
    /// <param name="LastName">The last name of the member. Cannot be null.</param>
    /// <param name="Gender">The gender of the member, as a string. May be null or empty if unspecified.</param>
    /// <param name="MembershipLength">The length of the member's membership, typically expressed as a string (for example, in years or months). May be
    /// null or empty if unknown.</param>
    /// <param name="Contribution">The most recent contribution amount made by the member, represented as a string. May be null or empty if not
    /// applicable.</param>
    /// <param name="TotalContribution">The total contribution amount made by the member, represented as a string. May be null or empty if not
    /// applicable.</param>
    /// <param name="Notes">Additional notes or comments about the member. May be null or empty if there are no notes.</param>
    private sealed record MemberRosterEntry(string FirstName, string LastName, string Gender, string MembershipLength, string Contribution, string TotalContribution, string Notes);
}
