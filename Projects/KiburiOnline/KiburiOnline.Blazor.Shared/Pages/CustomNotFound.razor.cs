using Microsoft.AspNetCore.Components;

namespace KiburiOnline.Blazor.Shared.Pages
{
    /// <summary>
    /// Represents a component that handles navigation when a requested resource is not found.
    /// </summary>
    /// <remarks>This class is typically used to display a custom "not found" page and provides functionality
    /// to navigate users back to the application's home page.</remarks>
    public partial class CustomNotFound
    {
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> used for managing URI navigation and location state within
        /// the application.
        /// </summary>
        [Inject]public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Navigates the application to the home page.
        /// </summary>
        /// <remarks>This method performs a client-side navigation to the application's root URL
        /// ("/").</remarks>
        public void NavigateToHome()
        {
            NavigationManager.NavigateTo("/");
        }        
    }
}
