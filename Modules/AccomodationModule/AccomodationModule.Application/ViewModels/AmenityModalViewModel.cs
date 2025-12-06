using System.ComponentModel;
using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for an amenity modal, providing details about an amenity such as its ID, name, and
    /// icon.
    /// </summary>
    /// <remarks>This class is typically used to bind data for displaying or editing amenity information in a
    /// modal dialog. It can be initialized with default values or populated using an <see cref="AmenityDto"/>
    /// object.</remarks>
    public class AmenityModalViewModel
    {
		#region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityModalViewModel"/> class.
        /// </summary>
        public AmenityModalViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityModalViewModel"/> class using the specified amenity
        /// data.
        /// </summary>
        /// <param name="amenity">The amenity data used to populate the view model. Cannot be null.</param>
        public AmenityModalViewModel(AmenityDto amenity) 
        {
            AmenityId = amenity.AmenityId;
            Name = amenity.Name;
            Icon = amenity.Icon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the amenity.
        /// </summary>
        [DisplayName("Amenity Id")] public string? AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
		[DisplayName("Short Description")] public string Name { get; set; }

        /// <summary>
        /// Gets or sets the icon associated with the item.
        /// </summary>
        [DisplayName("Icon")] public string Icon { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="Amenity"/> instance to an <see cref="AmenityDto"/>.
        /// </summary>
        /// <returns>An <see cref="AmenityDto"/> object containing the data from the current <see cref="Amenity"/> instance.</returns>
        public AmenityDto ToDto()
        {
            return new AmenityDto
            {
                AmenityId = this.AmenityId,
                Name = this.Name,
                Icon = this.Icon
            };
        }

        #endregion
    }
}
