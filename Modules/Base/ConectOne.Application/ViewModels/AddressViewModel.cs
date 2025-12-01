using System.ComponentModel;
using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;

namespace ConectOne.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an address, providing detailed information about a physical or other type of
    /// address.
    /// </summary>
    /// <remarks>This class is designed to encapsulate address-related data for use in applications, such as
    /// mapping, routing, or display purposes. It includes properties for geographic coordinates, address components,
    /// and metadata such as the address type and default status.</remarks>
    public class AddressViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressViewModel"/> class.
        /// </summary>
        public AddressViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressViewModel"/> class using the specified <see
        /// cref="AddressDto"/>.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="AddressDto"/> to the
        /// corresponding properties of the <see cref="AddressViewModel"/>. If the <paramref name="address"/> parameter
        /// is <see langword="null"/>,  the properties will be initialized with default values: <list type="bullet">
        /// <item><description>Numeric properties (e.g., <c>Latitude</c>, <c>Longitude</c>, <c>Radius</c>) will be set
        /// to <c>0</c>.</description></item> <item><description>Boolean properties (e.g., <c>Default</c>) will be set
        /// to <see langword="false"/>.</description></item> <item><description>Enum properties (e.g.,
        /// <c>AddressType</c>) will be set to <c>AddressType.Physical</c>.</description></item>
        /// <item><description>String properties will be set to <see langword="null"/>.</description></item>
        /// </list></remarks>
        /// <param name="address">The <see cref="AddressDto"/> containing the address details to populate the view model.  If <paramref
        /// name="address"/> is <see langword="null"/>, default values will be assigned to the properties.</param>
        public AddressViewModel(AddressDto address)
        {
            AddressId = address?.AddressId;
            ParentId = address?.ParentId;
            GoogleMapLink = address?.GoogleMapLink;
            UnitNumber= address?.UnitNumber;
            Complex = address?.Complex;
            StreetNumber= address?.StreetNumber;
            StreetName= address?.StreetName;
            Suburb = address?.Suburb;
            PostalCode= address?.PostalCode;
            City= address?.City;
            Province= address?.Province;
            Country= address?.Country;
            RouteId= address?.RouteId;
            RouteName= address?.RouteName;
            Latitude= address == null ? 0 : address.Latitude;
            Longitude= address == null ? 0 : address.Longitude;
            Radius= address == null ? 0 : address.Radius;
            Default = address == null ? false : address.Default;
            AddressType= address == null ? AddressType.Physical : address.AddressType;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Gets or sets the unique identifier for an address.
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the URL link to a Google Map location.
        /// </summary>
        [DisplayName("Google Map Link")] public string? GoogleMapLink { get; set; }

        /// <summary>
        /// Gets or sets the unit number associated with the entity.
        /// </summary>
        [DisplayName("Unit")] public string? UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets a value that represents a complex string, which may be null.
        /// </summary>
        public string? Complex { get; set; }

        /// <summary>
        /// Gets or sets the street number of an address.
        /// </summary>
        [DisplayName("Number")] public string? StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        [DisplayName("Street")] public string? StreetName { get; set; }

        /// <summary>
        /// Gets or sets the name of the suburb associated with the address.
        /// </summary>
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the postal code associated with the address.
        /// </summary>
        [DisplayName("Code")] public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the city associated with the entity.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the province associated with the entity.
        /// </summary>
        public string? Province { get; set; }

        /// <summary>
        /// Gets or sets the name of the country associated with the entity.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the route.
        /// </summary>
        public string? RouteId { get; set; }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        [DisplayName("Route Name")] public string? RouteName { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of a geographic location.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the radius of the shape.
        /// </summary>
        public int Radius { get; set; } = 50;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is the default configuration.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets the type of address associated with this instance.
        /// </summary>
        public AddressType AddressType { get; set; } = AddressType.Physical;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="Address"/> instance to an <see cref="AddressDto"/> object.
        /// </summary>
        /// <remarks>This method maps all properties of the <see cref="Address"/> instance to their
        /// corresponding properties in the <see cref="AddressDto"/> object. The returned object is a new instance and
        /// does not share references with the original <see cref="Address"/>.</remarks>
        /// <returns>An <see cref="AddressDto"/> object containing the data from the current <see cref="Address"/> instance.</returns>
        public AddressDto ToDto()
        {
            return new AddressDto()
            {
                AddressId = AddressId,
                ParentId = ParentId,
                RouteId = RouteId,
                RouteName = RouteName,
                UnitNumber = UnitNumber,
                Complex = Complex,
                StreetNumber = StreetNumber,
                StreetName = StreetName,
                Suburb = Suburb,
                PostalCode = PostalCode,
                City = City,
                Province = Province,
                Country = Country,
                Latitude = Latitude,
                Longitude = Longitude,
                Radius = Radius,
                AddressType = AddressType,
                Default = Default
            };
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the address, including unit number, complex, street number, street name,
        /// suburb, city, and province.
        /// </summary>
        /// <returns>A string that concatenates the unit number, complex, street number, street name, suburb, city, and province,
        /// separated by spaces.</returns>
        public override string ToString()
        {
            return $"{UnitNumber} {Complex} {StreetNumber} {StreetName} {Suburb} {City} {Province}";
        }

        #endregion
    }
}
