namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents ad hoc data containing a response and an associated message.
    /// </summary>
    public class AdhocData
    {
        /// <summary>
        /// Gets or sets the response message content.
        /// </summary>
        public string response { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        public string message { get; set; }
    }
}
