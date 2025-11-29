namespace MessagingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Request model used to create or update a chat group and associate it with a list of user IDs.
    /// </summary>
    public class AddUpdateChatGroupRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the chat group.
        /// Defaults to a new GUID if not provided.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the chat group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of user IDs that are members of the chat group.
        /// </summary>
        public List<string> UserIds { get; set; } = new();
    }

}
