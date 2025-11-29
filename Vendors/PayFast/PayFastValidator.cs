using System.Net;

namespace PayFast
{
    /// <summary>
    /// Provides methods for validating PayFast payment notifications, merchant credentials, and request source IP
    /// addresses.
    /// </summary>
    /// <remarks>This class is typically used to ensure that incoming PayFast notifications are authentic and
    /// originate from valid sources. It encapsulates the necessary settings, notification data, and request IP address
    /// required for validation. Thread safety is not guaranteed; create a new instance per request if used in a
    /// multi-threaded environment.</remarks>
    public class PayFastValidator
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PayFastValidator class with the specified settings, notification data, and
        /// request IP address.
        /// </summary>
        /// <param name="settings">The PayFastSettings instance containing configuration values required for validation. Cannot be null.</param>
        /// <param name="notify">The PayFastNotify instance representing the notification data to be validated. Cannot be null.</param>
        /// <param name="requestIpAddress">The IP address of the incoming request to be used during validation. Cannot be null.</param>
        public PayFastValidator(PayFastSettings settings, PayFastNotify notify, IPAddress requestIpAddress)
        {
            Settings = settings;
            Notify = notify;
            RequestIpAddress = requestIpAddress;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the PayFast configuration settings used by this instance.
        /// </summary>
        public PayFastSettings Settings { get; private set; }

        /// <summary>
        /// Gets the PayFast notification data associated with the current request.
        /// </summary>
        public PayFastNotify Notify { get; private set; }

        /// <summary>
        /// Gets the IP address from which the current request originated.
        /// </summary>
        public IPAddress RequestIpAddress { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Validates the current merchant identifier and determines whether it meets all required criteria.
        /// </summary>
        /// <returns>true if the merchant identifier is valid; otherwise, false.</returns>
        public bool ValidateMerchantId()
        {
            return ValidateMerchantIdWithChecks();
        }

        #if NETFULL

        /// <summary>
        /// Validates whether the current request's source IP address is allowed according to the configured access
        /// rules.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the source
        /// IP address is permitted; otherwise, <see langword="false"/>.</returns>
        public bool ValidateSourceIp()
        {
            return this.ValidateSourceIpAddress(ipAddress: this.RequestIpAddress);
        }
        #else
        /// <summary>
        /// Validates whether the current request's source IP address is allowed according to the configured access
        /// rules.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the source
        /// IP address is permitted; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> ValidateSourceIp()
        {
            return await ValidateSourceIpAddressAsync(ipAddress: RequestIpAddress);
        }
        #endif

        /// <summary>
        /// Validates the current data set asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the data is
        /// valid; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> ValidateData()
        {
            return await ValidateDataAsync(Notify);
        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Validates that the merchant ID in the notification matches the configured merchant ID.
        /// </summary>
        /// <returns>true if the merchant ID in the notification is equal to the configured merchant ID; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the Settings or Notify property is null.</exception>
        private bool ValidateMerchantIdWithChecks()
        {
            if (Settings == null)
            {
                throw new ArgumentNullException(nameof(Settings));
            }

            if (Notify == null)
            {
                throw new ArgumentNullException(nameof(Notify));
            }

            return Notify.merchant_id == Settings.MerchantId;
        }

        #if NETFULL

        /// <summary>
        /// Asynchronously determines whether the specified IP address matches any of the IP addresses resolved from a
        /// predefined list of valid hostnames.
        /// </summary>
        /// <remarks>This method performs DNS resolution for each valid site hostname and compares the
        /// specified IP address to the resolved addresses. The operation may incur network latency depending on DNS
        /// response times.</remarks>
        /// <param name="ipAddress">The IP address to validate against the set of valid site host addresses. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the IP
        /// address is found in the set of valid site addresses; otherwise, <see langword="false"/>.</returns>
        private bool ValidateSourceIpAddress(IPAddress ipAddress)
        {
            var validIps = new List<IPAddress>();

            for (int i = 0; i < PayFastStatics.ValidSites.Length; i++)
            {
                validIps.AddRange(Dns.GetHostAddresses(PayFastStatics.ValidSites[i]));
            }

            if (validIps.Contains(ipAddress))
                return true;
            else
                return false;
        }
        #else
        /// <summary>
        /// Asynchronously determines whether the specified IP address matches any of the IP addresses resolved from a
        /// predefined list of valid hostnames.
        /// </summary>
        /// <remarks>This method performs DNS resolution for each valid site hostname and compares the
        /// specified IP address to the resolved addresses. The operation may incur network latency depending on DNS
        /// response times.</remarks>
        /// <param name="ipAddress">The IP address to validate against the set of valid site host addresses. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the IP
        /// address is found in the set of valid site addresses; otherwise, <see langword="false"/>.</returns>
        private async Task<bool> ValidateSourceIpAddressAsync(IPAddress ipAddress)
        {
            var validIps = new List<IPAddress>();

            for (int i = 0; i < PayFastStatics.ValidSites.Length; i++)
            {
                validIps.AddRange(await Dns.GetHostAddressesAsync(PayFastStatics.ValidSites[i]));
            }

            if (validIps.Contains(ipAddress))
                return true;
            else
                return false;
        }
        #endif

        /// <summary>
        /// Asynchronously validates the specified PayFast notification data against the remote PayFast validation
        /// service.
        /// </summary>
        /// <param name="notifyViewModel">The PayFast notification data to be validated. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
        /// notification data is valid; otherwise, <see langword="false"/>.</returns>
        private async Task<bool> ValidateDataAsync(PayFastNotify notifyViewModel)
        {
            try
            {
                var nameValueList = Notify.GetUnderlyingProperties();

                if (nameValueList == null)
                {
                    return false;
                }

                using (var formContent = new FormUrlEncodedContent(nameValueList))
                {
                    using (var webClient = new HttpClient())
                    {
                        using (var response = await webClient.PostAsync(Settings.ValidateUrl, formContent))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                if (result == null || !result.Equals("VALID"))
                                {
                                    return false;
                                }

                                return true;
                            }

                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Private Methods
    }
}
