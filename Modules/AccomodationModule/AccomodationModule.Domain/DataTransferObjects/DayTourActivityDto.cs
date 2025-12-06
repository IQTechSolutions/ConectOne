using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a day tour activity.
    /// Provides a lightweight representation of the <see cref="DayTourActivity"/> entity
    /// for use in data transfer between layers or systems.
    /// </summary>
    public class DayTourActivityDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityDto"/> class with default values.
        /// </summary>
        public DayTourActivityDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityDto"/> class using a <see cref="DayTourActivity"/> entity.
        /// Copies the properties from the entity to the DTO.
        /// </summary>
        /// <param name="dayTourActivity">The <see cref="DayTourActivity"/> entity to copy data from.</param>
        public DayTourActivityDto(DayTourActivity dayTourActivity)
        {
            DayTourActivityId = dayTourActivity.Id;
            DayNr = dayTourActivity.DayNr;
            StartTime = dayTourActivity.StartTime;
            DayTourActivityTemplate = dayTourActivity.DayTourActivityTemplate == new DayTourActivityTemplate() ? new DayTourActivityTemplateDto() : new DayTourActivityTemplateDto(dayTourActivity.DayTourActivityTemplate);
            VacationId = dayTourActivity.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the day tour activity.
        /// </summary>
        public string DayTourActivityId { get; init; } = null!;

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the day tour activity.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets the description of the day tour activity.
        /// </summary>
        public DayTourActivityTemplateDto DayTourActivityTemplate { get; init; }

        /// <summary>
        /// The identity of the vacation that this activity belongs to.
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// The identity of the vacation that this activity belongs to.
        /// </summary>
        public string? VacationExtensionId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO into a <see cref="DayTourActivity"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="DayTourActivity"/> object with the same data.</returns>
        public DayTourActivity ToDayTourActivity()
        {
            return new DayTourActivity()
            {
                Id = DayTourActivityId,
                DayNr = DayNr,
                StartTime = StartTime,
                DayTourActivityTemplateId = DayTourActivityTemplate.Id,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="DayTourActivity"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="dayTourActivity">The day tour that needs to be updated</param>
        public void UpdateDayTourActivityValues(in DayTourActivity dayTourActivity)
        {
            dayTourActivity.DayNr = DayNr;
            dayTourActivity.StartTime = StartTime;
            dayTourActivity.DayTourActivityTemplateId = DayTourActivityTemplate.Id;
        }

        #endregion
    }
}
