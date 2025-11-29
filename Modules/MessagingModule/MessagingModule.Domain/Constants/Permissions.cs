using System.ComponentModel;

namespace MessagingModule.Domain.Constants;

/// <summary>
/// Represents a collection of permission constants organized by domain, such as messages and chats.
/// </summary>
/// <remarks>This class provides nested static classes that define string constants representing specific
/// permissions  for various operations within an application. Each nested class corresponds to a domain (e.g.,
/// messages, chats)  and contains constants for actions such as viewing, creating, editing, deleting, and searching. 
/// These constants can be used to enforce or check permissions in role-based access control (RBAC) systems.</remarks>
public class Permissions
{
    /// <summary>
    /// Provides a collection of constants representing permission keys for message-related operations.
    /// </summary>
    /// <remarks>This class defines string constants that represent specific permissions for viewing,
    /// creating, editing, deleting, and searching messages.  These constants can be used to enforce or check
    /// permissions in an application.</remarks>
    [DisplayName("Messages"), Description("Message Permissions")]
    public static class MessagePermissions
    {
        public const string View = "Permissions.Messages.View";
        public const string Create = "Permissions.Messages.Create";
        public const string Edit = "Permissions.Messages.Edit";
        public const string Delete = "Permissions.Messages.Delete";
        public const string Search = "Permissions.Messages.Search";
    }

    /// <summary>
    /// Provides a collection of permission constants related to chat operations.
    /// </summary>
    /// <remarks>This class defines string constants representing various permissions for managing chats, 
    /// such as viewing, creating, editing, deleting, and searching chats. These constants can  be used to enforce or
    /// check permissions in an application.</remarks>
    [DisplayName("Chats"), Description("Chats")]
    public static class ChatPermissions
    {
        public const string View = "Permissions.Chats.View";
        public const string Create = "Permissions.Chats.Create";
        public const string Edit = "Permissions.Chats.Edit";
        public const string Delete = "Permissions.Chats.Delete";
        public const string Search = "Permissions.Chats.Search";
    }
}
