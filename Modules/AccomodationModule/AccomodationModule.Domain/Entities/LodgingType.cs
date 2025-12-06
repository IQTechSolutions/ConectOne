using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a type of lodging, such as a hotel, motel, or vacation rental.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of a lodging type, including its name and
    /// description. It can be used to categorize different types of accommodations in a system.</remarks>
    public class LodgingType : EntityBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the LodgingType class.
        /// </summary>
        public LodgingType() { }

        /// <summary>
        /// Initializes a new instance of the LodgingType class using the specified data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the identifier, name, and description to initialize the LodgingType
        /// instance. Cannot be null.</param>
        public LodgingType(LodgingTypeDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
        }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string? Description { get; set; }
    }
}
