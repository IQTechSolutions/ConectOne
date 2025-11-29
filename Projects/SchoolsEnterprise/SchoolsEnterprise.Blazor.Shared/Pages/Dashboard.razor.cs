using System.Security.Claims;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Pages;

/// <summary>
/// Represents the main dashboard component for authenticated users, providing personalized greetings and access to
/// features based on user roles.
/// </summary>
/// <remarks>The dashboard customizes its content and available actions according to the authenticated user's
/// roles, such as administrator, teacher, parent, or learner. It displays relevant role badges and contextual greetings
/// to enhance the user experience. This component requires authentication and is intended to be used within a Blazor
/// application with proper authentication and navigation services configured.</remarks>
public partial class Dashboard
{
    private ClaimsPrincipal? _user;
    private string _displayName = "there";
    private bool _isLoaded;
    private bool _isAdmin;
    private bool _isTeacher;
    private bool _isParent;
    private bool _isLearner;
    private IReadOnlyList<string> _visibleRoleBadges = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the task that, when completed, provides the current user's authentication state.
    /// </summary>
    /// <remarks>This property is typically supplied as a cascading parameter in Blazor components to enable
    /// access to authentication information. The task may not be completed immediately; consumers should await the task
    /// to obtain the authentication state.</remarks>
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    /// <summary>
    /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
    /// </summary>
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Asynchronously initializes the component and sets up user-related state based on the current authentication
    /// context.
    /// </summary>
    /// <remarks>This method retrieves the current user's authentication state and populates user-specific
    /// properties, such as display name and role information, for use within the component. It is typically called by
    /// the Blazor framework during component initialization.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        _user = authState.User;

        if (_user?.Identity?.IsAuthenticated is not true)
        {
            return;
        }

        _displayName = _user.GetUserDisplayName();
        if (string.IsNullOrWhiteSpace(_displayName))
        {
            _displayName = _user.Identity?.Name ?? "there";
        }

        _isAdmin = _user.IsInRole(RoleConstants.SuperUser) || _user.IsInRole(RoleConstants.Administrator) || _user.IsInRole(RoleConstants.CompanyAdmin);
        _isTeacher = _user.IsInRole(RoleConstants.Teacher);
        _isParent = _user.IsInRole(RoleConstants.Parent);
        _isLearner = _user.IsInRole(RoleConstants.Learner);

        // Capture a human friendly list of roles to surface on the dashboard.
        var roleBadges = new List<string>();
        foreach (var (role, label) in RoleBadgeLabels)
        {
            if (_user.IsInRole(role))
            {
                roleBadges.Add(label);
            }
        }

        if (!_isAdmin && !_isTeacher && !_isParent && !_isLearner && _user.IsInRole(RoleConstants.Basic))
        {
            roleBadges.Add("Community Member");
        }

        _visibleRoleBadges = roleBadges;
        _isLoaded = true;
    }

    /// <summary>
    /// Provides a mapping between role identifiers and their corresponding display labels.
    /// </summary>
    /// <remarks>This array can be used to retrieve the user-friendly label for a given role constant. The
    /// order of elements is consistent with the defined role hierarchy.</remarks>
    private static readonly (string Role, string Label)[] RoleBadgeLabels =
    [
        (RoleConstants.SuperUser, "Super User"),
        (RoleConstants.Administrator, "Administrator"),
        (RoleConstants.CompanyAdmin, "Company Administrator"),
        (RoleConstants.Teacher, "Teacher"),
        (RoleConstants.Parent, "Parent"),
        (RoleConstants.Learner, "Learner")
    ];

    /// <summary>
    /// Gets the personalized greeting message for the current user based on their role.
    /// </summary>
    private string CurrentGreeting => _isAdmin ? "Keep the school community running smoothly with these leadership tools."
        : _isTeacher ? "Plan lessons, manage activities, and communicate with your classes."
            : _isParent ? "Track your learner's progress and stay close to the school community."
                : _isLearner ? "Stay on top of your classes, activities, and announcements."
                    : "Explore the latest updates and happenings at Nelspruit High.";
}
