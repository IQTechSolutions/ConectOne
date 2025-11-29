using ConectOne.Domain.Entities;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a type of ticket available for a school event, including details such as name, description, price,
    /// and availability.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of a specific ticket type for a school
    /// event,  including its unique identifier, pricing, and inventory details.</remarks>
    public class SchoolEventTicketType : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketType"/> class.
        /// </summary>
        public SchoolEventTicketType() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketType"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="SchoolEventTicketTypeDto"/> to the corresponding properties of the <see cref="SchoolEventTicketType"/>
        /// instance.</remarks>
        /// <param name="eventTicketTypeDto">A <see cref="SchoolEventTicketTypeDto"/> object containing the ticket type details, including its
        /// identifier, event ID, name, description, price, available quantity, and quantity sold.</param>
        public SchoolEventTicketType(SchoolEventTicketTypeDto eventTicketTypeDto)
        {
            Id = eventTicketTypeDto.Id;
            EventId = eventTicketTypeDto.Id;
            Name = eventTicketTypeDto.Name;
            Description = eventTicketTypeDto.Description;
            Price = eventTicketTypeDto.Price;
            QuantityAvailable = eventTicketTypeDto.QuantityAvailable;
            QuantitySold = eventTicketTypeDto.QuantitySold;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of items available in stock.
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Gets or sets the quantity of items sold.
        /// </summary>
        public int QuantitySold { get; set; }

        #endregion

    }
}
