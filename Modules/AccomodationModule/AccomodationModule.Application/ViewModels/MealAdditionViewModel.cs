using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing additional meal options associated with a vacation.
    /// This class is used to bind data between the UI and the application logic.
    /// It includes details such as the restaurant, date, time, guest type, meal type, and additional notes.
    /// </summary>
    public class MealAdditionViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAdditionViewModel"/> class with default values.
        /// </summary>
        public MealAdditionViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAdditionViewModel"/> class using a <see cref="MealAdditionDto"/>.
        /// </summary>
        /// <param name="dto">The <see cref="MealAdditionDto"/> to map to this ViewModel.</param>
        public MealAdditionViewModel(MealAdditionDto dto)
        {
            MealAdditionId = dto.MealAdditionId;
            DayNr = dto.DayNr;
            StartTime = dto.StartTime;
            VacationId = dto.VacationId;
            VacationExtensionId = dto.VacationExtensionId;
            MealAdditionTemPlate = dto.MealAdditionTemPlate;
            IntervalInclusion = dto.IntervalInclusion;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the meal addition.
        /// </summary>
        public string MealAdditionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the day tour activity.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the restaurant associated with this meal addition.
        /// </summary>
        public MealAdditionTemplateDto MealAdditionTemPlate { get; set; }

        /// <summary>
        /// The identity of the Vacation this meal addition belongs to
        /// </summary>
        public string VacationId { get; set; }

        /// <summary>
        /// The identity of the Vacation Extension this meal addition belongs to
        /// </summary>
        public string VacationExtensionId { get; set; }

        /// <summary>
        /// Gets or sets the guest type for this meal addition.
        /// </summary>
        public bool IntervalInclusion { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the <see cref="MealAddition"/> class to a <see cref="MealAdditionDto"/>
        /// object.
        /// </summary>
        /// <returns>A <see cref="MealAdditionDto"/> object that represents the current instance, including all relevant
        /// properties.</returns>
        public MealAdditionDto ToDto()
        {
            return new MealAdditionDto
            {
                MealAdditionId = this.MealAdditionId,
                DayNr = this.DayNr,
                StartTime = this.StartTime,
                VacationId = this.VacationId,
                VacationExtensionId = this.VacationExtensionId,
                MealAdditionTemPlate = this.MealAdditionTemPlate,
                IntervalInclusion = this.IntervalInclusion
            };
        }

        #endregion
    }
}
