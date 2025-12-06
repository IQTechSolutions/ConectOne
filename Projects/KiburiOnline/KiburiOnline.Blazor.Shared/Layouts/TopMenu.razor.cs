using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the top navigation menu component for the application, providing navigation functionality and
    /// integration with browser scripting.
    /// </summary>
    /// <remarks>This component is typically used to render and manage the application's top-level navigation,
    /// including handling navigation events and invoking JavaScript interop for enhanced user experience. It relies on
    /// dependency injection for navigation and JavaScript runtime services.</remarks>
    public partial class TopMenu
    {
        /// <summary>
        /// Gets or sets the NavigationManager used to manage and perform navigation operations within the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods for programmatic navigation and access to the
        /// current URI. This property is typically injected by the framework and should not be set manually in most
        /// scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework to enable
        /// interoperability between .NET and JavaScript. Assigning a custom implementation may affect how JavaScript
        /// calls are handled.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

        /// <summary>
        /// Navigates asynchronously to the specified section within the current page or to the home page with a scroll
        /// target, depending on the current navigation context.
        /// </summary>
        /// <remarks>If the current page is the home page, the method scrolls directly to the specified
        /// section. Otherwise, it navigates to the home page and sets a scroll target for the section. No action is
        /// taken if <paramref name="sectionId"/> is null, empty, or white-space.</remarks>
        /// <param name="sectionId">The identifier of the section to navigate to. Cannot be null, empty, or consist only of white-space
        /// characters.</param>
        /// <returns>A task that represents the asynchronous navigation operation.</returns>
        private async Task NavigateToSectionAsync(string sectionId)
        {
            if (string.IsNullOrWhiteSpace(sectionId))
            {
                return;
            }

            if (IsOnHomePage())
            {
                NavigationManager.NavigateTo($"/#{sectionId}", false);
                await JsRuntime.InvokeVoidAsync("scrollToElement", sectionId);
            }
            else
            {
                NavigationManager.NavigateTo($"/?scrollTo={sectionId}", false);
            }
        }

        /// <summary>
        /// Determines whether the current navigation location is the application's home page.
        /// </summary>
        /// <returns>true if the current URI path is the root ("/"); otherwise, false.</returns>
        private bool IsOnHomePage()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return uri.AbsolutePath == "/";
        }
    }
}
