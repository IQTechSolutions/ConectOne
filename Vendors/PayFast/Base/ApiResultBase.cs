namespace PayFast.Base
{
    /// <summary>
    /// Represents the base result returned by an API operation, including status and code information.
    /// </summary>
    public class ApiResultBase
    {
        /// <summary>
        /// Gets or sets the code associated with this instance.
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Gets or sets the current status message.
        /// </summary>
        public string status { get; set; }
    }
}
