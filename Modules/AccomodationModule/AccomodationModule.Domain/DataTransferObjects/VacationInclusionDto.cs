using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for vacation inclusions.
    /// Represents additional services or features included in a vacation package.
    /// </summary>
    public record VacationInclusionDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor that initializes a new instance of <see cref="VacationInclusionDto"/>.
        /// </summary>
        public VacationInclusionDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationInclusionDto"/> using an <see cref="Inclusions"/> entity.
        /// </summary>
        /// <param name="vacation">The entity containing vacation inclusion details.</param>
        public VacationInclusionDto(Inclusions vacation)
        {
            VacationInclusionId = vacation.Id;
            VacationId = vacation.VacationId!;
            Name = vacation.Name;
            Description = vacation.Description;
            Selector = vacation.Selector;
            Order = vacation.Order;
        }

        #endregion

        /// <summary>
        /// Gets or initializes the unique identifier for the vacation inclusion.
        /// </summary>
        public string? VacationInclusionId { get; init; } 

        /// <summary>
        /// Gets or initializes the associated vacation package ID.
        /// </summary>
        public string? VacationId { get; init; }

        /// <summary>
        /// Gets or initializes the associated vacation extension ID (if applicable).
        /// </summary>
        public string? VacationExtensionId { get; init; } 

        /// <summary>
        /// Gets or initializes the name of the vacation inclusion.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the optional description for the vacation inclusion.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the selector for the vacation inclusion.
        /// </summary>
        public string? Selector { get; set; }

        /// <summary>
        /// Gets or sets the order of the vacation inclusion.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Converts the DTO into an <see cref="Inclusions"/> entity.
        /// </summary>
        /// <returns>An <see cref="Inclusions"/> entity representing the vacation inclusion.</returns>
        public Inclusions ToInclusion()
        {
            return new Inclusions()
            {
                Id = VacationInclusionId!,
                VacationId = VacationId,
                Name = Name,
                Description = Description ?? "",
                Selector = Selector ?? "",
                Order = Order
            };
        }
        
        /// <summary>
        /// Updates the values of an existing <see cref="Inclusions"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="vacationInclusion">The vacation inclusion that needs to be updated</param>
        public void UpdateVacationInclusionValues(in Inclusions vacationInclusion)
        {
            vacationInclusion.Name = Name;
            vacationInclusion.Description = Description ?? "";
            vacationInclusion.Selector = Selector ?? "";
            vacationInclusion.Order = Order;
        }
    }
}
