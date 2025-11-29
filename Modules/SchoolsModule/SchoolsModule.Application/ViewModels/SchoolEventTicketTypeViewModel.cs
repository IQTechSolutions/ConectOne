using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a ticket type for a school event, including details such as name, description, price, and ticket
    /// quantities.
    /// </summary>
    /// <remarks>This view model is used to encapsulate the details of a specific ticket type for a school
    /// event.  It includes properties for identifying the ticket type, associating it with an event, and tracking
    /// ticket availability and sales.</remarks>
    public class SchoolEventTicketTypeViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketTypeViewModel"/> class.
        /// </summary>
        public SchoolEventTicketTypeViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventTicketTypeViewModel"/> class using the specified
        /// data transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="SchoolEventTicketTypeDto"/>  to the corresponding properties of the <see
        /// cref="SchoolEventTicketTypeViewModel"/> instance.</remarks>
        /// <param name="dto">The data transfer object containing the ticket type details, including its ID, event ID, name,  description,
        /// price, quantity available, and quantity sold.</param>
        public SchoolEventTicketTypeViewModel(SchoolEventTicketTypeDto dto)
        {
            Id = dto.Id;
            EventId = dto.EventId;
            Name = dto.Name;
            Description = dto.Description;
            Price = dto.Price;
            QuantityAvailable = dto.QuantityAvailable;
            QuantitySold = dto.QuantitySold;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string? Description { get; set; } 

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of items currently available in stock.
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Gets or sets the quantity of items sold.
        /// </summary>
        public int QuantitySold { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="SchoolEventTicketType"/> to a  <see
        /// cref="SchoolEventTicketTypeDto"/>.
        /// </summary>
        /// <returns>A <see cref="SchoolEventTicketTypeDto"/> that represents the current ticket type,  including its identifier,
        /// associated event, name, description, price, and quantity details.</returns>
        public SchoolEventTicketTypeDto ToDto()
        {
            return new SchoolEventTicketTypeDto
            {
                Id = this.Id,
                EventId = this.EventId,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                QuantityAvailable = this.QuantityAvailable,
                QuantitySold = this.QuantitySold
            };
        }

        #endregion
    }
}
