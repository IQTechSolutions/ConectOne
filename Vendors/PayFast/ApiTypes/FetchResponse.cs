namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the response returned from a fetch operation, including the authentication token and the result
    /// status.
    /// </summary>
    public class FetchResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the authentication token used to authorize requests.
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// Gets or sets the result status of the operation.
        /// </summary>
        public ResultStatus status { get; set; }

        #endregion Properties
    }
}
