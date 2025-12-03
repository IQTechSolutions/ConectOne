namespace SchoolsEnterprise.Base.Constants
{
    /// <summary>
    /// Provides application-wide constant values used throughout the application.
    /// </summary>
    /// <remarks>This class contains nested static classes that group related constants, such as those used
    /// for SignalR hub communication. Use these constants to avoid hardcoding string values and to ensure consistency
    /// across the application.</remarks>
    public static class ApplicationConstants
    {
        /// <summary>
        /// Provides constant values for SignalR hub URLs and method names used for client-server communication.
        /// </summary>
        /// <remarks>This class contains string constants representing hub endpoints and method names for
        /// sending and receiving messages via SignalR. Use these constants to ensure consistency when referencing
        /// SignalR hubs and methods throughout the application.</remarks>
        public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SendPushNotification = "SendPushNotification";





            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";

            public const string PingRequest = "PingRequestAsync";
            public const string PingResponse = "PingResponseAsync";
        }
    }
}
