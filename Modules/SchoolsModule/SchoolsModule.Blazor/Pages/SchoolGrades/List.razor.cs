using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.SchoolGrades
{
    /// <summary>
    /// Represents the list page for managing school grades.
    /// This component displays a table of school grades and provides functionality to search, sort, and delete grades.
    /// </summary>
    public partial class List
    {
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Grades", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];

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

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state of the current user. The task should not be null and is expected to complete with a valid <see
        /// cref="AuthenticationState"/>.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider must be set before making any HTTP requests. Dependency injection is
        /// used to supply the implementation.</remarks>
        [Inject] public ISchoolGradeService SchoolGradeService { get; set; } = null!;

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

        // Private fields for managing the list of school grades and table state
        private IEnumerable<SchoolGradeDto> _schoolGrades = null!;
        private MudTable<SchoolGradeDto> _table = null!;

        /// <summary>
        /// Represents the parameters used to configure the behavior of a school grade page.
        /// </summary>
        /// <remarks>This field is initialized with default values and is intended for internal use within
        /// the class.</remarks>
        private readonly SchoolGradePageParameters _args = new();

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Checks the user's permissions to determine if they can create, edit, or delete school grades.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            // Check if the user has permission to create, edit, or delete school grades
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
        private async Task<TableData<SchoolGradeDto>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<SchoolGradeDto> { TotalItems = _totalItems, Items = _schoolGrades };
        }

        /// <summary>
        /// Loads the data from the server based on the current page number, page size, and table state.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="state">The current state of the table.</param>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            var request = await SchoolGradeService.PagedSchoolGradesAsync(_args);

            if (request.Succeeded)
            {
                var gradeCollection = (request.Data ?? Enumerable.Empty<SchoolGradeDto>())
                    .OrderByNumericText(static grade => grade.SchoolGrade);
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _schoolGrades = gradeCollection;
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Reloads the table data with default parameters.
        /// </summary>
        private async Task Reload()
        {
            _args.OrderBy = "";
            _args.PageSize = 10;
            _args.PageNr = 1;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a school grade by its ID after user confirmation.
        /// </summary>
        /// <param name="eventId">The ID of the school grade to delete.</param>
        private async Task DeleteSchoolGrade(string eventId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this school grade from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await SchoolGradeService.DeleteAsync(eventId);
                await _table.ReloadServerData();
            }
        }
    }
}
