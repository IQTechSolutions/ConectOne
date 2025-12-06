namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a grouping of vacation-related pricing information.
    /// </summary>
    /// <remarks>This class associates a name with a specific vacation and serves as a container for
    /// pricing-related data. It includes a reference to the associated vacation entity.</remarks>
    public record VacationPriceGroupDto 
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the unique identifier for the vacation.
        /// </summary>
        public string? VacationId { get; init; }
    }
}
