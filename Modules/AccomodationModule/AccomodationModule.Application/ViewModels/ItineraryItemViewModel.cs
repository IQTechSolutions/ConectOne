using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an itinerary item, containing details such as its unique identifier,  associated
    /// itinerary, and description.
    /// </summary>
    /// <remarks>This class is typically used to transfer data related to individual itinerary items between 
    /// application layers, such as from a database or API to the user interface.</remarks>
    public class ItineraryItemViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for an itinerary item.
        /// </summary>
        public string ItineraryItemId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the itinerary.
        /// </summary>
        public string ItineraryId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="ItineraryItemViewModel"/> to an equivalent <see
        /// cref="ItineraryItemDto"/>.
        /// </summary>
        /// <returns>An <see cref="ItineraryItemDto"/> that represents the current instance, including its <see
        /// cref="ItineraryItemId"/>, <see cref="ItineraryId"/>, and <see cref="Description"/> values.</returns>
        public ItineraryItemDto ToDto()
        {
            return new ItineraryItemDto()
            {
                ItineraryItemId = ItineraryItemId,
                ItineraryId = ItineraryId,
                Description = Description
            };
        }

        #endregion
    }
}
