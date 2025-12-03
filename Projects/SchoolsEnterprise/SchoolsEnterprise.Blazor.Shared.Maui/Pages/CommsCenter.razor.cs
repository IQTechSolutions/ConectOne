using System.Security.Claims;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using NeuralTech.Base.Enums;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using SchoolsEnterprise.Base.Constants;

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
    public partial class CommsCenter
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
        /// Injected service for retrieving unread notification counts from the server.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

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

        #region Overrides

        /// <summary>
        /// Initializes the component, subscribing to notification changes and 
        /// fetching the authenticated user’s ID. Then populates unread counts.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
#if DEBUG
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        // WARNING: This accepts any cert. Use ONLY for local dev.
                        return true;
                    }
                };

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{Configuration["ApiConfiguration:BaseApiAddress"]}/notificationsHub", options => options.HttpMessageHandlerFactory = _ => handler)
                    .WithAutomaticReconnect().Build();
#else
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{Configuration["ApiConfiguration:BaseApiAddress"]}/notificationsHub")
                    .WithAutomaticReconnect().Build();
#endif


                _hubConnection.On<string, string, string, string>(ApplicationConstants.SignalR.SendPushNotification,
                    (type, title, message, url) =>
                    {
                        if (type == ((int)MessageType.Global).ToString())
                        {
                            _globalNotificationCount += 1;
                            StateHasChanged();
                        }
                    });

                await _hubConnection.StartAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           

            var authState = await AuthenticationStateTask;
            _user = authState.User;
            _userId = authState.User.GetUserId();

            await SetMessageCounts();
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
