using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a single amenity entity, which can be associated with various
    /// accommodations, facilities, or services. An amenity typically includes 
    /// a display icon, a name, and an optional description.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="EntityBase{TId}"/> which provides a unique 
    /// identifier property. In this case, the identifier is of type <see cref="int"/>.
    /// 
    /// Consider integrating validation attributes and business logic to ensure data 
    /// integrity (e.g., the <see cref="Name"/> and <see cref="IconClass"/> properties 
    /// should not be null or empty to avoid inconsistent UI representations).
    /// </remarks>
    public class Amenity : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the CSS class or icon identifier associated with the amenity.
        /// This property is used to display a relevant icon representing the amenity 
        /// in a UI context, such as on a website or application interface.
        /// </summary>
        /// <example>"fa fa-swimmer"</example>
        [Required(ErrorMessage = "IconClass is required."), StringLength(100, ErrorMessage = "IconClass cannot exceed 100 characters.")]
        public string IconClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable name of the amenity.
        /// This property is essential for identifying the amenity to end-users.
        /// </summary>
        /// <example>"Swimming Pool"</example>
        [Required(ErrorMessage = "Name is required."), StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional textual description providing additional details 
        /// about the amenity. While not required, it can give more context, usage 
        /// instructions, or highlight special features.
        /// </summary>
        /// <example>"Heated indoor pool available from 6 AM to 10 PM."</example>
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters.")]
        public string? Description { get; set; }
    }
}
