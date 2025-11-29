using PayFast.Base;

namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the result of an ad hoc cancellation operation, including the associated data returned by the API.
    /// </summary>
    public class AdhocCancelResult : ApiResultBase
    {
        /// <summary>
        /// Gets or sets the ad hoc data associated with this instance.
        /// </summary>
        public AdhocData data { get; set; }
    }
}
