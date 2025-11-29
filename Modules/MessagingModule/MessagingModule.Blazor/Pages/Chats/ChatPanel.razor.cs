using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.RequestFeatures;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using MessagingModule.Application.ViewModels;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using NeuralTech.Base.Enums;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using FilingModule.Application.Services;
using static IdentityModule.Domain.Constants.Permissions;

namespace MessagingModule.Blazor.Pages.Chats
{
    /// <summary>
    /// Represents the main chat panel component.
    /// This component handles user authentication, SignalR connection management,
    /// message sending and receiving, and chat group interactions.
    /// </summary>
    public partial class ChatPanel : IAsyncDisposable
    {
        #region Private Fields

        private List<ChatMessageDto> _messages = new();
        private List<ChatGroupDto> _chatGroups = new();
        private List<UserInfoDto> _chatGroupMembers = new();

        private readonly HashSet<string> _onlineUsers = new();
        private readonly Dictionary<string, DateTime?> _messageReadTimes = new();
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        private const string SignalRHubUrl = "/signalRHub";
        private const string OnConnectMethod = "OnConnectAsync";
        private const string OnDisconnectMethod = "OnDisconnectAsync";
        private const string PingRequestMethod = "PingRequestAsync";
        private const string PingResponseMethod = "PingResponseAsync";
        private const string ConnectUserEvent = "ConnectUser";
        private const string DisconnectUserEvent = "DisconnectUser";
        private const string PingRequestEvent = "PingRequestAsync";
        private const string PingResponseEvent = "PingResponseAsync";

        private string? _currentUserId;
        private string? _currentUserName;
        private string? _currentUserDisplayName;

        private bool _uploadInProgress;
        private bool _busySending;
        private bool _open;
        private Anchor _anchor;
        private ClaimsPrincipal user;
        private readonly ICollection<UploadResult> _objectsBusyUploading = [];

        private List<ImageDto> _chatImages = [];
        private List<VideoDto> _chatVideos = [];
        private List<DocumentDto> _chatDocuments = [];

        /// <summary>
        /// Indicates whether the SignalR callbacks have already been wired.
        /// </summary>
        private bool _callbacksWired;

        /// <summary>
        /// Cached delegate for the message received handler so it can be
        /// removed when the component is disposed.
        /// </summary>
        private Action<ChatMessageDto>? _messageReceivedHandler;

        /// <summary>
        /// Cached delegate for the message read handler so it can be
        /// removed when the component is disposed.
        /// </summary>
        private Action<string, string, DateTime>? _messageReadHandler;

        #endregion

        #region Injected Services

        /// <summary>
        /// Provides the current authentication state of the user.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Base HTTP provider for communicating with the backend API.
        /// </summary>
        [Inject] private IChatService ChatService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage chat groups.
        /// </summary>
        [Inject] private IChatGroupService ChatGroupService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications.
        /// </summary>
        [Inject] private IPushNotificationService PushNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Assign an implementation of
        /// IBaseHttpProvider to customize how HTTP requests are handled.</remarks>
        [Inject] private IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Snackbar service used for displaying messages to the user.
        /// </summary>
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Provides configuration settings for building SignalR connections.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Navigation manager for redirecting between routes.
        /// </summary>
        [Inject] private NavigationManager NavManager { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The current selected group ID to load chat messages for.
        /// </summary>
        [Parameter] public string? CurrentGroupId { get; set; }

        #endregion

        #region UI Interaction Methods

        /// <summary>
        /// Opens the drawer from the specified anchor.
        /// </summary>
        /// <param name="anchor">The side of the screen to open the drawer from.</param>
        private async Task OpenDrawer(Anchor anchor)
        {
            _anchor = anchor;
            _open = true;

            if (!string.IsNullOrEmpty(CurrentGroupId))
            {
                await LoadGroupMembersAsync(CurrentGroupId);
            }
        }

        /// <summary>
        /// Represents a collection of breadcrumb items used for navigation.
        /// </summary>
        /// <remarks>Each breadcrumb item includes a label, a hyperlink, and an optional icon.  This
        /// collection is pre-initialized with default items such as "Dashboard" and "Chat Panel."</remarks>
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Chat Panel", href: "#", icon: Icons.Material.Filled.PeopleAlt)
        ];

        #endregion

        #region Chat Logic Methods

