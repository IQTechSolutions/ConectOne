namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the different API partners that can be integrated with the accommodation system.
    /// </summary>
    public enum ApiPartners
    {
        /// <summary>
        /// No API partner specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the NightsBridge API partner.
        /// NightsBridge is a property management system for accommodation providers.
        /// </summary>
        NightsBridge = 1,

        /// <summary>
        /// Represents the Cimso API partner.
        /// Cimso provides software solutions for the hospitality industry.
        /// </summary>
        Cimso = 2,
    }
}
