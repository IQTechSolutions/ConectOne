using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data transfer object (DTO) representing a vacation pricing item.
    /// This DTO is used to transfer vacation pricing details between different layers of the application.
    /// </summary>
    public record VacationPricingItemDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for serialization/deserialization purposes.
        /// </summary>
        public VacationPricingItemDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationPricingItemDto"/> from a <see cref="VacationPrice"/> entity.
        /// </summary>
        /// <param name="vacation">The vacation pricing entity to copy data from.</param>
        public VacationPricingItemDto(VacationPrice vacation)
        {
            VacationPriceItemId = vacation.Id;
            VacationId = vacation.VacationId!;
            Name = vacation.Name;
            Description = vacation.Description;
            Price = vacation.Price;
            Selector = vacation.Selector;
            Order = vacation.Order;
        }

        #endregion

        /// <summary>
        /// Unique identifier for the vacation pricing item.
        /// </summary>
        public string VacationPriceItemId { get; init; } = null!;

        /// <summary>
        /// Identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; init; }

        /// <summary>
        /// Identifier for the vacation extension associated with this pricing item.
        /// </summary>
        public string? VacationExtensionId { get; init; } 

        /// <summary>
        /// Name of the vacation pricing item.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Description of the vacation pricing item.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Price of the vacation pricing item.
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Gets or sets the selector used to categorize or group the vacation pricing item.
        /// </summary>
        public string? Selector { get; set; }

        /// <summary>
        /// Gets or sets the order of the vacation pricing item within its group or category.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Converts this DTO into a <see cref="VacationPrice"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="VacationPrice"/> object with the same data.</returns>
        public VacationPrice ToVacationPrice()
        {
            return new VacationPrice()
            {
                Id = VacationPriceItemId,
                VacationId = VacationId,
                Name = Name,
                Description = Description,
                Price = Price,
                Selector = Selector,
                Order = Order
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="VacationPrice"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="vacationPrice">The vacation price that needs to be updated</param>
        public void UpdateVactionPricingValues(in VacationPrice vacationPrice)
        {
            vacationPrice.Name = Name;
            vacationPrice.Description = Description ?? "";
            vacationPrice.Price = Price;
            vacationPrice.Selector = Selector;
            vacationPrice.Order = Order;
        }
    }

    /// <summary>
    /// Request object for updating vacation inclusion display type information.
    /// </summary>
    /// <param name="VacationId">The identity of the vacation that the display type sections are being updated for </param>
    /// <param name="Items">The items being updated</param>
    public record VacationPricingItemGroupUpdateRequest(string VacationId, List<VacationPricingItemDto> Items);
}
