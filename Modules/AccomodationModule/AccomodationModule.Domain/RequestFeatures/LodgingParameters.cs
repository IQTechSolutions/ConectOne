using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a set of parameters used to filter and query lodging data.
    /// </summary>
    /// <remarks>This class provides properties for specifying various criteria, such as price range,
    /// availability,  location, and amenities, to refine lodging search results. It is commonly used in APIs or
    /// services  that retrieve lodging information based on user preferences or requirements.</remarks>
    public class LodgingParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingParameters"/> class with default values.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><see cref="OrderBy"/>
        /// is set to "Name".</description></item> <item><description><see cref="PageSize"/> is set to
        /// 12.</description></item> </list> These defaults can be modified after initialization.</remarks>
        public LodgingParameters()
        {
            OrderBy = "Name";
            PageSize = 12;
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the display is enabled.
        /// </summary>
        public bool Display { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool? Featured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bookings are allowed.
        /// </summary>
        public bool? AllowBookings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the getaway option is enabled.
        /// </summary>
        public bool? Getaway { get; set; }

        /// <summary>
        /// Gets or sets the minimum price for a product or service.
        /// </summary>
        public double MinPrice { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum allowable price for a transaction.
        /// </summary>
        public double MaxPrice { get; set; } = 100000;

        /// <summary>
        /// Gets or sets the identifier of the province.
        /// </summary>
        public string? ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the beneficiary.
        /// </summary>
        public string? BeneficiaryId { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of category IDs.
        /// </summary>
        public string? CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of amenity identifiers.
        /// </summary>
        public string? AmenityIds { get; set; }

        #region Room Availability

        /// <summary>
        /// Gets or sets the identifier for the location associated with room availability.
        /// </summary>
        public string? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or activity.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms.
        /// </summary>
        public int Rooms { get; set; }

        /// <summary>
        /// Gets or sets the number of adults associated with the current context.
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// Gets or sets the number of children associated with the entity.
        /// </summary>
        public int Kids { get; set; }

        #endregion
    }
}
