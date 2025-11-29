using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.AgeGroups
{
    /// <summary>
    /// The List component is responsible for displaying a list of age groups.
    /// It provides functionality to search, sort, and delete age groups.
    /// </summary>
    public partial class List
    {
        private IEnumerable<AgeGroupViewModel> _ageGroups = null!;
        private MudTable<AgeGroupViewModel> _table = null!;
        private int _totalItems;
        private int _currentPage;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canCreateChat;
        private bool _canSendMessage;
        private readonly AgeGroupPageParameters _args = new();
        private readonly List<BreadcrumbItem> _items = new()
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Grades", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IAgeGroupService AgeGroupService { get; set; } = null!;


        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;
        
        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Checks the user's permissions to determine if they can create, edit, or delete age groups.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.SystemDataPermissions.Delete)).Succeeded;
            _canCreateChat = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanCreateChat)).Succeeded;
            _canSendMessage = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;

            _loaded = true;

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Reloads the table data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<AgeGroupViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<AgeGroupViewModel> { TotalItems = _totalItems, Items = _ageGroups };
        }

        /// <summary>
        /// Loads the data from the server based on the current page number, page size, and table state.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="state">The current state of the table.</param>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string sortOrder = string.Empty;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = "";
                if (state.SortDirection == SortDirection.Ascending)
                    sortDirection = "asc";
                if (state.SortDirection == SortDirection.Descending)
                    sortDirection = "desc";

                sortOrder = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }

            _args.OrderBy = sortOrder;
            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            var request = await AgeGroupService.PagedAgeGroupsAsync(_args);
            if (request.Succeeded)
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _ageGroups = request.Data.Select(c => new AgeGroupViewModel(c));
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Reloads the table data from the server.
        /// </summary>
        private async Task Reload()
        {
            _args.PageNr = 1;
            _args.OrderBy = "";
            _args.PageSize = 10;

            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes an age group by its ID after user confirmation.
        /// </summary>
        /// <param name="ageGroupId">The ID of the age group to delete.</param>
        private async Task DeleteAgeGroup(string ageGroupId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this age group from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await AgeGroupService.DeleteAsync(ageGroupId);
                await _table.ReloadServerData();
            }
        }
    }
}
