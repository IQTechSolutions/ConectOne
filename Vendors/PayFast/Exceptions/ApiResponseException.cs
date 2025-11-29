namespace PayFast.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an HTTP API response indicates an error or invalid state.
    /// </summary>
    /// <remarks>Use this exception to access the underlying HTTP response details when handling API errors.
    /// The associated HttpResponseMessage provides access to the status code, headers, and content of the failed
    /// response for further analysis or logging.</remarks>
    public class ApiResponseException : Exception
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ApiResponseException class using the specified HTTP response message.
        /// </summary>
        /// <remarks>The provided HttpResponseMessage is available through the HttpResponseMessage
        /// property for further inspection of the response details.</remarks>
        /// <param name="httpResponseMessage">The HTTP response message that caused the exception. Cannot be null.</param>
        public ApiResponseException(HttpResponseMessage httpResponseMessage) : base(message: "Invalid Response. See The HttpResponseMessage Property For Details")
        {
            HttpResponseMessage = httpResponseMessage;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the HTTP response message associated with the current operation.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; }

        #endregion Properties
    }
}
