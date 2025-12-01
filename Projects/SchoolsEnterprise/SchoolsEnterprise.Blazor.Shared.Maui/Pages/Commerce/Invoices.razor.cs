using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// Represents a collection of functionality and state related to invoice management within the application.
    /// </summary>
    public partial class Invoices
    {
        private List<BreadcrumbItem> _routes = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Shopping", href: "/shopping"),
            new BreadcrumbItem("Order History", href: null, disabled: true)
        };

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to new pages and retrieving
        /// the current navigation state within a Blazor application. This property is typically set by the framework
        /// through dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Navigates asynchronously to the order detail page in the order history section.
        /// </summary>
        /// <returns>A task that represents the asynchronous navigation operation.</returns>
        private async Task NavigateToOrderDetail()
        {
            NavigationManager.NavigateTo("/orderhistory/detail");
        }

        /// <summary>
        /// Gets or sets the theme used to style components in the application.
        /// </summary>
        /// <remarks>Assigning a custom theme allows you to control the appearance of UI elements
        /// globally. Changes to this property will affect the look and feel of all components that use the
        /// theme.</remarks>
        public MudTheme Theme = new MudTheme();
    }
}
