using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents an address entity with various address-related properties.
    /// </summary>
    public class Address : EntityBase<string>, IDefaultEntity
    {
        /// <summary>
        /// Gets or sets the Google Map link for the address.
        /// </summary>
        public string? GoogleMapLink { get; set; }

        /// <summary>
        /// Gets or sets the unit number of the address.
        /// </summary>
        [MaxLength(10, ErrorMessage = "Maximum length for the Unit Nr is 10 characters.")]
        public string? UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets the complex name of the address.
        /// </summary>
        [MaxLength(255, ErrorMessage = "Maximum length for the Complex is 255 characters.")]
        public string? Complex { get; set; }

        /// <summary>
        /// Gets or sets the street number of the address.
        /// </summary>
        [MaxLength(10, ErrorMessage = "Maximum length for the Street Nr is 10 characters.")]
        public string? StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Maximum length for the Address Line is 1000 characters.")]
        public string? StreetName { get; set; }

        /// <summary>
        /// Gets or sets the suburb of the address.
        /// </summary>
        [MaxLength(250, ErrorMessage = "Maximum length for the Suburb is 30 characters.")]
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        [MaxLength(5, ErrorMessage = "Maximum length for the Postal Code is 5 characters.")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        [MaxLength(250, ErrorMessage = "Maximum length for the city is 30 characters.")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the province of the address.
        /// </summary>
        [MaxLength(250, ErrorMessage = "Maximum length for the Province is 30 characters.")]
        public string? Province { get; set; }

        /// <summary>
        /// Gets or sets the country of the address. Default is "South Africa".
        /// </summary>
        [MaxLength(255, ErrorMessage = "Maximum length for the Country is 255 characters.")]
        public string? Country { get; set; } = "South Africa";

        /// <summary>
        /// Gets or sets the latitude of the address.
        /// </summary>
        [MaxLength(255, ErrorMessage = "Maximum length for the Country is 255 characters.")]
        public double Latitude { get; set; } = 0;

        /// <summary>
        /// Gets or sets the longitude of the address.
        /// </summary>
        [MaxLength(255, ErrorMessage = "Maximum length for the Country is 255 characters.")]
        public double Longitude { get; set; } = 0;

        /// <summary>
        /// Gets or sets the route ID of the address.
        /// </summary>
        [MaxLength(255, ErrorMessage = "Maximum length for the Country is 255 characters.")]
        public string? RouteId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this address is the default address.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets the type of the address.
        /// </summary>
        public AddressType AddressType { get; set; } = AddressType.Physical;

        /// <summary>
        /// Gets a single line representation of the address.
        /// </summary>
        public string OnelineAddress => $"{UnitNumber} {Complex} {StreetNumber} {StreetName}, {Suburb}, {City}, {Province}, {Country}";

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Address";
        }
    }

    /// <summary>
    /// Represents an address entity associated with another entity.
    /// </summary>
    /// <typeparam name="T">The type of the associated entity.</typeparam>
    public class Address<T> : Address
    {
        /// <summary>
        /// Gets or sets the ID of the associated entity.
        /// </summary>
        [ForeignKey(nameof(Entity))]
        public string? EntityId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated entity.
        /// </summary>
        public T? Entity { get; set; } = default!;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Address for {typeof(T).Name}";
        }
    }
}

