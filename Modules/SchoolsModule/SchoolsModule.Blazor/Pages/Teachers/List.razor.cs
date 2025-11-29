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

namespace SchoolsModule.Blazor.Pages.Teachers
{
    /// <summary>
    /// Represents the list page for managing teachers.
    /// This component displays a table of teachers and provides functionality to search, sort, and delete teachers.
    /// </summary>
    public partial class List
    {
        private IEnumerable<TeacherViewModel> _teachers = null!;
        private MudTable<TeacherViewModel> _table = null!;

        private int _totalItems;
        private int _currentPage;

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private bool _loaded;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state of the current user. The task should not be null and is expected to complete with a valid <see
        /// cref="AuthenticationState"/>.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations.
        /// </summary>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;
        
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
        [Inject]public IAuthorizationService AuthorizationService { get; set; } = null!;

        // Parameters for paging and filtering teachers
        private TeacherPageParameters args = new();

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Checks the user's permissions to determine if they can create, edit, or delete teachers.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            // Check if the user has permission to create, edit, or delete teachers
            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.TeacherPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.TeacherPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.TeacherPermissions.Delete)).Succeeded;

            _loaded = true;

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Reloads the table data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<TeacherViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(args.SearchText))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<TeacherViewModel> { TotalItems = _totalItems, Items = _teachers };
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

            args.OrderBy = sortOrder;
            args.PageSize = pageSize;
            args.PageNr = pageNumber + 1;

            var request = await TeacherService.PagedTeachersAsync(args);
            if (request.Succeeded)
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _teachers = request.Data.Select(c => new TeacherViewModel(c));
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Searches for teachers based on the provided text.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async Task OnSearch(string text)
        {
            args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a teacher by their ID after user confirmation.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to delete.</param>
        private async Task DeleteTeacher(string teacherId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this teacher from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await TeacherService.RemoveAsync(teacherId);
                if (deleteResult.Succeeded)
                {
                    await _table.ReloadServerData();
                }
            }
        }
    }
}