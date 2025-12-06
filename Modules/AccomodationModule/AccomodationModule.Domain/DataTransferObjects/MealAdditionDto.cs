using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing additional meal options associated with a vacation.
    /// This class is used to transfer data between layers, such as from the database to the UI or API.
    /// It includes details such as the restaurant, date, time, guest type, meal type, and additional notes.
    /// </summary>
    public record MealAdditionDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAdditionDto"/> class with default values.
        /// </summary>
        public MealAdditionDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAdditionDto"/> class using a <see cref="MealAddition"/> entity.
        /// </summary>
        /// <param name="mealAddition">The <see cref="MealAddition"/> entity to map to this DTO.</param>
        public MealAdditionDto(MealAddition mealAddition)
        {
            MealAdditionId = mealAddition.Id;
            DayNr = mealAddition.DayNr;
            StartTime = mealAddition.StartTime;
            MealAdditionTemPlate = mealAddition.MealAdditionTemplate == null ? null : new MealAdditionTemplateDto(mealAddition.MealAdditionTemplate);
            VacationId = mealAddition.VacationId;
            IntervalInclusion = mealAddition.IntervalInclusion;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the meal addition.
        /// </summary>
        public string MealAdditionId { get; init; }

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
        public string? VacationId { get; set; }

        /// <summary>
        /// The identity of the Vacation Extension this meal addition belongs to
        /// </summary>
        public string? VacationExtensionId { get; set; }

        /// <summary>
        /// Gets or sets the guest type for this meal addition.
        /// </summary>
        public bool IntervalInclusion { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO to a <see cref="MealAddition"/> entity.
        /// </summary>
        /// <returns>A new instance of <see cref="MealAddition"/> populated with the values from this DTO.</returns>
        public MealAddition ToMealAddition()
        {
            return new MealAddition()
            {
                Id = MealAdditionId,
                StartTime = StartTime,
                DayNr = DayNr,
                MealAdditionTemplateId = MealAdditionTemPlate.Id,
                VacationId = VacationId,
                IntervalInclusion = IntervalInclusion
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="MealAddition"/> entity with the values from this DTO.
        /// </summary>
        /// <param name="vacationPrice">The <see cref="MealAddition"/> entity to update.</param>
        public void UpdateMealAdditionValues(in MealAddition vacationPrice)
        {
            vacationPrice.StartTime = StartTime;
            vacationPrice.DayNr = DayNr;
            vacationPrice.MealAdditionTemplateId = MealAdditionTemPlate.Id;
            vacationPrice.IntervalInclusion = IntervalInclusion;
        }

        #endregion
    }
}
