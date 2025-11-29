using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a ticket type for a school event, including details such as name, description, price, and
    /// availability.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate information about a specific ticket
    /// type for a school event. It includes details such as the ticket's unique identifier, the associated event,
    /// pricing, and availability.</remarks>
    public record SchoolEventTicketTypeDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketTypeDto"/> class.
        /// </summary>
        public SchoolEventTicketTypeDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketTypeDto"/> class using the specified ticket
        /// type.
        /// </summary>
        /// <param name="type">The ticket type entity containing the details to initialize the DTO. Cannot be null.</param>
        public SchoolEventTicketTypeDto(SchoolEventTicketType type)
        {
            Id = type.Id;
            EventId = type.EventId;
            Name = type.Name;
            Description = type.Description;
            Price = type.Price;
            QuantityAvailable = type.QuantityAvailable;
            QuantitySold = type.QuantitySold;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier for the event.
        /// </summary>
        public string EventId { get; init; } = null!;

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the description associated with the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the price of the item.
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Gets the quantity of the item that is currently available.
        /// </summary>
        public int QuantityAvailable { get; init; }

        /// <summary>
        /// Gets the quantity of items sold.
        /// </summary>
        public int QuantitySold { get; init; }

        #endregion
    }
}
