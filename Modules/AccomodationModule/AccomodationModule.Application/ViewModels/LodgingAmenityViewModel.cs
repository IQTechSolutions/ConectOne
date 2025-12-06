using System.ComponentModel;
using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for lodging amenities, providing details such as the amenity's name, description, icon,
    /// and selection state.
    /// </summary>
    /// <remarks>This class is typically used to display and manage amenity information in a user interface.
    /// It includes properties for identifying the amenity, describing it, and tracking whether it is
    /// selected.</remarks>
    public class LodgingAmenityViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingAmenityViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see
        /// cref="LodgingAmenityViewModel"/> class. Use this constructor when no initial data is required.</remarks>
        public LodgingAmenityViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingAmenityViewModel"/> class using the specified amenity
        /// data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="AmenityDto"/> to the
        /// corresponding properties of the <see cref="LodgingAmenityViewModel"/> instance.</remarks>
        /// <param name="amenity">The amenity data transfer object containing information about the amenity. Cannot be null.</param>
        public LodgingAmenityViewModel(AmenityDto amenity)
        {
            AmenityId = Convert.ToInt32(amenity.AmenityId);
            Name = amenity.Name;
            Description = amenity.Description;
            Icon = amenity.Icon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for an amenity.
        /// </summary>
        public int AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        [DisplayName("Short Description")] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the long description associated with the object.
        /// </summary>
		[DisplayName("Long Description")] public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the icon associated with the item.
        /// </summary>
        [DisplayName("Icon")] public string Icon { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the item is selected.
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="o">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="LodgingAmenityViewModel"/>  and its
        /// <c>AmenityId</c> matches the <c>AmenityId</c> of the current instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? o)
        {
            var other = o as LodgingAmenityViewModel;
            return other?.AmenityId==AmenityId;
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the <see cref="AmenityId"/> property. This method is
        /// suitable for use in hashing algorithms and data structures such as hash tables.</remarks>
        /// <returns>An integer representing the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return AmenityId.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>The value of the <see cref="Name"/> property.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Converts the current instance of the amenity to its corresponding data transfer object (DTO) representation.
        /// </summary>
        /// <returns>An <see cref="AmenityDto"/> object containing the amenity's identifier, name, description, and icon.</returns>
        public AmenityDto ToDto()
        {
            return new AmenityDto
            {
                AmenityId = AmenityId.ToString(),
                Name = Name,
                Description = Description,
                Icon = Icon
            };
        }

        #endregion
    }
}
