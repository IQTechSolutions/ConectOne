using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents a navigation menu component that tracks the current URL and updates its state when the location
    /// changes.
    /// </summary>
    /// <remarks>This component listens for location changes using the <see cref="NavigationManager"/> and
    /// updates its internal state accordingly.  It is designed to be used in Blazor applications where dynamic updates
    /// to the navigation menu are required.</remarks>
    public partial class NavMenu
    {
        private string? currentUrl;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> used to manage and perform navigation within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for
        /// programmatic navigation and access to the current URI. It is commonly used in Blazor components to navigate
        /// to different pages or to respond to navigation events.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Invoked when the component is initialized. Sets the current URL and subscribes to location change events.
        /// </summary>
        /// <remarks>This method retrieves the base-relative path of the current URL and assigns it to the
        /// <c>currentUrl</c> field.  It also subscribes to the <see cref="NavigationManager.LocationChanged"/> event to
        /// handle navigation changes.</remarks>
        protected override void OnInitialized()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        /// <summary>
        /// Handles the event triggered when the location changes in the application.
        /// </summary>
        /// <param name="sender">The source of the event. This can be <see langword="null"/>.</param>
        /// <param name="e">The event data containing information about the new location.</param>
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        /// <summary>
        /// Releases resources used by the current instance and unsubscribes from the location change event.
        /// </summary>
        /// <remarks>This method should be called when the instance is no longer needed to ensure proper
        /// cleanup of resources. After calling this method, the instance should not be used.</remarks>
        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
