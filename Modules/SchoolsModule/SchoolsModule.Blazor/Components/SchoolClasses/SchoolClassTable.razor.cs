using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.SchoolClasses
{
    /// <summary>
    /// The SchoolClassTable component is responsible for displaying a table of school classes.
    /// It provides functionality to load, sort, and delete school classes.
    /// </summary>
    public partial class SchoolClassTable
    {
        private string _currentUserId = string.Empty;
        private int _totalItems;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;

        /// <summary>
        /// Gets or sets the authentication state task used to retrieve the current user's identity and roles.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; } = null!;
        
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
        /// The ID of the grade to which the school classes belong.
        /// </summary>
        [Parameter] public string GradeId { get; set; } = null!;

        /// <summary>
        /// Parameters for paging and filtering school classes.
        /// </summary>
        private SchoolClassPageParameters _args = new();

        /// <summary>
        /// Collection of school class view models.
        /// </summary>
        private IEnumerable<SchoolClassViewModel> _schoolClasses = null!;

        /// <summary>
        /// Reference to the MudTable for programmatic interaction (e.g., reloading).
        /// </summary>
        private MudTable<SchoolClassViewModel> _table = null!;

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
                var sortDirection = state.SortDirection == SortDirection.Ascending ? "asc" : "desc";
                sortOrder = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }

            _args.OrderBy = sortOrder;
            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            var request = await SchoolClassService.PagedSchoolClassesAsync(_args);
            if (request.Succeeded)
            {
                var classCollection = (request.Data ?? Enumerable.Empty<SchoolClassDto>())
                    .Select(dto => new SchoolClassViewModel(dto))
                    .OrderByNumericText(static schoolClass => schoolClass.SchoolClass);
                _totalItems = request.TotalCount;
                _schoolClasses = classCollection;
            }
        }

        /// <summary>
        /// Reloads the table data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<SchoolClassViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<SchoolClassViewModel> { TotalItems = _totalItems, Items = _schoolClasses };
        }

        /// <summary>
        /// Reloads the table data from the server.
        /// </summary>
        private async Task Reload()
        {
            _args.OrderBy = "";
            _args.PageSize = 10;
            _args.PageNr = 1;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a school class by its ID after user confirmation.
        /// </summary>
        /// <param name="eventId">The ID of the school class to delete.</param>
        private async Task DeleteEvent(string eventId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this school class from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                await SchoolClassService.DeleteAsync(eventId);
                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Initiates the creation or joining of a chat group for the specified parent.
        /// On success, navigates to the newly created chat group.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateChatGroup(string schoolClassId)
        {
            var result = await SchoolClassService.CreateSchoolClassChatGroupAsync(schoolClassId, _currentUserId);

            if (result.Succeeded)
            {
                NavigationManager.NavigateTo($"chats/{result.Data}");
            }
            else
            {
                SnackBar.AddErrors(result.Messages);
            }
        }

        #region Overrides

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the grade ID for the school classes.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _currentUserId = authState.User.GetUserId();

            _args.GradeId = GradeId;

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
