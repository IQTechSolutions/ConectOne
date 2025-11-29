using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;

namespace SchoolsEnterprise.Blazor.Shared.Layout
{
    /// <summary>
    /// The MainBody component represents the primary layout structure for the authenticated section of the application.
    /// It includes methods and properties for user interface state management (e.g., dark mode, RTL layout, drawer toggle),
    /// user information retrieval, and integration with a SignalR hub for real-time communication.
    /// </summary>
    public partial class MainBody
    {
        private bool _drawerOpen = true;
        private string _userId = null!;
        private ClaimsPrincipal _user = null!;

        /// <summary>
        /// Contains the current authentication state of the user as a CascadingParameter.
        /// This is typically provided by the Blazor authentication system.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
        /// </summary>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// ChildContent will render the body content passed from a parent component or layout.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;

        /// <summary>
        /// An event callback that is invoked when the user toggles dark mode in the application.
        /// The parent layout or component can handle this event to apply the dark mode preference.
        /// </summary>
        [Parameter] public EventCallback OnDarkModeToggle { get; set; }

        /// <summary>
        /// An event callback that is invoked when the user toggles the text direction (e.g., RTL or LTR).
        /// This allows the parent layout or component to react and update styles accordingly.
        /// </summary>
        [Parameter] public EventCallback<bool> OnRightToLeftToggle { get; set; }

        /// <summary>
        /// On component initialization, attempts to retrieve the preferred layout direction (RTL or LTR)
        /// and registers events for the HTTP Interceptor. Also displays a welcome message if the first name is known.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            try
            {
                //Interceptor.RegisterEvent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
