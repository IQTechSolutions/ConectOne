using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Messages
{
    /// <summary>
    /// Displays a list of messages for a given user or message type. 
    /// Retrieves and displays messages from the <see cref="IMessagingProvider"/>,
    /// and allows navigation to message details or deletion of messages.
    /// </summary>
    public partial class Public
    {
        private string EventNotificationCount = "0";
        private string ActivityNotificationCount = "0";
        private string _userId = null!;
        private string _userEmail = null!;

        /// <summary>
        /// Provides the current authentication state (for determining the user ID 
        /// or checking roles if not a super user).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Service used for fetching, updating, and deleting messages.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events within a Blazor application. This property is
        /// typically injected by the framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the category.
        /// </summary>
        [Parameter] public string CategoryId { get; set; }

        // Returns a string for the title of the page or section, based on whether a 
        // specific MessageType was provided.
        private string TitleText => $"Activity Group Messages";

        /// <summary>
        /// Represents the parameters used to configure the message page.
        /// </summary>
        /// <remarks>This field is initialized with default values and is intended to be used internally
        /// to store configuration settings for the message page.</remarks>
        private readonly MessagePageParameters _args = new();

        /// <summary>
        /// Represents a private list of messages and events used internally within the application.
        /// </summary>
        /// <remarks>This list is intended for internal use only and stores instances of <see
        /// cref="PublicMessageAndEventViewModel"/>. It is not exposed publicly and should be accessed or modified
        /// through appropriate methods or properties.</remarks>
        private List<PublicMessageAndEventViewModel> _messageList = [];

        /// <summary>
        /// Indicates whether the object has been successfully loaded.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Navigates to the message details page, optionally including a notification ID if relevant.
        /// </summary>
        /// <param name="messageId">The unique ID of the message to view.</param>
        /// <param name="notificationId">An optional notification ID if this message relates to a notification record.</param>
        public void NavigateToMessageDetails(string url)
        {
            // If no notification ID is supplied, navigate to "messages/{messageId}";
            // otherwise, navigate to "notifications/{notificationId}".
            NavigationManager.NavigateTo(url);
        }

        /// <summary>
        /// Navigates to the specified URL using the Blazor router.
        /// </summary>
        /// <remarks>The navigation is performed immediately and forces a reload of the page.</remarks>
        /// <param name="url">The URL to navigate to. This must be a valid, absolute or relative URL.</param>
        private void NavigateToPage(string url)
        {
            // Navigate to the specified URL, which will be handled by the Blazor router.
            NavigationManager.NavigateTo(url, true);
        }

        #region Overrides

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var authState = await AuthenticationStateTask;
                _userId = authState.User.GetUserId();
                _userEmail = authState.User.GetUserEmail();

                _args.EntityId = CategoryId;
                _args.Public = true;
                _args.PageSize = 50;

                // Retrieve messages matching our filter parameters.
                var messageListResult = await Provider.GetPagedAsync<MessageDto, MessagePageParameters>("messages/paged", _args);
                messageListResult.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    _messageList.AddRange(messageListResult.Data.Select(c => new PublicMessageAndEventViewModel(c.CreatedTime.Value, c.Subject, c.ShortDescription, $"messages/{c.MessageId}"))); 
                });

                var eventResult = await Provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents", new SchoolEventPageParameters() { PageSize = 50, CategoryId = CategoryId});
                    eventResult.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        _messageList.AddRange(eventResult.Data.Select(c => new PublicMessageAndEventViewModel(c.StartDate, c.Name, $"Details for {c.Name}", $"/events/{c.EventId}")));
                    });

                    // 8) Event
                    var eventNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.Event, ReceiverId = _userId };
                    var eventNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{eventNotificationCountArgs.GetQueryString()}");
                    if (eventNotificationCountResult.Succeeded)
                        EventNotificationCount = eventNotificationCountResult.Data.ToString();

                    // 9) ActivityGroup (and ActivityCategory)
                    //    Merge counts for ActivityGroup and ActivityCategory into one total.
                    var activityNotificationCountArgs = new NotificationPageParameters { MessageType = MessageType.ActivityGroup, ReceiverId = _userId };
                    var activityNotificationCountResult = await Provider.GetAsync<int>($"notifications/unread/count?{activityNotificationCountArgs.GetQueryString()}");
                    var activityNotificationCount = 0;
                    if (activityNotificationCountResult.Succeeded)
                    {
                        activityNotificationCount = activityNotificationCountResult.Data;
                        ActivityNotificationCount = activityNotificationCountResult.Data.ToString();
                    }

                    if(_messageList.Any())
                        _messageList = _messageList.OrderBy(c => c.DateTime).ToList();

                _loaded = true;
                StateHasChanged();
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a view model for public messages and events, containing details such as date, title, description, and
    /// URL.
    /// </summary>
    /// <remarks>This class is used to encapsulate the information related to public messages and events,
    /// providing properties to access and modify the details.</remarks>
    public class PublicMessageAndEventViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicMessageAndEventViewModel"/> class.
        /// </summary>
        public PublicMessageAndEventViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicMessageAndEventViewModel"/> class with the specified
        /// details.
        /// </summary>
        /// <param name="dateTime">The date and time associated with the message or event.</param>
        /// <param name="title">The title of the message or event. Cannot be null or empty.</param>
        /// <param name="description">The description of the message or event. Cannot be null or empty.</param>
        /// <param name="url">The URL related to the message or event. Can be null if no URL is applicable.</param>
        public PublicMessageAndEventViewModel(DateTime dateTime, string title, string description, string url)
        {
            DateTime = dateTime;
            Title = title;
            Description = description;
            Url = url;
        }

        /// <summary>
        /// Gets or sets the date and time value.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL associated with the current request.
        /// </summary>
        public string Url { get; set; }
    }
}