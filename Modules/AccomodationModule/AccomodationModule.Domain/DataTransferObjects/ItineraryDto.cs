using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for Itinerary.
    /// </summary>
    public record ItineraryDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryDto"/> class.
        /// </summary>
        public ItineraryDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryDto"/> class based on an <see cref="Itinerary"/> model.
        /// </summary>
        /// <param name="model">The <see cref="Itinerary"/> model to create the DTO from.</param>
        public ItineraryDto(Itinerary model)
        {
            ItineraryId = model.Id;
            Date = model.Date;
            Details = model.ItineraryDetails.Select(c => new ItineraryItemDto(c)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the itinerary ID.
        /// </summary>
        public string ItineraryId { get; init; } = null!;

        /// <summary>
        /// Gets the date of the itinerary.
        /// </summary>
        public DateTime? Date { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether to show itinerary items.
        /// </summary>
        public bool ShowItems { get; set; } = false;

        public string? VacationId { get; set; }

        /// <summary>
        /// Gets the collection of itinerary details.
        /// </summary>
        public List<ItineraryItemDto> Details { get; init; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO to an <see cref="Itinerary"/> entity.
        /// </summary>
        /// <returns>An <see cref="Itinerary"/> entity.</returns>
        public Itinerary ToItinerary()
        {
            return new Itinerary()
            {
                Id = ItineraryId,
                Date = Date!.Value,
                ItineraryDetails = Details.Select(c => c.ToItineraryItem()).ToList()
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="Itinerary"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="itinerary">The itinerary entity to update.</param>
        public void UpdateVacationItineraryValues(in Itinerary itinerary)
        {
            // Update the date of the itinerary
            itinerary.Date = Date!.Value;
        }

        #endregion
    }
}