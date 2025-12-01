using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Application.Services;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.RequestFeatures;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using MessagingModule.Application.ViewModels;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Radzen;
using System.Net.Http.Headers;
using System.Security.Claims;
using static IdentityModule.Domain.Constants.Permissions;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Chats
{
    /// <summary>
    /// Represents a chat panel component that provides real-time messaging functionality, user presence tracking, and
    /// file upload capabilities for a specific chat group.
    /// </summary>
    /// <remarks>This component is designed to manage chat interactions within a group, including sending and
    /// receiving messages, tracking user presence, and handling file uploads (images, videos, and documents). It
    /// integrates with SignalR for real-time updates and supports asynchronous operations for improved responsiveness. 
    /// <para> Key features include: <list type="bullet"> <item>Real-time messaging with SignalR integration.</item>
    /// <item>File upload support for images, videos, and documents.</item> <item>User presence tracking and
    /// notifications.</item> <item>Read receipt management for messages.</item> </list> </para> This component is
    /// typically used in Blazor applications and relies on dependency injection for services such as <see
    /// cref="ChatSignalRService"/>, <see cref="IBaseHttpProvider"/>, and <see cref="ISnackbar"/>.</remarks>
    public partial class ChatPanel 
    {
        private bool _loaded;
        private string _currentUserId;
        private string? _currentUserName;
        private string _currentMessage;
        private string? _currentUserDisplayName;
        private bool _busySending;
        private ClaimsPrincipal user;
        private List<ChatMessageDto> _messages = new();
        private List<UserInfoDto> _chatGroupMembers = new();

        private readonly HashSet<string> _onlineUsers = new();
        private readonly Dictionary<string, DateTime?> _messageReadTimes = new();

        private bool _uploadInProgress;
        private readonly ICollection<UploadResult> _objectsBusyUploading = [];
        private List<ImageDto> _chatImages = [];
        private List<VideoDto> _chatVideos = [];
        private List<DocumentDto> _chatDocuments = [];
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

        /// <summary>
        /// Gets or sets the task that represents the asynchronous operation to retrieve the current authentication
        /// state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state asynchronously. The value is provided as a cascading parameter and should not be set manually in most
        /// scenarios.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts in the user interface. Ensure that the service is properly configured and injected before
        /// use.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the service used to send push notifications.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and query the current URI within the
        /// application.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and for
        /// retrieving information about the current navigation state. This property is typically set by the framework
        /// through dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the group.
        /// </summary>
        [Parameter] public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group associated with this parameter.
        /// </summary>
        [Parameter] public string GroupName { get; set; }        

        /// <summary>
        /// Determines the alignment of a chat message based on the sender.
        /// </summary>
        /// <param name="msg">The chat message to evaluate.</param>
        /// <returns><see cref="Justify.FlexEnd"/> if the message was sent by the current user;  otherwise, <see
        /// cref="Justify.FlexStart"/>.</returns>
        private Justify GetMessageAlignment(ChatMessageDto msg) => msg.SenderId == _currentUserId ? Justify.FlexEnd : Justify.FlexStart;

        /// <summary>
        /// Determines the CSS class to apply to a chat message based on the sender.
        /// </summary>
        /// <param name="msg">The chat message for which the CSS class is determined.</param>
        /// <returns>A string representing the CSS class. Returns <see langword="ml-auto bg-green-100"/> if the message  is sent
        /// by the current user; otherwise, returns <see langword="mr-auto bg-white"/>.</returns>
        private string GetMessageClass(ChatMessageDto msg) => msg.SenderId == _currentUserId ? "ml-auto bg-green-100" : "mr-auto bg-white";

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
        /// Navigates asynchronously to the chat groups page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/chatgroups" route and forces a reload of the
        /// page.</remarks>
        /// <returns></returns>
        private async Task ShowGroupsAsync()
        {
            await Task.Delay(0);
            NavigationManager.NavigateTo("/chatgroups", true);
        }

        /// <summary>
        /// Navigates to the chat members page for the specified group.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation before navigating to the chat members
        /// page. The navigation is forced to reload the page.</remarks>
        /// <returns></returns>
        private async Task ShowMembersAsync()
        {
            await Task.Delay(0);
            NavigationManager.NavigateTo($"/chatmembers/{GroupId}", true);
        }

        /// <summary>
        /// Asynchronously uploads the attachments associated with a message to the specified entity.
        /// </summary>
        /// <remarks>This method processes the attachments in parallel, including images, documents, and
        /// videos.  If an upload is already in progress, the method will return immediately without performing any
        /// action.</remarks>
        /// <param name="message">The message containing the attachments to be uploaded.</param>
        /// <param name="entityId">The identifier of the entity to which the attachments will be uploaded.</param>
        /// <returns></returns>
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
        /// Initializes a new <see cref="UploadResult"/> instance with the specified total bytes and adds it to the
        /// collection of objects currently being uploaded.
        /// </summary>
        /// <remarks>The created <see cref="UploadResult"/> instance is automatically added to the
        /// internal collection of objects being uploaded.</remarks>
        /// <param name="totalBytes">The total number of bytes to be uploaded. Must be a non-negative value.</param>
        /// <returns>A new <see cref="UploadResult"/> instance with its <see cref="UploadResult.Progress"/> set to 0 and <see
        /// cref="UploadResult.TotalBytes"/> set to the specified value.</returns>
        private UploadResult InitialiseUploadResult(long totalBytes)
        {
            var ui = new UploadResult { Progress = 0, TotalBytes = totalBytes };
            _objectsBusyUploading.Add(ui);
            return ui;
        }

        /// <summary>
        /// Sends a chat message asynchronously to the specified group, including any attached images or files.
        /// </summary>
        /// <remarks>This method creates a new chat message, assigns it a unique identifier, and sends it
        /// to the chat group specified by the <c>GroupId</c> property. The message includes the sender's details, the
        /// message content, and any attached images or files.  The method also updates the local message list and
        /// notifies connected clients via SignalR. If the operation fails, an error is logged or propagated. 
        /// Preconditions: - The <c>_currentUserName</c> and <c>GroupId</c> properties must not be null or empty. - The
        /// <paramref name="message"/> parameter must contain valid data.  Postconditions: - The message is sent to the
        /// server and added to the local message list. - Connected clients in the group are notified of the new
        /// message.</remarks>
        /// <param name="message">The message to send, including its content, images, and files. The <see cref="MessageInputViewModel"/> must
        /// not be null, and its <see cref="MessageInputViewModel.Message"/> property must not be empty.</param>
        /// <returns></returns>
        private async Task SendMessageAsync(MessageInputViewModel message)
        {
            _busySending = true;

            if (!string.IsNullOrEmpty(_currentUserName) && !string.IsNullOrEmpty(GroupId))
            {
                var guid = Guid.NewGuid().ToString();
                var chatMessage = new ChatMessageDto()
                {
                    Id = guid,
                    SenderId = _currentUserId,
                    SenderUserName = _currentUserName,
                    SenderDisplayName = user.GetUserDisplayName(),
                    ChatGroupId = GroupId,
                    Content = message.Message,
                    Timestamp = DateTime.Now
                };

                var result = await Provider.PutAsync("chats", chatMessage);
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
                    Images = _chatImages.Select(c => $"{c.RelativePath}").ToList(),
                    Documents = _chatDocuments.Select(c => $"{c.RelativePath}").ToList(),
                    Videos = _chatVideos.Select(c => $"{c.RelativePath}").ToList()
                };

                _messages.Add(newChatMessage);
                _messageReadTimes[guid] = null;

               
                var offlineMembers = _chatGroupMembers.Where(m => m.UserId != _currentUserId && !_onlineUsers.Contains(m.UserId));
                if (offlineMembers.Any())
                {
                    var recipients = offlineMembers.Select(m => new RecipientDto(m.UserId, m.FirstName ?? string.Empty, m.LastName ?? string.Empty, new List<string>(), true, false, m.CoverImageUrl, null)).ToList();
                    var notification = new NotificationDto
                    {
                        Title = $"New message from {_currentUserDisplayName}",
                        ShortDescription = message.Message,
                        Message = message.Message,
                        NotificationUrl = $"/chats/{GroupId}/{GroupName}",
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
        /// <returns></returns>
        private async ValueTask UploadImageFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
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
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task ExecuteImageUploadAsync(Stream stream, string fileName, string contentType, UploadType uploadType, UploadResult uiResult, string messageId, CancellationToken ct)
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
            content.Add(new StringContent(((int)uploadType).ToString()), "ImageType");

            var response = await Provider.PostingAsync<ImageDto>("images/upload", content);
            if (!response.Succeeded) Snackbar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            else
            {
                _chatImages.Add(response.Data);

                var additionResult = await Provider.PostAsync("chats/addImage", new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.Id });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }

        /// <summary>
        /// Uploads a document file asynchronously using the provided file and associated metadata.
        /// </summary>
        /// <remarks>The method initializes the upload process, streams the file for upload, and handles
        /// any exceptions that occur  during the operation. If the cancellation token is triggered, the operation will
        /// terminate without throwing an exception.</remarks>
        /// <param name="tuple">A tuple containing the file to upload and a string representing additional metadata.  The first item is an
        /// <see cref="IBrowserFile"/> representing the file, and the second item is a string for metadata.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests during the upload process.</param>
        /// <returns></returns>
        private async ValueTask UploadDocumentFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
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
        /// <remarks>This method uploads a document to the server using a multipart form-data request.
        /// During the upload, the progress is updated in the <paramref name="uiResult"/> object. After the upload
        /// completes successfully, the document is associated with the specified chat message.</remarks>
        /// <param name="stream">The <see cref="Stream"/> containing the document data to upload. Must be readable and seekable.</param>
        /// <param name="fileName">The name of the file being uploaded. This will be used as the document's name on the server.</param>
        /// <param name="contentType">The MIME type of the document being uploaded (e.g., "application/pdf").</param>
        /// <param name="uiResult">An object representing the upload progress and result. The <see cref="UploadResult.Progress"/> property will
        /// be updated during the upload.</param>
        /// <param name="messageId">The identifier of the chat message to which the uploaded document will be associated.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        private async Task ExecuteDocumentUploadAsync(Stream stream, string fileName, string contentType, UploadResult uiResult, string messageId, CancellationToken ct)
        {
            using var content = new MultipartFormDataContent();
            var progressContent = new ProgressableStreamContent(stream, bufferSize: 64 * 1024, progress => uiResult.Progress = (int)(progress * 100 / uiResult.TotalBytes));
            progressContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Required API contract fields
            content.Add(progressContent, "Document", fileName);
            content.Add(new StringContent(fileName), "Name");
            content.Add(new StringContent("false"), "Featured");
            content.Add(new StringContent(string.Empty), "Selector");
            content.Add(new StringContent("0"), "Order");

            var response = await Provider.PostingAsync<DocumentDto>("documents/upload", content);
            if (!response.Succeeded) Snackbar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);
            else
            {
                _chatDocuments.Add(response.Data);

                var additionResult = await Provider.PostAsync("chats/addDocument", new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.Id });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }

        /// <summary>
        /// Uploads a video file asynchronously using the provided file and associated metadata.
        /// </summary>
        /// <remarks>The method initializes the upload process, streams the video file, and executes the
        /// upload operation. If the upload fails and the operation is not canceled, an error message is displayed using
        /// a snackbar notification.</remarks>
        /// <param name="tuple">A tuple containing the video file to upload and a string representing additional metadata. The first item is
        /// an <see cref="IBrowserFile"/> representing the video file, and the second item is a string containing
        /// metadata associated with the upload.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests during the upload process.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
        private async ValueTask UploadVideoFileAsync(Tuple<IBrowserFile, string> tuple, CancellationToken ct)
        {
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
        /// Uploads a video file to the server and associates it with a chat message.
        /// </summary>
        /// <remarks>This method uploads a video file to the server using a multipart form-data request.
        /// The upload progress is tracked and updated in the <paramref name="uiResult"/> object. Upon successful
        /// upload, the video is associated with the specified chat message. <para> If the upload fails, an error
        /// message is displayed in the user interface. If the association with the chat message fails, the
        /// corresponding error messages are also displayed. </para></remarks>
        /// <param name="stream">The stream containing the video file data to upload. Must be readable and seekable.</param>
        /// <param name="fileName">The name of the video file being uploaded. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the video file (e.g., "video/mp4"). Cannot be null or empty.</param>
        /// <param name="uiResult">An object representing the upload progress and result. Cannot be null.</param>
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

                var additionResult = await Provider.PostAsync("chats/addDocument", new AddEntityImageRequest() { EntityId = messageId, ImageId = response.Data.Id });
                if (!additionResult.Succeeded) Snackbar.AddErrors(additionResult.Messages);
            }
        }
        
        #endregion

        #region Lifecycle
        
        /// <summary>
        /// 
        /// </summary>
        private void WireSignalRCallbacks()
        {
            
        }

        /// <summary>
        /// Asynchronously initializes the component, setting up the current user's information,  retrieving chat
        /// messages for the specified group, and starting the SignalR service for real-time updates.
        /// </summary>
        /// <remarks>This method retrieves the current user's ID, username, and display name from the
        /// authentication state.  It then fetches the chat messages for the specified group and processes the response.
        /// The SignalR service is started to enable real-time message updates, and an event handler is registered  to
        /// handle incoming messages from other users. The component's state is updated as necessary.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            user = authState.User;
            _currentUserId = authState.User.GetUserId();
            _currentUserName = authState.User.Identity.Name;
            _currentUserDisplayName = authState.User.GetUserDisplayName();

            WireSignalRCallbacks();

            var result = await Provider.GetAsync<IEnumerable<ChatMessageDto>>($"chats/messages/{GroupId}");
            result.ProcessResponseForDisplay(Snackbar, () =>
            {
                _messages = result.Data.ToList();
            });

            var memberResult = await Provider.GetAsync<IEnumerable<UserInfoDto>>($"chats/groups/members/{GroupId}");
            if (memberResult.Succeeded && memberResult.Data != null)
            {
                _chatGroupMembers = memberResult.Data.ToList();
            }

            if(string.IsNullOrEmpty(GroupName))
            {
                var groupResult = await Provider.GetAsync<ChatGroupDto>($"chats/groups/{GroupId}");
                if (groupResult.Succeeded && groupResult.Data != null)
                {
                    GroupName = groupResult.Data.Name;
                }
            }

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
