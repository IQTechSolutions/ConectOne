using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MessagingModule.Application.ViewModels;
using MessagingModule.Blazor.Modals;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;

namespace MessagingModule.Blazor.Components.Notifications
{
    /// <summary>
    /// The NotificationsTable component is responsible for displaying a table of notifications.
    /// It provides functionality to view, search, sort, and delete notifications.
    /// </summary>
    public partial class NotificationsTable
    {
        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Collection of notifications to be displayed in the table.
        /// </summary>
        private ICollection<NotificationViewModel> _notifications = null!;

        /// <summary>
        /// The ID of the receiver to filter notifications by.
        /// </summary>
        [Parameter] public string? ReceiverId { get; set; }

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts in the user interface. Ensure that the service is properly configured and injected before
        /// use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public INotificationService NotificationService { get; set; } = null!;

        /// <summary>
        /// The ID of the entity to filter notifications by.
        /// </summary>
        [Parameter] public string? EntityId { get; set; }

        /// <summary>
        /// Reference to the MudTable component for notifications.
        /// </summary>
        private MudTable<NotificationViewModel> _table = null!;

        /// <summary>
        /// Total number of items in the table.
        /// </summary>
        private int _totalItems;

        /// <summary>
        /// Current page number in the table.
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// Indicates whether the table rows are dense.
        /// </summary>
        private bool _dense = false;

        /// <summary>
        /// Indicates whether the table rows are striped.
        /// </summary>
        private bool _striped = true;

        /// <summary>
        /// Indicates whether the table has borders.
        /// </summary>
        private bool _bordered = false;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Indicates whether the user has permission to create notifications.
        /// </summary>
        private bool _canCreate;

        /// <summary>
        /// Indicates whether the user has permission to edit notifications.
        /// </summary>
        private bool _canEdit;

        /// <summary>
        /// Indicates whether the user has permission to delete notifications.
        /// </summary>
        private bool _canDelete;

        /// <summary>
        /// Parameters for paging and filtering notifications.
        /// </summary>
        private readonly NotificationPageParameters _args = new();

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the loaded flag to true.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _loaded = true;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Shows the details of a specific notification in a modal dialog.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to view.</param>
        private async Task ShowNotificationDetails(string notificationId)
        {
            var parameters = new DialogParameters<NotificationDetailsModal>
            {
                { messageDetailsModal => messageDetailsModal.NotificationId, notificationId }
            };

            var dialog = await DialogService.ShowAsync<NotificationDetailsModal>("Notification Details", parameters);
            var result = await dialog.Result;
            if (result.Canceled)
            {
                // Handle the case when the dialog is canceled
            }
        }

        /// <summary>
        /// Reloads the table data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A TableData object containing the total items and the items to display.</returns>
        private async Task<TableData<NotificationViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_args.SearchText))
            {
                state.Page = 0;
            }

            await LoadData(state.Page, state.PageSize, state);

            return new TableData<NotificationViewModel> { TotalItems = _totalItems, Items = _notifications };
        }

        /// <summary>
        /// Loads the data for the table based on the current page number, page size, and table state.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="state">The current state of the table.</param>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            ConfigureFilters(state);

            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            await ConfigureProvider();
        }

        /// <summary>
        /// Configures the filters for the table based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        private void ConfigureFilters(TableState state)
        {
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = "";
                if (state.SortDirection == SortDirection.Ascending)
                    sortDirection = "asc";
                if (state.SortDirection == SortDirection.Descending)
                    sortDirection = "desc";

                _args.OrderBy = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }
        }

        /// <summary>
        /// Configures the provider to fetch the notifications from the server.
        /// </summary>
        private async Task ConfigureProvider()
        {
            var request = await NotificationService.PagedNotificationsAsync(_args);
            request.ProcessResponseForDisplay(SnackBar, () =>
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _notifications = request.Data.Select(c => new NotificationViewModel(c)).ToList();
            });
        }

        /// <summary>
        /// Performs a search for notifications based on the specified text.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a specific notification after user confirmation.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to delete.</param>
        private async Task DeleteNotification(string notificationId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this notification from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await NotificationService.RemoveNotificationAsync(notificationId);
                await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Lifecycle method invoked when the component is initialized.
        /// Sets the receiver ID in the notification parameters if provided.
        /// </summary>
        protected override void OnInitialized()
        {
            if (ReceiverId != null)
                _args.ReceiverId = ReceiverId;

            base.OnInitialized();
        }
    }
}
