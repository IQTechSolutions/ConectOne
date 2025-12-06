namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a grouping of vacation-related pricing information.
    /// </summary>
    /// <remarks>This class associates a name with a specific vacation and serves as a container for
    /// pricing-related data. It includes a reference to the associated vacation entity.</remarks>
    public class VacationPriceGroupDViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }
    }
}
