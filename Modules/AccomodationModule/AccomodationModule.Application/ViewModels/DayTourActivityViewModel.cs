using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for managing day tour activity details associated with a vacation.
    /// Provides properties for binding day tour activity data in the UI.
    /// </summary>
    public class DayTourActivityViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityViewModel"/> class with default values.
        /// </summary>
        public DayTourActivityViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityViewModel"/> class using a <see cref="DayTourActivityDto"/>.
        /// Copies the properties from the DTO to the view model.
        /// </summary>
        /// <param name="dayTourActivity">The data transfer object containing day tour activity details.</param>
        public DayTourActivityViewModel(DayTourActivityDto dayTourActivity)
        {
            DayTourActivityId = string.IsNullOrEmpty(dayTourActivity.DayTourActivityId) ? Guid.NewGuid().ToString() : dayTourActivity.DayTourActivityId;
            DayNr = dayTourActivity.DayNr;
            StartTime = dayTourActivity.StartTime;
            DayTourActivityTemplate = dayTourActivity.DayTourActivityTemplate;
            VacationId = dayTourActivity.VacationId;
            VacationExtensionId = dayTourActivity.VacationExtensionId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the day tour activity.
        /// </summary>
        public string? DayTourActivityId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the day tour activity.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the Day Tour Activity Template.
        /// </summary>
        public DayTourActivityTemplateDto? DayTourActivityTemplate { get; set; }

        /// <summary>
        /// The identity of the vacation that this activity belongs to.
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// The identity of the vacation extension that this activity belongs to.
        /// </summary>
        public string? VacationExtensionId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="DayTourActivity"/> to a <see cref="DayTourActivityDto"/>.
        /// </summary>
        /// <returns>A <see cref="DayTourActivityDto"/> that represents the current instance, including its associated properties
        /// such as <see cref="DayTourActivityId"/>, <see cref="DayNr"/>, <see cref="StartTime"/>, <see
        /// cref="DayTourActivityTemplate"/>, <see cref="VacationId"/>, and <see cref="VacationExtensionId"/>.</returns>
        public DayTourActivityDto ToDto()
        {
            return new DayTourActivityDto
            {
                DayTourActivityId = DayTourActivityId,
                DayNr = DayNr,
                StartTime = StartTime,
                DayTourActivityTemplate = DayTourActivityTemplate,
                VacationId = VacationId,
                VacationExtensionId = VacationExtensionId
            };
        }

        #endregion
    }
}