using PayFast.Base;

namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the result of an ad hoc fetch operation, including the retrieved data and status information.
    /// </summary>
    public class AdhocFetchResult : ApiResultBase
    {
        /// <summary>
        /// Gets or sets the data retrieved by the fetch operation.
        /// </summary>
        public FetchData data { get; set; }
    }
}
