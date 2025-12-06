using AccomodationModule.Domain.Enums;
using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Arguments
{
    /// <summary>
    /// Represents the parameters used for filtering and querying booking-related data.
    /// </summary>
    /// <remarks>This class provides a flexible way to specify filtering criteria, pagination settings, and
    /// sorting options for booking-related queries. It includes properties for price ranges, category filters,
    /// beneficiary filters, and room availability details, among others.</remarks>
    public class BookingParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingParameters"/> class with default values.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><see cref="OrderBy"/>
        /// is set to "Name".</description></item> <item><description><see cref="PageSize"/> is set to
        /// 12.</description></item> </list> These defaults can be modified after initialization by setting the
        /// respective properties.</remarks>
        public BookingParameters()
        {
            OrderBy = "Name";
            PageSize = 12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingParameters"/> class, providing filtering, sorting, and
        /// pagination options for booking queries.
        /// </summary>
        /// <remarks>This constructor allows the caller to specify a wide range of filtering, sorting, and
        /// pagination options for querying bookings. Use the provided parameters to narrow down the results based on
        /// specific criteria.</remarks>
        /// <param name="sortOrder">The sorting order for the results. Typically specifies the field and direction (e.g., "price_desc").</param>
        /// <param name="pageNr">The page number for pagination. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page for pagination. Defaults to 50.</param>
        /// <param name="minPrice">The minimum price filter for bookings. Defaults to 0.</param>
        /// <param name="maxPrice">The maximum price filter for bookings. Defaults to <see cref="double.MaxValue"/>.</param>
        /// <param name="featured">A filter indicating whether to include only featured bookings. If <see langword="null"/>, no filter is
        /// applied.</param>
        /// <param name="getaway">A filter indicating whether to include only getaway bookings. If <see langword="null"/>, no filter is
        /// applied.</param>
        /// <param name="active">A filter indicating whether to include only active bookings. Defaults to <see langword="true"/>.</param>
        /// <param name="display">A filter indicating whether to include only bookings marked for display. Defaults to <see langword="true"/>.</param>
        /// <param name="searchText">A search term to filter bookings by text. If <see langword="null"/>, no search filter is applied.</param>
        /// <param name="categoryId">The ID of the category to filter bookings. If <see langword="null"/>, no category filter is applied.</param>
        /// <param name="beneficiaryId">The ID of the beneficiary to filter bookings. If <see langword="null"/>, no beneficiary filter is applied.</param>
        /// <param name="provinceId">The ID of the province to filter bookings. If <see langword="null"/>, no province filter is applied.</param>
        public BookingParameters(string sortOrder, int pageNr = 1, int pageSize = 50, double minPrice = 0, double maxPrice = double.MaxValue, bool? featured = null, bool? getaway = null, 
            bool active = true, bool display = true, string? searchText = null, string? categoryId = null, string? beneficiaryId = null, string? provinceId = null) : base(pageNr, pageSize, sortOrder)
        {
            OrderBy = sortOrder;
            MinPrice= minPrice;
            MaxPrice= maxPrice;
            Active = active;
            Display = display;
            Getaway = getaway;
            Featured = featured;
            SearchText= searchText;
            CategoryId = categoryId;
            BeneficiaryId= beneficiaryId;
            ProvinceId=provinceId;
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
        /// Gets or sets a value indicating whether the operation is configured as a getaway.
        /// </summary>
        public bool? Getaway { get; set; }

        /// <summary>
        /// Gets or sets the minimum price allowed for a product or transaction.
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        public BookingStatus? BookingStatus { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable price for a product or service.
        /// </summary>
        public double MaxPrice { get; set; }     

        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
		public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the province.
        /// </summary>
        public string? ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the beneficiary.
        /// </summary>
        public string? BeneficiaryId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string? UserId { get; set; }

        #region Room Availability

        /// <summary>
        /// Gets or sets the identifier for the location associated with room availability.
        /// </summary>
        public string? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public string? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or process.
        /// </summary>
        public string? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms in the building.
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
