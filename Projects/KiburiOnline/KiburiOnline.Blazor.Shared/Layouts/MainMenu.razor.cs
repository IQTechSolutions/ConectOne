using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the main menu component of the application, providing navigation and booking functionality.
    /// </summary>
    /// <remarks>This component is responsible for rendering the main menu and handling user interactions such
    /// as  navigating to specific sections of the application or initiating the booking process for a vacation.  It
    /// integrates with various services, including navigation, JavaScript runtime, and a vacation service,  to provide
    /// its functionality. The component also supports dependency injection for configuration and  other
    /// services.</remarks>
    public partial class MainMenu
    {
        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
        /// Blazor application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be set
        /// manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service provides methods for displaying transient
        /// messages to the user,  such as notifications or alerts. Ensure that this property is properly initialized
        /// before use.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the "Book Now" button should be displayed.
        /// </summary>
        [Parameter] public bool displayBookNowButton { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        [Parameter] public string vacationId { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation  of <see cref="IVacationService"/> is provided before using this property.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
        /// </summary>
        /// <remarks>The <see cref="IJSRuntime"/> instance allows .NET code to call JavaScript functions
        /// in the browser or other JavaScript environments. Ensure that this property is properly initialized before
        /// using it to perform JavaScript interop.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

        /// <summary>
        /// Scrolls to the specified element on the current page or navigates to the element on a different page.
        /// </summary>
        /// <remarks>If the current page is the home page, this method scrolls to the specified element
        /// using JavaScript. Otherwise, it navigates to the element on a different page by updating the URL
        /// fragment.</remarks>
        /// <param name="elementId">The ID of the element to scroll to or navigate to. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task ScrollOrNavigateAsync(string sectionId)
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
                NavigationManager.NavigateTo($"/#{sectionId}", false);
                await JsRuntime.InvokeVoidAsync("scrollToElement", sectionId);
            }
        }

        /// <summary>
        /// Initiates the booking process for the specified vacation.
        /// </summary>
        /// <remarks>This method retrieves the vacation details based on the provided vacation name and
        /// navigates  to the booking creation page if the retrieval is successful. If the retrieval fails, error 
        /// messages are displayed using the SnackBar.</remarks>
        /// <returns></returns>
        private async Task BookNow()
        {
            var vacationIdResult = await VacationService.VacationAsync(vacationId);
            if (!vacationIdResult.Succeeded)
            {
                Snackbar.AddErrors(vacationIdResult.Messages);
                return;
            }

            NavigationManager.NavigateTo($"/bookings/create/{vacationIdResult.Data.VacationId}", true);
        }

        /// <summary>
        /// Determines whether the current page is the home page.
        /// </summary>
        /// <remarks>The method checks if the absolute path of the current URI matches the root path
        /// ("/").</remarks>
        /// <returns><see langword="true"/> if the current page is the home page; otherwise, <see langword="false"/>.</returns>
        private bool IsOnHomePage()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return uri.AbsolutePath == "/";
        }
    }
}
