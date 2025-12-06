using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an itinerary, providing details about the itinerary's schedule, associated vacation,
    /// and related items.
    /// </summary>
    /// <remarks>This class is typically used to transfer itinerary data between layers in an application,
    /// such as from a data source to a user interface. It includes properties for the itinerary's unique identifier,
    /// associated vacation, date, time intervals, notes, and detailed items.</remarks>
    public class ItineraryViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates an empty instance of the <see cref="ItineraryViewModel"/>
        /// class. Use this constructor when you need to initialize the view model without any predefined
        /// data.</remarks>
        public ItineraryViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryViewModel"/> class using the specified itinerary data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ItineraryDto"/> to
        /// the corresponding properties of the <see cref="ItineraryViewModel"/>. Ensure that the <paramref name="dto"/>
        /// parameter is not null before calling this constructor.</remarks>
        /// <param name="dto">The <see cref="ItineraryDto"/> containing the itinerary data to initialize the view model.</param>
        public ItineraryViewModel(ItineraryDto dto)
        {
            ItineraryId = dto.ItineraryId;
            Date = dto.Date;
            Details = dto.Details;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the itinerary.
        /// </summary>
        public string ItineraryId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation interval.
        /// </summary>
        public string VacationIntervalId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date associated with the current operation or entity.
        /// </summary>
        public DateTime? Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the start time for the operation.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time at which the operation or event is expected to end.
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the notes associated with the entity.
        /// </summary>
        public string Notes { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether detailed information should be displayed.
        /// </summary>
        public bool ShowDetails { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of itinerary item details.
        /// </summary>
        public ICollection<ItineraryItemDto> Details { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current itinerary to a data transfer object (DTO).
        /// </summary>
        /// <returns>An <see cref="ItineraryDto"/> instance containing the itinerary's ID, date, and details.</returns>
        public ItineraryDto ToDto()
        {
            return new ItineraryDto()
            {
                ItineraryId = ItineraryId,
                Date = Date,
                Details = Details.ToList()
            };
        }

        #endregion
    }
}
