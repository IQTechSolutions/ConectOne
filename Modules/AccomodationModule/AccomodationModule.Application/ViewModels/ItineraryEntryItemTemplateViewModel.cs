using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an itinerary entry item template, containing details such as the entry's unique
    /// identifier, day number, and content.
    /// </summary>
    /// <remarks>This class is designed to be used as a data container for itinerary entry templates,
    /// typically in scenarios where itinerary details need to be displayed or manipulated in a user
    /// interface.</remarks>
    public class ItineraryEntryItemTemplateViewModel 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryEntryItemTemplateViewModel"/> class.
        /// </summary>
        public ItineraryEntryItemTemplateViewModel() { }

        /// <summary>
        /// Represents a view model for an itinerary entry item template.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the itinerary entry item template. Must not be <see
        /// langword="null"/>.</param>
        public ItineraryEntryItemTemplateViewModel(ItineraryEntryItemTemplateDto dto)
        {
            Id = dto.Id;
            DayNr = dto.DayNr;
            Content = dto.Content;
            VacationId = dto.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; } 

        /// <summary>
        /// Gets or sets the content associated with this instance.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets the unique identifier for the vacation.
        /// </summary>
        public string? VacationId { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to an <see cref="ItineraryEntryItemTemplateDto"/>.
        /// </summary>
        /// <returns>An <see cref="ItineraryEntryItemTemplateDto"/> that represents the current instance,  including its
        /// identifier, day number, content, and associated vacation ID.</returns>
        public ItineraryEntryItemTemplateDto ToDto()
        {
            return new ItineraryEntryItemTemplateDto
            {
                Id = this.Id,
                DayNr = this.DayNr,
                Content = this.Content,
                VacationId = this.VacationId
            };
        }

        #endregion
    }
}
