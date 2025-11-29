using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for address information, encapsulating details such as street, city,
    /// province, country, and geographic coordinates.
    /// </summary>
    /// <remarks>The AddressDto type is typically used to transfer address data between application layers or
    /// services. It provides a convenient structure for serializing and deserializing address-related information, and
    /// includes methods for mapping to and from domain address entities. This record supports both physical and other
    /// address types, and can be associated with a specific entity type when converting to a strongly typed address
    /// object.</remarks>
    public record AddressDto : IDefaultEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDto"/> class.
        /// </summary>
        public AddressDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDto"/> class using the specified <see cref="Address"/>
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Address"/> object to
        /// the corresponding properties of the <see cref="AddressDto"/>. If <paramref name="address"/> is <see
        /// langword="null"/>, no properties will be set.</remarks>
        /// <param name="address">The <see cref="Address"/> object from which to populate the properties of the <see cref="AddressDto"/>
        /// instance. If <paramref name="address"/> is <see langword="null"/>, the properties of the <see
        /// cref="AddressDto"/> will not be initialized.</param>
        public AddressDto(Address address)
        {
            if (address != null)
            {
                AddressId = address?.Id;
                GoogleMapLink = address?.GoogleMapLink;
                UnitNumber = address?.UnitNumber;
                Complex = address?.Complex;
                StreetNumber = address?.StreetNumber;
                StreetName = address?.StreetName;
                Suburb = address?.Suburb;
                PostalCode = address?.PostalCode;
                City = address?.City;
                Province = address?.Province;
                Country = address?.Country;
                Latitude = address.Latitude;
                Longitude = address.Longitude;
                AddressType = address.AddressType;

            }
            
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the address.
        /// </summary>
        public string? AddressId { get; init; }

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the route.
        /// </summary>
        public string? RouteId { get; init; }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        public string? RouteName { get; set; }

        /// <summary>
        /// Gets or sets the URL link to the location on Google Maps.
        /// </summary>
        public string? GoogleMapLink { get; set; }

        /// <summary>
        /// Gets the unit number associated with the entity.
        /// </summary>
        public string? UnitNumber { get; init; }

        /// <summary>
        /// Gets the complex value associated with this instance.
        /// </summary>
        public string? Complex { get; init; }

        /// <summary>
        /// Gets the street number of the address.
        /// </summary>
        public string? StreetNumber { get; init; }

        /// <summary>
        /// Gets the name of the street associated with the address.
        /// </summary>
        public string? StreetName { get; init; }

        /// <summary>
        /// Gets the suburb associated with the address.
        /// </summary>
        public string? Suburb { get; init; }

        /// <summary>
        /// Gets the postal code associated with the address.
        /// </summary>
        public string? PostalCode { get; init; }

        /// <summary>
        /// Gets the name of the city associated with the address.
        /// </summary>
        public string? City { get; init; }

        /// <summary>
        /// Gets the name of the province associated with the address.
        /// </summary>
        public string? Province { get; init; }

        /// <summary>
        /// Gets the country associated with the current entity.
        /// </summary>
        public string? Country { get; init; }

        /// <summary>
        /// Gets the latitude coordinate of a geographic location.
        /// </summary>
        public double Latitude { get; init; }

        /// <summary>
        /// Gets the longitude coordinate of a geographic location.
        /// </summary>
        public double Longitude { get; init; }

        /// <summary>
        /// Gets the radius of the circle.
        /// </summary>
        public int Radius { get; init; }

        /// <summary>
        /// Gets the type of address associated with this instance.
        /// </summary>
        public AddressType AddressType { get; init; } = AddressType.Physical;

        /// <summary>
        /// Gets a value indicating whether this instance represents the default configuration or state.
        /// </summary>
        public bool Default { get; init; }


        #region Read Only

        /// <summary>
        /// Gets the string representation of the address.
        /// </summary>
        public string AddressString => this.ToString();

        /// <summary>
        /// Returns a string representation of the address, including the street name, street number, suburb, city, and
        /// province.
        /// </summary>
        /// <returns>A string in the format "StreetName StreetNumber, Suburb City Province".</returns>
        public override string ToString() => $"{StreetName} {StreetNumber}, {Suburb} {City} {Province}";

        #endregion

        /// <summary>
        /// Converts the current instance to an <see cref="Address{TEntity}"/> object.
        /// </summary>
        /// <remarks>The returned <see cref="Address{TEntity}"/> object includes all address-related
        /// properties such as the street name, city, postal code, and geographic coordinates. This method is useful for
        /// mapping address data to a strongly typed object.</remarks>
        /// <typeparam name="TEntity">The type of the entity associated with the address. This allows the address to be strongly typed to a
        /// specific entity type.</typeparam>
        /// <returns>An <see cref="Address{TEntity}"/> object containing the address details from the current instance.</returns>
        public Address<TEntity> ToAddress<TEntity>() 
        {
            return new Address<TEntity>()
            {
                GoogleMapLink = this.GoogleMapLink,
                UnitNumber = this.UnitNumber,
                Complex = this.Complex,
                StreetNumber = this.StreetNumber,
                StreetName = this.StreetName,
                Suburb = this.Suburb,
                PostalCode = this.PostalCode,
                City = this.City,
                Province = this.Province,
                Country = this.Country,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                AddressType = this.AddressType,
            };
        }
    }
}
