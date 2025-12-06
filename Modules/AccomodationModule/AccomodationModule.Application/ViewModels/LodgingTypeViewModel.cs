using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for lodging types, providing details such as the unique identifier, name, and
    /// description.
    /// </summary>
    /// <remarks>This class is typically used to transfer lodging type data between the application layers,
    /// such as from a data source to a user interface. It includes properties for the lodging type's unique identifier,
    /// name, and an optional description.</remarks>
    public class LodgingTypeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingTypeViewModel"/> class.
        /// </summary>
        public LodgingTypeViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingTypeViewModel"/> class using the specified data transfer
        /// object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the lodging type information. Must not be <see langword="null"/>.</param>
        public LodgingTypeViewModel(LodgingTypeDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [Required] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; } 
    }
}
