using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an itinerary entry item template.
    /// </summary>
    /// <remarks>This class is used to transfer data related to itinerary entry item templates between
    /// different layers of the application. It provides properties for identifying the entry, associating it with a
    /// vacation, and specifying its content and day number.</remarks>
    public class ItineraryEntryItemTemplateDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryEntryItemTemplateDto"/> class.
        /// </summary>
        public ItineraryEntryItemTemplateDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryEntryItemTemplateDto"/> class using the specified
        /// itinerary entry item template.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="ItineraryEntryItemTemplate"/> to the corresponding properties of the <see
        /// cref="ItineraryEntryItemTemplateDto"/>.</remarks>
        /// <param name="item">The itinerary entry item template used to populate the properties of the DTO.</param>
        public ItineraryEntryItemTemplateDto(ItineraryEntryItemTemplate item)
        {
            Id = item.Id;
            DayNr = item.DayNr;
            Content = item.Content;
            VacationId = item.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the day number represented by this instance.
        /// </summary>
        public int DayNr { get; init; }

        /// <summary>
        /// Gets the content associated with this instance.
        /// </summary>
        public string? Content { get; init; }

        /// <summary>
        /// Gets the unique identifier for the vacation.
        /// </summary>
        public string? VacationId { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current object to an instance of <see cref="ItineraryEntryItemTemplate"/>.
        /// </summary>
        /// <returns>A new <see cref="ItineraryEntryItemTemplate"/> instance populated with the values of the current object.</returns>
        public ItineraryEntryItemTemplate ToEntity()
        {
            return new ItineraryEntryItemTemplate
            {
                Id = Id,
                DayNr = DayNr,
                Content = Content,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the specified itinerary entry item template with the current day number and content.
        /// </summary>
        /// <param name="item">The itinerary entry item template to update. Must not be null.</param>
        /// <returns>The updated itinerary entry item template.</returns>
        public ItineraryEntryItemTemplate UpdateEntity(in ItineraryEntryItemTemplate item)
        {
            item.DayNr = DayNr;
            item.Content = Content;

            return item;
        }

        #endregion
    }
}
