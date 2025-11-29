using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using MessagingModule.Blazor.Modals;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NeuralTech.Base.Enums;

namespace MessagingModule.Blazor.Components.Messages
{
    /// <summary>
    /// The MessagesTable component displays and manages a paginated list of messages.
    /// It supports sorting, searching, deletion of messages, and opening a details modal.
    /// It integrates with an <see cref="IMessagingProvider"/> for server-side data retrieval
    /// and optionally sends push notifications via a callback.
    /// </summary>
    public partial class MessagesTable
    {
        private ICollection<MessageDto> _messages = null!;
        private MudTable<MessageDto> _table = null!;
        private int _totalItems;
        private int _currentPage;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private readonly MessagePageParameters _args = new();

        /// <summary>
        /// Obtains the current user's authentication state (e.g., identity/claims).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs and notifications.
        /// </summary>
        /// <remarks>This property must be set to a valid instance of <see cref="IDialogService"/> before
        /// it can be used.</remarks>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> instance used to display notifications or messages.
        /// </summary>
        /// <remarks>This property must be set to a valid <see cref="ISnackbar"/> implementation before it
        /// can be used.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to send requests and handle responses.
        /// </summary>
        /// <remarks>The <see cref="Provider"/> property must be set to a valid instance of <see
        /// cref="IBaseHttpProvider"/> before making any HTTP requests. This property is critical for configuring the
        /// behavior of HTTP operations, such as request serialization, response deserialization, and error
        /// handling.</remarks>
        [Inject] public IMessageService MessageService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Optional ID of an entity related to filtering messages (e.g., a user, event, etc.).
        /// </summary>
        [Parameter] public string? EntityId { get; set; }

        /// <summary>
        /// Optional receiver ID for filtering messages intended for a specific user.
        /// </summary>
        [Parameter] public string? ReceiverId { get; set; }

        /// <summary>
        /// Optional sender ID for filtering messages from a particular user.
        /// </summary>
        [Parameter] public string? SenderId { get; set; }

        /// <summary>
        /// Optional integer representing the <see cref="MessageType"/> (cast on initialization).
        /// </summary>
        [Parameter] public int? MessageType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the public message feature is enabled.
        /// </summary>
        [Parameter] public bool PublicMessage { get; set; }

        /// <summary>
        /// Event callback for sending a push notification about a particular message.
        /// If set, the component can invoke this callback after a user action.
        /// </summary>
        [Parameter] public EventCallback<MessageDto> SendPushNotification { get; set; }

        /// <summary>
        /// Opens a modal dialog to display details for a particular message, and optionally 
        /// send a push notification if the user chooses an action in the modal.
        /// </summary>
        private async Task ShowMessageDetails(string messageId, string? notificationId)
        {
            var dialogOptions = new  DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true };
            var parameters = new DialogParameters<DetailsModal>
            {
                { messageDetailsModal => messageDetailsModal.MessageId, messageId },
                { messageDetailsModal => messageDetailsModal.NotificationId, notificationId }
            };

            var dialog = await DialogService.ShowAsync<DetailsModal>("Message Details", parameters, dialogOptions);
            var result = await dialog.Result;

            // If modal is confirmed, potentially trigger a push notification for this message.
            if (!result!.Canceled)
            {
                var messageDto = (MessageDto)result.Data;
                await SendPushNotification.InvokeAsync(messageDto);
            }
        }

        /// <summary>
        /// Invoked by the MudTable for server-side data retrieval. 
        /// Loads messages from the server based on table state (paging, sorting) and local filters.
        /// </summary>
        private async Task<TableData<MessageDto>> ServerReload(TableState state, CancellationToken token)
        {
            // If the user typed a search, reset the page to 0.
            if (!string.IsNullOrWhiteSpace(_args.SearchText))
            {
                state.Page = 0;
            }

            // Load data from the server for the given page, page size, and sort state.
            await LoadData(state.Page, state.PageSize, state);

            // Return data for the table to render.
            return new TableData<MessageDto>
            {
                TotalItems = _totalItems,
                Items = _messages
            };
        }

        /// <summary>
        /// Loads messages from the server after preparing the filter and paging parameters.
        /// </summary>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            // Apply the sorting from the table state into our request parameters.
            ConfigureFilters(state);

            _args.PageSize = pageSize;
            _args.PageNr = pageNumber + 1;

            await ConfigureProvider();
        }

        /// <summary>
        /// Examines the table’s sorting state and updates our request arguments accordingly.
        /// </summary>
        private void ConfigureFilters(TableState state)
        {
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = "";
                if (state.SortDirection == SortDirection.Ascending)
                    sortDirection = "asc";
                if (state.SortDirection == SortDirection.Descending)
                    sortDirection = "desc";

                _args.OrderBy = state.SortDirection != SortDirection.None
                    ? $"{state.SortLabel} {sortDirection}"
                    : string.Empty;
            }
        }

        /// <summary>
        /// Calls the messaging provider to fetch a paged list of messages. 
        /// After retrieval, sets local fields for total count and the messages data.
        /// </summary>
        private async Task ConfigureProvider()
        {
            var request = await MessageService.PagedMessagesAsync(_args);
            request.ProcessResponseForDisplay(SnackBar, () =>
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _messages = request.Data;
            });
        }

        /// <summary>
        /// Callback for searching messages by text, then reloading the table.
        /// </summary>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a message by its ID after displaying a confirmation dialog.
        /// If confirmed, reloads the table data from the server.
        /// </summary>
        private async Task Delete(string messageId)
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
                var removalResult = await MessageService.DeleteMessageAsync(messageId);
                if(removalResult.Succeeded)
                    await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and sets up the necessary state and permissions.
        /// </summary>
        /// <remarks>This method determines the user's authorization for various messaging and calendar
        /// permissions  and configures the component's state based on the provided parameters. It also ensures that 
        /// the component is fully loaded before invoking the base implementation.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.MessagePermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.MessagePermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.MessagePermissions.Delete)).Succeeded;

            _args.Public = PublicMessage;

            if (ReceiverId != null)
                _args.ReceiverId = ReceiverId;

            if (EntityId != null)
                _args.EntityId = EntityId;

            if (SenderId != null)
                _args.SenderId = SenderId;

            if (MessageType != null)
                _args.MessageType = (MessageType)MessageType;

            _loaded = true;

            await base.OnInitializedAsync();
        }
    }
}
