using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the top navigation bar component, which integrates user authentication state.
    /// </summary>
    /// <remarks>This component retrieves the current user's authentication state asynchronously during
    /// initialization. It is typically used in Blazor applications to display user-specific information or navigation
    /// options.</remarks>
    public partial class TopBar
    {
        private ClaimsPrincipal _user;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is commonly used in Blazor components to access the authentication
        /// state asynchronously. Ensure that the task is not null and is properly initialized by the authentication
        /// system.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The <see cref="IConfiguration"/> instance can be used to retrieve application
        /// settings, such as  connection strings, app-specific options, or environment variables. Ensure that the
        /// configuration  source is properly initialized before accessing this property.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and retrieves the current user's authentication state.
        /// </summary>
        /// <remarks>This method retrieves the authenticated user from the <see
        /// cref="AuthenticationStateTask"/>  and performs any additional initialization logic defined in the base
        /// class.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            _user = (await AuthenticationStateTask).User;

            await base.OnInitializedAsync();
        }
    }
}
