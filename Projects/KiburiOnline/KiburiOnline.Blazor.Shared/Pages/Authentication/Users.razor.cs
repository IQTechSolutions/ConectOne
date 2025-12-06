using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The Users component demonstrates how to retrieve and display a list of roles 
    /// within an application’s authentication module. It makes a call to 
    /// <see cref="IAccountsProvider"/> to fetch all roles, then processes and stores 
    /// them locally for display or further manipulation.
    /// </summary>
    public partial class Users
    {
        private bool _loaded;
        
        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;
 
        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Holds a list of <see cref="RoleDto"/> objects returned from the server.
        /// </summary>
        private IEnumerable<RoleDto> _roles = Array.Empty<RoleDto>();

        /// <summary>
        /// Executes custom logic after the component has rendered.
        /// </summary>
        /// <remarks>If <paramref name="firstRender"/> is <see langword="true"/>, this method retrieves
        /// all roles using the <c>AccountsProvider</c>, processes the response for display, and updates the component's
        /// state.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has been rendered.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // Retrieve all roles using the AccountsProvider
                var rolesResult = await Provider.GetAsync<IEnumerable<RoleDto>>("account/roles");     
                rolesResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _roles = rolesResult.Data;
                });

                _loaded = true;

                StateHasChanged();
            }
        }
    }
}
