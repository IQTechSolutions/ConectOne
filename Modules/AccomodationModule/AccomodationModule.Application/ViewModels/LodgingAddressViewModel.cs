using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for managing lodging address details.
    /// </summary>
    public class LodgingAddressViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingAddressViewModel"/> class.
        /// </summary>
        public LodgingAddressViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingAddressViewModel"/> class with a <see cref="LodgingDto"/>.
        /// </summary>
        /// <param name="lodging">The lodging DTO.</param>
        public LodgingAddressViewModel(LodgingDto lodging)
        {
            AreaId = lodging.AreaId;
            AreaInfo = lodging.AreaInfo;
            Address = lodging.Address;
            Suburb = lodging.Suburb;
            ProviceId = lodging.ProvinceId;
            Lat = lodging.Lat;
            Lng = lodging.Lng;
            MapLink = lodging.MapLink;
            Directions = lodging.Directions;
        }

        #endregion

        /// <summary>
        /// Gets or sets the area ID.
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// Gets or sets the area information.
        /// </summary>
        public string? AreaInfo { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the unit number.
        /// </summary>
        public string? UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets the complex name.
        /// </summary>
        public string? Complex { get; set; }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        public string? StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        public string? StreetName { get; set; }

        /// <summary>
        /// Gets or sets the suburb.
        /// </summary>
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the province ID.
        /// </summary>
        public int? ProviceId { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the directions.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string? Directions { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the map link.
        /// </summary>
        [DisplayName("Map Link")]
        public string? MapLink { get; set; }

        /// <summary>
        /// Gets or sets the location description.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string? LocationDescription { get; set; }

        /// <summary>
        /// Gets or sets the location ID.
        /// </summary>
        [DisplayName("Location")]
        public int? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the available locations.
        /// </summary>
        public IEnumerable<SelectListItem>? AvailableLocations { get; set; }

        /// <summary>
        /// Updates the current instance with values from another <see cref="LodgingAddressViewModel"/>.
        /// </summary>
        /// <param name="address">The address view model to copy values from.</param>
        public void Update(LodgingAddressViewModel address)
        {
            AreaId = address.AreaId;
            AreaInfo = address.AreaInfo;
            Address = address.Address;
            UnitNumber = address.UnitNumber;
            Complex = address.Complex;
            StreetName = address.StreetName;
            StreetNumber = address.StreetNumber;
            Suburb = address.Suburb;
            PostalCode = address.PostalCode;
            City = address.City;
            Country = address.Country;
            Lat = address.Lat;
            Lng = address.Lng;
            MapLink = address.MapLink;
            Directions = address.Directions;
            LocationId = address.LocationId;
        }
    }
}
