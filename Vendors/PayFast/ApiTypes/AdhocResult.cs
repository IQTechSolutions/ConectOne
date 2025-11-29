using PayFast.Base;

namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the result of an ad hoc API operation, including the associated data payload.
    /// </summary>
    public class AdhocResult : ApiResultBase
    {
        /// <summary>
        /// Gets or sets the ad hoc data associated with the current instance.
        /// </summary>
        public AdhocData data { get; set; }
    }
}
