namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the rate scheme for pricing accommodation.
    /// </summary>
    public enum RateScheme
    {
        /// <summary>
        /// Pricing is based on the number of people sharing the accommodation.
        /// </summary>
        PerPersonSharing = 1,

        /// <summary>
        /// Pricing is based on a fixed unit price, regardless of the number of people.
        /// </summary>
        UnitPrice = 2
    }
}