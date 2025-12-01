using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MessagingModule.Blazor.NotificationDelegates
{
    /// <summary>
    /// Represents a message that is sent when a push notification is received.
    /// </summary>
    /// <param name="value">The payload of the received push notification. Cannot be null.</param>
    public class PushNotificationReceived(string value) : ValueChangedMessage<string>(value);

    /// <summary>
    /// Represents a message indicating that a push notification has been sent, carrying the notification payload.
    /// </summary>
    /// <param name="value">The payload of the push notification that was sent. Cannot be null.</param>
    public class PushNotificationSent(string value) : ValueChangedMessage<string>(value);
}
