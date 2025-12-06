using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for Itinerary Items.
    /// </summary>
    public class ItineraryItemDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryItemDto"/> class.
        /// </summary>
        public ItineraryItemDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryItemDto"/> class based on an <see cref="ItineraryItem"/> model.
        /// </summary>
        /// <param name="model">The <see cref="ItineraryItem"/> model to create the DTO from.</param>
        public ItineraryItemDto(ItineraryItem model)
        {
            ItineraryItemId = model.Id;
            ItineraryId = model.ItineraryId!;
            Description = model.Description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the itinerary item ID.
        /// </summary>
        public string? ItineraryItemId { get; init; }

        /// <summary>
        /// Gets the itinerary ID.
        /// </summary>
        public string ItineraryId { get; init; } = null!;

        /// <summary>
        /// Gets or sets the description of the itinerary item.
        /// </summary>
        public string Description { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO to an <see cref="ItineraryItem"/> entity.
        /// </summary>
        /// <returns>An <see cref="ItineraryItem"/> entity.</returns>
        public ItineraryItem ToItineraryItem()
        {
            return new ItineraryItem()
            {
                Id = ItineraryItemId,
                ItineraryId = ItineraryId,
                Description = Description
            };
        }

        #endregion
    }
}
