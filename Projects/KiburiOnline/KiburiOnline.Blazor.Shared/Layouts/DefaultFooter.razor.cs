using System.Security.Claims;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the default footer component for a vacation booking application.
    /// </summary>
    /// <remarks>This component is responsible for displaying footer-related information and functionality, 
    /// such as a list of vacations and an optional "Book Now" button. It interacts with authentication,  navigation,
    /// and JavaScript runtime services to provide dynamic behavior.</remarks>
    public partial class DefaultFooter
    {
        private ClaimsPrincipal _user;
        private bool displayBookNowButton = true;
        private string vacationId;

        /// <summary>
        /// Gets or sets the task that represents the authentication state of the current user.
        /// </summary>
        /// <remarks>This property is typically provided by the authentication system and is used to
        /// access  the current user's authentication state asynchronously. It is marked with the  <see
        /// cref="CascadingParameterAttribute"/> to enable cascading dependency injection in Blazor
        /// components.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation  of <see cref="IVacationService"/> is provided before using this property.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service provides methods for displaying transient
        /// messages to the user,  such as notifications or alerts. Ensure that this property is properly initialized
        /// before use.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should be used to
        /// perform JavaScript interop operations.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
        /// Blazor application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be set
        /// manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        [Parameter] public string VacationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "Book Now" button should be displayed.
        /// </summary>
        [Parameter] public bool DisplayBookNowButton { get; set; }

        /// <summary>
        /// Gets or sets the collection of vacations associated with the entity.
        /// </summary>
        public ICollection<VacationDto> Vacations { get; set; } = new List<VacationDto>();
        
        /// <summary>
        /// Executes logic after the component has rendered, with additional behavior for the first render.
        /// </summary>
        /// <remarks>On the first render, this method initializes the user state, retrieves a paginated
        /// list of vacations, and determines whether to display the "Book Now" button based on the current URI. It also
        /// triggers a state update to reflect these changes. Additionally, if the URI contains a fragment, the method
        /// scrolls to the corresponding element on the page.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has been rendered.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _user = (await AuthenticationStateTask).User;

                await Task.Delay(2000);
                var response = await VacationService.PagedAsync(new VacationPageParameters() { PageSize = 7 });
                response.ProcessResponseForDisplay(Snackbar, () =>
                {
                    Vacations = response.Data.ToList();
                });

                var uriParts = NavigationManager.Uri.Split("/");
                if (uriParts.All(c => c != "packages"))
                {
                    displayBookNowButton = false;
                }
                else
                {
                    vacationId = uriParts[uriParts.Length - 2];
                }

                await InvokeAsync(StateHasChanged);
            }

            var fragment = new Uri(NavigationManager.Uri).Fragment;
            if (!string.IsNullOrEmpty(fragment))
            {
                await JsRuntime.InvokeVoidAsync("ScrollTo", fragment.TrimStart('#'));
            }
        }
    }
}
