using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an additional meal option associated with a vacation.
    /// Includes details such as the restaurant, date, time, guest type, and notes.
    /// </summary>
    public class MealAddition : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAddition"/> class.
        /// </summary>
        public MealAddition() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAddition"/> class by copying the properties from an
        /// existing instance.
        /// </summary>
        /// <param name="addition">The <see cref="MealAddition"/> instance from which to copy the properties. Cannot be null.</param>
        public MealAddition(MealAddition addition)
        {
            DayNr = addition.DayNr;
            StartTime = addition.StartTime;
            MealAdditionTemplateId = addition.MealAdditionTemplateId;
            IntervalInclusion = addition.IntervalInclusion;
            VacationId = addition.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the meal addition.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the meal addition.
        /// </summary>
        public bool IntervalInclusion { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated meal addition template.
        /// </summary>
        [ForeignKey(nameof(MealAdditionTemplate))] public string? MealAdditionTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used to define additional meal options.
        /// </summary>
        public MealAdditionTemplate MealAdditionTemplate { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation.
        /// Maximum length: 100 characters.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }
        
        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="MealAddition"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="MealAddition"/> object that is a copy of this instance.</returns>
        public MealAddition Clone()
        {
            return new MealAddition(this);
        }

        #endregion
    }
}