        /// <summary>
        /// Asynchronously loads the members of a specified chat group.
        /// </summary>
        /// <remarks>This method retrieves the list of group members from the data provider and updates
        /// the internal collection of chat group members. Ensure that the <paramref name="groupId"/> is valid and
        /// corresponds to an existing chat group.</remarks>
        /// <param name="groupId">The unique identifier of the chat group whose members are to be loaded. Cannot be null or empty.</param>
        private async Task LoadGroupMembersAsync(string groupId)
        {
            var result = await ChatGroupService.GetGroupMembersAsync(groupId);
            if (result.Succeeded && result.Data != null)
            {
                _chatGroupMembers = result.Data.ToList();
            }
        }

        /// <summary>
        /// Determines whether the specified user is currently online.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to check. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the user is online; otherwise, <see langword="false"/>.</returns>
        private bool IsUserOnline(string userId)
        {
            var bb = _onlineUsers.Any(c => c == userId);
            return bb;
        } 

        /// <summary>
        /// Loads messages for the selected chat group.
        /// </summary>
        /// <param name="group">The chat group to load messages for.</param>
        private async Task LoadMessagesForGroup(ChatGroupDto group)
        {
            if (!_chatGroups.Any(g => g.Id == group.Id)) return;
            
            CurrentGroupId = group.Id;
            
            var result = await ChatService.GetRecentMessagesAsync(group.Id);
            if (!result.Succeeded)
            {
                Snackbar.Add("There was an error retrieving the messages", Severity.Error);
                return;
            }

            _messages = result.Data.ToList();
        }

        /// <summary>
        /// Asynchronously uploads the attachments associated with a message to the specified entity.
        /// </summary>
        /// <remarks>This method processes the attachments in parallel, including images, documents, and
        /// videos. If an upload is already in progress, the method will return immediately without performing any
        /// action.</remarks>
        /// <param name="message">The message containing the attachments to be uploaded.</param>
        /// <param name="entityId">The identifier of the entity to which the attachments will be uploaded.</param>
        private async Task UploadMessageAttachementsAsync(MessageInputViewModel message, string entityId)
        {
            if (_uploadInProgress) return; 
            _uploadInProgress = true;

            try
            {
                var uploadTasks = new List<Task>
                {
                    Parallel.ForEachAsync(message.Images.Select(c => Tuple.Create(c, entityId)).ToList(), _cancellationToken, UploadImageFileAsync),
                    Parallel.ForEachAsync(message.Files.Select(c => Tuple.Create(c, entityId)).ToList(), _cancellationToken, UploadDocumentFileAsync),
                    Parallel.ForEachAsync(message.Videos.Select(c => Tuple.Create(c, entityId)).ToList(), _cancellationToken, UploadVideoFileAsync),
                };

                await Task.WhenAll(uploadTasks);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
            }
            finally { _uploadInProgress = false; }
        }
        
        /// <summary>
        /// Initializes a new <see cref="UploadResult"/> instance with the specified total bytes and adds it to the list
        /// of active uploads.
        /// </summary>
        /// <param name="totalBytes">The total number of bytes to be uploaded. Must be a non-negative value.</param>
        /// <returns>A new <see cref="UploadResult"/> instance with its <see cref="Progress{T}"/> set to 0 and <see
        /// cref="UploadResult.TotalBytes"/> set to the specified value.</returns>
        private UploadResult InitialiseUploadResult(long totalBytes)
        {
            var ui = new UploadResult { Progress = 0, TotalBytes = totalBytes };
            _objectsBusyUploading.Add(ui);
            return ui;
        }


