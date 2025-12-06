using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Contacts
{
    /// <summary>
    /// Represents the page for creating a new contact.
    /// This component provides a form to input contact details and save them to the server.
    /// </summary>
    public partial class ContactsBanner
    {
        private ICollection<ContactDto> _contacts = [];

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration data can include settings from various sources, such as
        /// appsettings.json, environment variables, or user secrets. Ensure that the property is properly initialized
        /// before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage contact information.
        /// </summary>
        [Inject] public IContactService ContactService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and query the current URI within the
        /// application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation actions and access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snackbars in the application.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component by fetching featured contacts from the API.
        /// </summary>
        /// <remarks>This method retrieves a collection of featured contacts and processes the response
        /// for display. It updates the internal state with the retrieved data and displays any relevant messages using
        /// a snackbar.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
           

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Performs post-rendering logic after the component has rendered, and optionally executes additional actions
        /// on the first render.
        /// </summary>
        /// <remarks>Override this method to perform operations that require the component to be rendered,
        /// such as interacting with JavaScript or updating the UI based on data loaded after the initial render. This
        /// method is called after each render, but the firstRender parameter can be used to ensure certain logic runs
        /// only once.</remarks>
        /// <param name="firstRender">true if this is the first time the component has rendered; otherwise, false.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var response = await ContactService.GetFeaturedAsync();
                response.ProcessResponseForDisplay(Snackbar, () =>
                {
                    _contacts = response.Data ?? [];
                });
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
