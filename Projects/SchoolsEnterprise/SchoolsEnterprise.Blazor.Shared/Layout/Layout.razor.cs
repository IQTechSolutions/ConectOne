using ConectOne.Blazor.Settings;
using ConectOne.Blazor.StateManagers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Layout
{
    /// <summary>
    /// This Layout component is responsible for:
    /// 1. Managing theming (Light or Dark) using MudBlazor.
    /// 2. Handling right-to-left (RTL) layout preferences.
    /// 3. Loading user theme preferences (e.g., Dark/Light mode, RTL) from a ClientPreferenceManager.
    /// </summary>
    public partial class Layout
    {

        // Represents the currently applied MudTheme (Light or Dark).
        private MudTheme _currentTheme = null!;

        // Indicates whether the application UI is in a right-to-left orientation.
        private bool _rightToLeft;

        /// <summary>
        /// Gets or sets the <see cref="ClientPreferenceManager"/> instance used to manage client-specific preferences.
        /// </summary>
        [Inject] public IClientPreferenceManager ClientPreferenceManager { get; set; } = null!;

        /// <summary>
        /// Toggles the layout between right-to-left and left-to-right based on user preference.
        /// </summary>
        /// <param name="value">True if the layout should be in RTL mode, otherwise false.</param>
        private async Task RightToLeftToggle(bool value)
        {
            _rightToLeft = value;
            // Currently no additional async code; use CompletedTask for a placeholder.
            StateHasChanged();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Invoked after the component has rendered. Performs post-rendering logic, such as initializing theme and
        /// layout settings on the first render.
        /// </summary>
        /// <remarks>Override this method to execute logic that should run after the component has
        /// rendered. Use the firstRender parameter to ensure initialization code runs only once.</remarks>
        /// <param name="firstRender">true if this is the first time the component has rendered; otherwise, false.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _currentTheme = ApplciationTheme.LightTheme;

                // Check if the user has RTL mode enabled.
                _rightToLeft = await ClientPreferenceManager.IsRTL();
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Toggles between light and dark modes by querying the ClientPreferenceManager. 
        /// If the user toggles dark mode on, apply the LightTheme (it appears code here 
        /// is reversed in naming, but presumably sets the right theme references).
        /// </summary>
        private async Task DarkMode()
        {
            // The method returns whether Dark Mode is now on or off.
            bool isDarkMode = await ClientPreferenceManager.ToggleDarkModeAsync();

            // Switch the theme based on the isDarkMode result.
            // (Note: The code logic might be reversed from what you'd expect:
            //   If isDarkMode == true => _currentTheme = LightTheme 
            //   else => _currentTheme = DarkTheme
            // This could be a simple naming or variable mismatch, 
            // but it's left as-is to mirror your code.)
            _currentTheme = isDarkMode ? ApplciationTheme.LightTheme : ApplciationTheme.DarkTheme;

            await InvokeAsync(StateHasChanged);
        }
    }
}