        /// <summary>
        /// Sends a message to the current chat group.
        /// </summary>
        private async Task SendMessageAsync(MessageInputViewModel message)
        {
            _busySending = true;

            if (!string.IsNullOrEmpty(_currentUserName) && !string.IsNullOrEmpty(CurrentGroupId))
            {
                var guid = Guid.NewGuid().ToString();
                var chatMessage = new ChatMessageDto()
                {
                    Id = guid,
                    SenderId = _currentUserId,
                    SenderUserName = _currentUserName,
                    SenderDisplayName = user.GetUserDisplayName(),
                    ChatGroupId = CurrentGroupId,
                    Content = message.Message,
                    Timestamp = DateTime.Now
                };

                var result = await ChatService.SaveMessageAsync(chatMessage);
                if (!result.Succeeded) await Result.FailAsync(result.Messages);

                await UploadMessageAttachementsAsync(message, guid);

                var newChatMessage = new ChatMessageDto()
                {
                    Id = guid,
                    SenderId = _currentUserId,
                    SenderUserName = _currentUserName,
                    SenderDisplayName = user.GetUserDisplayName(),
                    Content = message.Message,
                    IsRead = false,
                    ReadTime = null,
                    Timestamp = DateTime.Now,
                    Images = _chatImages
                        .Select(c => $"{c.RelativePath}").ToList(),
                    Documents = _chatDocuments
                        .Select(c => $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{c.RelativePath}").ToList(),
                    Videos = _chatVideos
                        .Select(c => $"{Configuration["ApiConfiguration:BaseApiAddress"]}/{c.RelativePath}").ToList()
                };

                _messages.Add(newChatMessage);

                _messageReadTimes[guid] = null; // receiver has not read yet
                
                // send push notifications to offline members
                var offlineMembers = _chatGroupMembers.Where(m => m.UserId != _currentUserId && !IsUserOnline(m.UserId)).ToList();
                if (offlineMembers.Any())
                {
                    var recipients = offlineMembers.Select(m => new RecipientDto(m.UserId, m.FirstName ?? string.Empty, m.LastName ?? string.Empty, new List<string>() { m.EmailAddress }, true, false, m.CoverImageUrl, MessageType.Global)).ToList();
                    var notification = new NotificationDto
                    {
                        Title = $"New message from {_currentUserDisplayName}",
                        ShortDescription = message.Message,
                        Message = message.Message,
                        NotificationUrl = $"/chats/{CurrentGroupId}",
                        Created = DateTime.Now
                    };
                    await PushNotificationService.SendNotifications(recipients, notification);
                }
            }

            _chatImages.Clear();
            _chatVideos.Clear();
            _chatDocuments.Clear();

            _busySending = false;
            StateHasChanged();
        }

        /// <summary>
        /// Converts the content of the specified <see cref="IBrowserFile"/> to a Base64-encoded string.
        /// </summary>
        /// <remarks>The method reads the file content up to a maximum size of 5 MB and encodes it as a
        /// Base64 string. The resulting string is formatted as "data:[MIME type];base64,[Base64 content]".</remarks>
        /// <param name="state">The <see cref="IBrowserFile"/> representing the file to be encoded. Must not be null.</param>
        /// <returns>A Base64-encoded string representing the file's content, prefixed with the file's MIME type.</returns>
        private async Task<string> GetBase64String(IBrowserFile state)
        {
            long maxFileSize = 1024 * 1024 * 5;

            var readStream = state.OpenReadStream(maxFileSize);
            var buf = new byte[readStream.Length];
            var ms = new MemoryStream(buf);

            await readStream.CopyToAsync(ms);

            var buffer = ms.ToArray();

            return $"data:{state.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets a badge color representing the user's online status.
        /// </summary>
        /// <param name="isOnline">Whether the user is online.</param>
        /// <returns>A color representing the user's status.</returns>
        private Color GetUserStatusBadgeColor(bool isOnline)
        {
            return isOnline ? Color.Success : Color.Error;
        }

        #endregion

        #region Upload Implementations (Image/Document/Video)

        /// <summary>
        /// Uploads an image file asynchronously to the server.
        /// </summary>
        /// <remarks>The method initializes the upload process, streams the file for upload, and handles
        /// any exceptions  that occur during the operation. If the cancellation token is triggered, the upload will be
        /// aborted.</remarks>
        /// <param name="tuple">A tuple containing the image file to upload and an associated string value.  The first item is an <see
        /// cref="IBrowserFile"/> representing the file to be uploaded,  and the second item is a string used to provide
        /// additional context for the upload.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests during the upload process.</param>
        private async ValueTask UploadImageFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
            if (tuple?.Item1 == null) return;

            var result = InitialiseUploadResult(tuple.Item1.Size);
            try
            {
                await using var stream = tuple.Item1.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteImageUploadAsync(stream, tuple.Item1.Name, tuple.Item1.ContentType, UploadType.Slider, result, tuple.Item2, ct);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                Snackbar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Uploads an image asynchronously to the server and associates it with a chat message.
        /// </summary>
        /// <remarks>This method uploads an image to the server using a multipart form-data request. The
        /// progress of the upload is tracked and updated in the <paramref name="uiResult"/> parameter. Upon successful
        /// upload, the image is associated with the specified chat message. If the upload or association fails,
        /// appropriate error messages are displayed.</remarks>
        /// <param name="stream">The <see cref="Stream"/> containing the image data to upload. Must be readable and not null.</param>
        /// <param name="fileName">The name of the file being uploaded. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the image (e.g., "image/jpeg"). Cannot be null or empty.</param>
        /// <param name="uploadType">The type of upload, represented by the <see cref="UploadType"/> enumeration.</param>
        /// <param name="uiResult">An object representing the upload progress and result. Cannot be null.</param>
        /// <param name="messageId">The identifier of the chat message to associate the uploaded image with. Cannot be null or empty.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        private async Task ExecuteImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, string messageId, CancellationToken ct)
        {
            using var content = new MultipartFormDataContent();
            if (string.IsNullOrWhiteSpace(contentType))
                contentType = MediaTypeNames.Application.Octet;

            var progressContent = new ProgressableStreamContent(stream, bufferSize: 64 * 1024, progress => uiResult.Progress = (int)(progress * 100 / uiResult.TotalBytes));
            if (!string.IsNullOrWhiteSpace(contentType) && MediaTypeHeaderValue.TryParse(contentType, out var parsedContentType))
                progressContent.Headers.ContentType = parsedContentType;

            // Required API contract fields
            content.Add(progressContent, "File", fileName);
            content.Add(new StringContent(fileName), "Name");
            content.Add(new StringContent("false"), "Featured");
            content.Add(new StringContent(string.Empty), "Selector");
            content.Add(new StringContent("0"), "Order");
            content.Add(new StringContent(((int)uploadType).ToString()), "ImageType");

            var response = await Provider.PostingAsync<ImageDto>("images/upload", content);
            if (!response.Succeeded) Snackbar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            else
            {
                _chatImages.Add(response.Data);

                var additionResult = await ChatService.AddImage(new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.Id });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }

        /// <summary>
        /// Uploads a document file asynchronously using the provided file and metadata.
        /// </summary>
        /// <remarks>The method initializes the upload process, streams the file for upload, and handles
        /// any exceptions  that occur during the operation. If the cancellation token is triggered, the operation will
        /// terminate  without processing further.</remarks>
        /// <param name="tuple">A tuple containing the file to upload and an associated metadata string.  The first item is the <see
        /// cref="IBrowserFile"/> representing the file,  and the second item is a string containing additional
        /// metadata.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests during the upload process.</param>
        private async ValueTask UploadDocumentFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
            if (tuple?.Item1 == null) return;

            var result = InitialiseUploadResult(tuple.Item1.Size);
            try
            {
                await using var stream = tuple.Item1.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteDocumentUploadAsync(stream, tuple.Item1.Name, tuple.Item1.ContentType, result, tuple.Item2, ct);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                Snackbar.Add($"Document upload failed: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Uploads a document to the server and associates it with a chat message.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="bullet">
        /// <item><description>Uploads the document to the server using a multipart form-data
        /// request.</description></item> <item><description>Updates the progress of the upload in the <paramref
        /// name="uiResult"/> object.</description></item> <item><description>Associates the uploaded document with the
        /// specified chat message.</description></item> </list> If the upload or association fails, appropriate error
        /// messages are displayed using the application's notification system.</remarks>
        /// <param name="stream">The stream containing the document data to upload. Must be readable and seekable.</param>
        /// <param name="fileName">The name of the file being uploaded. This will be used as the document's name on the server.</param>
        /// <param name="contentType">The MIME type of the document being uploaded (e.g., "application/pdf").</param>
        /// <param name="uiResult">An object used to track the progress and result of the upload operation. The <see
        /// cref="Progress{T}"/> property will be updated during the upload.</param>
        /// <param name="messageId">The identifier of the chat message to which the uploaded document will be associated.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        private async Task ExecuteDocumentUploadAsync(Stream stream, string fileName, string contentType, UploadResult uiResult, string messageId, CancellationToken ct)
        {
            using var content = new MultipartFormDataContent();
            if (string.IsNullOrWhiteSpace(contentType))
                contentType = MediaTypeNames.Application.Octet;

            var progressContent = new ProgressableStreamContent(stream, bufferSize: 64 * 1024, progress => uiResult.Progress = (int)(progress * 100 / uiResult.TotalBytes));
            if (!string.IsNullOrWhiteSpace(contentType) && MediaTypeHeaderValue.TryParse(contentType, out var parsedContentType))
                progressContent.Headers.ContentType = parsedContentType;

            // Required API contract fields
            content.Add(progressContent, "Document", fileName);
            content.Add(new StringContent(fileName), "Name");
            content.Add(new StringContent("false"), "Featured");

            var response = await Provider.PostingAsync<DocumentUploadResponse>("documents/upload", content);
            if (!response.Succeeded) Snackbar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            else
            {
                _chatDocuments.Add(new DocumentDto(response.Data));

                var additionResult = await ChatService.AddDocument(new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.DocumentId });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }

        /// <summary>
        /// Uploads a video file asynchronously using the provided file and associated metadata.
        /// </summary>
        /// <remarks>The method initializes the upload process, streams the video file, and executes the
        /// upload operation. If the upload fails and the cancellation token has not been triggered, an error message is
        /// displayed using a snackbar notification.</remarks>
        /// <param name="tuple">A tuple containing the video file to upload and a string representing additional metadata. The first item is
        /// an <see cref="IBrowserFile"/> representing the video file, and the second item is a string containing
        /// metadata associated with the upload.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests during the upload process.</param>
        private async ValueTask UploadVideoFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
            if (tuple?.Item1 == null) return;

            var result = InitialiseUploadResult(tuple.Item1.Size);
            try
            {
                await using var stream = tuple.Item1.OpenReadStream(maxAllowedSize: 500 * 1024 * 1024, ct);
                await ExecuteVideoUploadAsync(stream, tuple.Item1.Name, tuple.Item1.ContentType, result, tuple.Item2, ct);
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                Snackbar.Add($"Upload failed: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Uploads a video file to the server and associates it with a chat message.
        /// </summary>
        /// <remarks>This method uploads a video file to the server using a multipart form-data request.
        /// The upload progress is tracked and updated in the <paramref name="uiResult"/> object. If the upload
        /// succeeds, the video is associated with the specified chat message. If the upload or association fails, error
        /// messages are displayed in the UI.</remarks>
        /// <param name="stream">The stream containing the video file data to upload. Must be readable and seekable.</param>
        /// <param name="fileName">The name of the video file being uploaded. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the video file (e.g., "video/mp4"). Cannot be null or empty.</param>
        /// <param name="uiResult">An object representing the upload progress and status. This will be updated during the upload process.</param>
        /// <param name="messageId">The identifier of the chat message to associate the uploaded video with. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the upload operation.</param>
        private async Task ExecuteVideoUploadAsync(Stream stream, string fileName, string contentType, UploadResult uiResult, string messageId, CancellationToken ct)
        {
            using var content = new MultipartFormDataContent();
            var progressContent = new ProgressableStreamContent(stream, bufferSize: 64 * 1024, progress => uiResult.Progress = (int)(progress * 100 / uiResult.TotalBytes));
            progressContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Required API contract fields
            content.Add(progressContent, "File", fileName);
            content.Add(new StringContent(fileName), "Name");
            content.Add(new StringContent("false"), "Featured");
            content.Add(new StringContent(string.Empty), "Selector");
            content.Add(new StringContent("0"), "Order");

            var response = await Provider.PostingAsync<VideoDto>("videos/upload", content);
            if (!response.Succeeded) Snackbar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            else
            {
                _chatVideos.Add(response.Data);

                var additionResult = await ChatService.AddDocument(new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.Id });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }

        #endregion

        #region Lifecycle

        /// <summary>
        /// Initializes the chat panel by loading user identity, chat groups, 
        /// connecting to SignalR, and subscribing to message events.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            user = authState.User;

            _currentUserId = user.GetUserId();
            _currentUserName = user.FindFirstValue(ClaimTypes.Name);
            _currentUserDisplayName = user.GetUserDisplayName();

            
            if (!string.IsNullOrEmpty(_currentUserId))
            {
                var result = await ChatGroupService.ChatGroups(_currentUserId);
                if (result.Succeeded)
                {
                    _chatGroups = result.Data.ToList();
                }
            }

            if (string.IsNullOrEmpty(CurrentGroupId))
            {
                CurrentGroupId = _chatGroups.FirstOrDefault()?.Id;
            }

            await LoadGroupMembersAsync(CurrentGroupId);
            await LoadMessagesForGroup(_chatGroups.FirstOrDefault(g => g.Id == CurrentGroupId)!);
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
