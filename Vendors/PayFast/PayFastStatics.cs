namespace PayFast
{
    /// <summary>
    /// Provides static members related to PayFast payment processing, including valid site domains and standard payment
    /// confirmation values.
    /// </summary>
    /// <remarks>This class contains constants and utility values commonly used when integrating with the
    /// PayFast payment gateway. All members are thread-safe and intended for use in PayFast-related
    /// operations.</remarks>
    public static class PayFastStatics
    {
        /// <summary>
        /// Gets the list of valid PayFast site hostnames that can be used for payment processing.
        /// </summary>
        public static string[] ValidSites => new[] { "www.payfast.co.za", "sandbox.payfast.co.za", "w1w.payfast.co.za", "w2w.payfast.co.za" };

        /// <summary>
        /// Represents the status code indicating that a payment confirmation is complete.
        /// </summary>
        public const string CompletePaymentConfirmation = "COMPLETE";

        /// <summary>
        /// Represents the confirmation code used to indicate that a payment has been cancelled.
        /// </summary>
        public const string CancelledPaymentConfirmation = "CANCELLED";
    }
}
