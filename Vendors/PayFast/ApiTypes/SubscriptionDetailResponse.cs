namespace PayFast.ApiTypes
{
    /// <summary>
    /// Represents the details of a subscription, including its status, billing information, and schedule.
    /// </summary>
    public class SubscriptionDetailResponse
    {
        /// <summary>
        /// Gets or sets the authentication token used to authorize requests.
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// Gets or sets the amount associated with the current instance.
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// Gets or sets the number of cycles to perform.
        /// </summary>
        public int cycles { get; set; }

        /// <summary>
        /// Gets or sets the number of completed cycles.
        /// </summary>
        public int cycles_complete { get; set; }

        /// <summary>
        /// Gets or sets the billing frequency for the associated plan or subscription.
        /// </summary>
        public PayfastBillingFrequency frequency { get; set; }

        /// <summary>
        /// Gets or sets the result status of the operation.
        /// </summary>
        public ResultStatus status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the operation was run.
        /// </summary>
        public DateTime run_date { get; set; }
    }
}
