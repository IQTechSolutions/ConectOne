namespace AccomodationModule.Domain.Arguments.Response
{
    /// <summary>
    /// Represents a response containing the count of lodgings.
    /// </summary>
    /// <remarks>This type is typically used to return the number of lodgings in response to a query or
    /// operation.</remarks>
    public record LodgingCountResponse
    {
        /// <summary>
        /// Gets the total number of lodging accommodations available.
        /// </summary>
        public int LodgingCount { get; init; }
    }
}