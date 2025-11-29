namespace PayFast
{
    /// <summary>
    /// Object we use to represent PayFast payment gateway settings
    /// </summary>
    public class PayFastSettings
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PayFastSettings() { }

        /// <summary>
        /// Constructor with Parameters
        /// </summary>
        /// <param name="merchantId">The supplied merchant Id</param>
        /// <param name="merchantKey">The supplied merchant key</param>
        /// <param name="passPhrase">The supplied pass phrase</param>
        /// <param name="processUrl">The url that is used to process a PayFast transaction</param>
        /// <param name="validateUrl">The url that is used to validate a PayFast transaction</param>
        /// <param name="returnUrl">The url that the application should return to after payment</param>
        /// <param name="cancelUrl">The url used to return to when a transaction is cancelled</param>
        /// <param name="notifyUrl">The url used to notify the application after payment verification</param>
        public PayFastSettings(string merchantId, string merchantKey, string passPhrase, string processUrl, string validateUrl, string returnUrl, string cancelUrl, string notifyUrl, string donationsNotifyUrl, string redirectUrl = null, string donationsRedirectUrl = null)
        {
            MerchantId = merchantId;
            MerchantKey = merchantKey;
            PassPhrase = passPhrase;
            ProcessUrl = processUrl;
            ValidateUrl = validateUrl;
            ReturnUrl = returnUrl;
            CancelUrl = cancelUrl;
            DonationsNotifyUrl = donationsNotifyUrl;
            DonationsReturnUrl = donationsRedirectUrl;
        }

        #region Properties

        /// <summary>
        /// Gets or Sets the supplied merchant Id
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or Sets the supplied merchant key
        /// </summary>
        public string MerchantKey { get; set; }

        /// <summary>
        /// Gets or Sets the supplied pass phrase
        /// </summary>
        public string PassPhrase { get; set; }

        /// <summary>
        /// Gets or Sets the url that is used to process a PayFast transaction
        /// </summary>
        public string ProcessUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url that is used to validate a PayFast transaction
        /// </summary>
        public string ValidateUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url that the application should return to after payment
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url used to return to when a transaction is cancelled
        /// </summary>
        public string CancelUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url used to notify the application after payment verification
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url that clients should be redirected back to after the callback completes
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url used to notify the application after payment verification
        /// </summary>
        public string DonationsNotifyUrl { get; set; }

        /// <summary>
        /// Gets or Sets the url that clients should be redirected back to after the callback completes
        /// </summary>
        public string DonationsReturnUrl { get; set; }

        #endregion
    }
}