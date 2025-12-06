using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KiburiOnline.Blazor.Shared.Pages.GolfCourses
{
    /// <summary>
    /// Represents a component that initializes and manages metadata transfer and navigation details for a page.
    /// </summary>
    /// <remarks>This component is designed to set up page-specific metadata, including breadcrumbs and
    /// navigation details,  during its initialization phase. It relies on dependency injection to access services such
    /// as  <see cref="MetadataTransferService"/> and <see cref="NavigationManager"/>.</remarks>
	public partial class List
	{
        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> contains information about the user's identity
        /// and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;
	}
}
