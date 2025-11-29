using PayFast.Base;

namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the response returned from an update operation, including the updated subscription details.
    /// </summary>
    public class UpdateResponse : ApiResultBase
    {
        /// <summary>
        /// Gets or sets the subscription detail response data returned by the API.
        /// </summary>
        public GenericData<SubscriptionDetailResponse> data { get; set; }
    }
}
