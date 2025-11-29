using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
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
        /// An injected service used to retrieve accounts and roles.
        /// </summary>
        [Inject] public IRoleService RoleService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Holds a list of <see cref="RoleDto"/> objects returned from the server.
        /// Initially empty, it's populated in the <see cref="OnInitializedAsync"/> method.
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
                var rolesResult = await RoleService.AllRoles();
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
