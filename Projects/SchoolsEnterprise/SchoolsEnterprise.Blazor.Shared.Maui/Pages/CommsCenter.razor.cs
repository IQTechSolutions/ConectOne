using Blazored.LocalStorage;
using ConectOne.Domain.Constants;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using MessagingModule.Application.HubServices;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsEnterprise.Base.Constants;
using System.Diagnostics;
using System.Security.Claims;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages
{
    /// <summary>
    /// Provides notification center functionality for a user, including retrieving unread notification counts and
    /// handling notification state changes within the application.
    /// </summary>
    /// <remarks>The CommsCenter class is designed for use in Blazor applications and integrates with
    /// authentication, navigation, and HTTP services via dependency injection. It manages notification counts for
    /// various message types and updates the UI in response to notification state changes. This class is typically used
    /// as a component or service to display and update notification indicators for the authenticated user.</remarks>
    public partial class CommsCenter : IDisposable
    {
        private HubConnection? _hubConnection;
        private string _userId = null!;
        private ClaimsPrincipal _user = null!;
        private int _notificationCount;
        private int _globalNotificationCount;
        private int _gradeNotificationCount;
        private int _classNotificationCount;
        private int _learnerNotificationCount;
        private int _parentNotificationCount;
        private int _teacherNotificationCount;
        private int _eventNotificationCount;
        private int _activityNotificationCount;

        /// <summary>
        /// Cascading parameter for retrieving the authenticated user's identity/claims.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected authentication state provider used to react to sign-in/out events.
        /// </summary>
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        /// <summary>
        /// Injected service for retrieving unread notification counts from the server.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        [Inject] public NotificationHubService NotificationHubService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to interact with the browser's local storage.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use this service to
        /// store, retrieve, or remove data from the browser's local storage in a Blazor application.</remarks>
        [Inject] public ILocalStorageService LocalStorage { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current URI and related navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// control application behavior. This property is typically populated by the dependency injection
        /// system.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #region Navigation

        /// <summary>
        /// Navigates the application to the specified URL.
        /// </summary>
        /// <remarks>The method uses the <see cref="NavigationManager"/> to perform the navigation. Ensure
        /// that the URL is properly formatted and accessible.</remarks>
        /// <param name="url">The URL to navigate to. This must be a valid, non-null, and non-empty string.</param>
        public void NavigateToPage(string url)
        {
            NavigationManager.NavigateTo(url);
        }

        #endregion

        /// <summary>
        /// Fetches and sets the unread notification counts for each MessageType, 
        /// populating them as strings for the UI.
        /// </summary>
        private async Task SetMessageCounts()
        {
            // 1) Total unread notifications for the user
            var notificationCountArgs = new NotificationPageParameters { ReceiverId = _userId };
            var notificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{notificationCountArgs.GetQueryString()}");
            if (notificationCountResult.Succeeded)
                _notificationCount = notificationCountResult.Data;

            // 2) Global
            var globalNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Global, ReceiverId = _userId };
            var globalNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{globalNotificationCountArgs.GetQueryString()}");
            if (globalNotificationCountResult.Succeeded)
                _globalNotificationCount = globalNotificationCountResult.Data;

            // 3) Grade
            var gradeNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Grade, ReceiverId = _userId };
            var gradeNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{gradeNotificationCountArgs.GetQueryString()}");
            if (gradeNotificationCountResult.Succeeded)
                _gradeNotificationCount = gradeNotificationCountResult.Data;

            // 4) SchoolClass
            var schoolClassNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.SchoolClass, ReceiverId = _userId };
            var schoolClassNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{schoolClassNotificationCountArgs.GetQueryString()}");
            if (schoolClassNotificationCountResult.Succeeded)
                _classNotificationCount = schoolClassNotificationCountResult.Data;

            // 5) Learner
            var learnerNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Learner, ReceiverId = _userId };
            var learnerNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{learnerNotificationCountArgs.GetQueryString()}");
            if (learnerNotificationCountResult.Succeeded)
                _learnerNotificationCount = learnerNotificationCountResult.Data;

            // 6) Parent
            var parentNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Parent, ReceiverId = _userId };
            var parentNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{parentNotificationCountArgs.GetQueryString()}");
            if (parentNotificationCountResult.Succeeded)
                _parentNotificationCount = parentNotificationCountResult.Data;

            // 7) Teacher
            var teacherNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Teacher, ReceiverId = _userId };
            var teacherNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{teacherNotificationCountArgs.GetQueryString()}");
            if (teacherNotificationCountResult.Succeeded)
                _teacherNotificationCount = teacherNotificationCountResult.Data;

            // 8) Event
            var eventNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Event, ReceiverId = _userId };
            var eventNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{eventNotificationCountArgs.GetQueryString()}");
            if (eventNotificationCountResult.Succeeded)
                _eventNotificationCount = eventNotificationCountResult.Data;

            // 9) ActivityGroup (and ActivityCategory)
            //    Merge counts for ActivityGroup and ActivityCategory into one total.
            var activityNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.ActivityGroup, ReceiverId = _userId };
            var activityNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{activityNotificationCountArgs.GetQueryString()}");
            var activityNotificationCount = 0;
            if (activityNotificationCountResult.Succeeded)
            {
                activityNotificationCount = activityNotificationCountResult.Data;
                _activityNotificationCount = activityNotificationCountResult.Data;
            }

            var activityGroupNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.ActivityCategory, ReceiverId = _userId };
            var activityGroupNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{activityGroupNotificationCountArgs.GetQueryString()}");
            if (activityGroupNotificationCountResult.Succeeded)
            {
                // Combine the two counts
                _activityNotificationCount = (activityNotificationCount + activityGroupNotificationCountResult.Data);
            }
        }

        /// <summary>
        /// Initializes the component, subscribing to notification changes and 
        /// fetching the authenticated user’s ID. Then populates unread counts.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userId = _user.GetUserId();

            await NotificationHubService.InitializeAsync();
            NotificationHubService.OnMessageReceived += OnMessageReceived;
            NotificationHubService.OnMessageRead += OnMessageRead;

            await SetMessageCounts();

            await base.OnInitializedAsync();

        }

        /// <summary>
        /// Handles the logic required when a notification message is marked as read, updating the relevant notification
        /// counts based on the message type.
        /// </summary>
        /// <remarks>This method decrements the overall and type-specific notification counters to reflect
        /// that a message has been read. It should be called whenever a notification is marked as read to ensure
        /// notification counts remain accurate.</remarks>
        /// <param name="type">The type of the notification message, represented as a string value corresponding to a member of the
        /// MessageType enumeration.</param>
        /// <param name="notificationId">The unique identifier of the notification message that has been read.</param>
        private void OnMessageRead(string type, string notificationId)
        {
            _notificationCount -= 1;
            if (type == ((int)MessageType.Global).ToString()) _globalNotificationCount -= 1;
            if (type == ((int)MessageType.Grade).ToString()) _gradeNotificationCount -= 1;
            if (type == ((int)MessageType.SchoolClass).ToString()) _classNotificationCount -= 1;
            if (type == ((int)MessageType.Learner).ToString()) _learnerNotificationCount -= 1;
            if (type == ((int)MessageType.Parent).ToString()) _parentNotificationCount -= 1;
            if (type == ((int)MessageType.Teacher).ToString()) _teacherNotificationCount -= 1;
            if (type == ((int)MessageType.Event).ToString()) _eventNotificationCount -= 1;
            if (type == ((int)MessageType.ActivityGroup).ToString()) _activityNotificationCount -= 1;
            if (type == ((int)MessageType.ActivityCategory).ToString()) _activityNotificationCount -= 1;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles an incoming message notification and updates the corresponding notification counters based on the
        /// message type.
        /// </summary>
        /// <remarks>This method increments the total notification count and updates specific counters for
        /// each message type. It also triggers a UI update to reflect the new notification state.</remarks>
        /// <param name="type">A string representing the type of the message. Must correspond to a valid value of the MessageType
        /// enumeration.</param>
        /// <param name="title">The title of the message to be processed.</param>
        /// <param name="message">The content or body of the message.</param>
        /// <param name="url">An optional URL associated with the message. May be null or empty if not applicable.</param>
        private void OnMessageReceived(string type, string title, string message, string url)
        {
            _notificationCount += 1;
            if (type == ((int)MessageType.Global).ToString()) _globalNotificationCount += 1;
            if (type == ((int)MessageType.Grade).ToString()) _gradeNotificationCount += 1;
            if (type == ((int)MessageType.SchoolClass).ToString()) _classNotificationCount += 1;
            if (type == ((int)MessageType.Learner).ToString()) _learnerNotificationCount += 1;
            if (type == ((int)MessageType.Parent).ToString()) _parentNotificationCount += 1;
            if (type == ((int)MessageType.Teacher).ToString()) _teacherNotificationCount += 1;
            if (type == ((int)MessageType.Event).ToString()) _eventNotificationCount += 1;
            if (type == ((int)MessageType.ActivityGroup).ToString()) _activityNotificationCount += 1;
            if (type == ((int)MessageType.ActivityCategory).ToString()) _activityNotificationCount += 1;

            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            NotificationHubService.OnMessageReceived -= OnMessageReceived;
            NotificationHubService.OnMessageRead -= OnMessageRead;
        }
    }
}
