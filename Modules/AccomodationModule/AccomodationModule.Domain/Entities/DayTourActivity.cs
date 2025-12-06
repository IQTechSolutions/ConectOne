using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a day tour activity associated with a vacation.
    /// Includes details such as name, date, start time, summary, description, and guest type.
    /// </summary>
    public class DayTourActivity : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivity"/> class.
        /// </summary>
        public DayTourActivity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivity"/> class by copying the values from an existing
        /// instance.
        /// </summary>
        /// <param name="dayTourActivity">The <see cref="DayTourActivity"/> instance from which to copy values. Cannot be <see langword="null"/>.</param>
        public DayTourActivity(DayTourActivity dayTourActivity)
        {
            DayNr = dayTourActivity.DayNr;
            StartTime = dayTourActivity.StartTime;
            DayTourActivityTemplateId = dayTourActivity.DayTourActivityTemplateId;
            VacationId = dayTourActivity.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the day tour activity.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated day tour activity template.
        /// </summary>
        [ForeignKey(nameof(DayTourActivityTemplate))] public string? DayTourActivityTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template for a day tour activity.
        /// </summary>
        public DayTourActivityTemplate? DayTourActivityTemplate { get; set; }

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
        /// Creates a new instance of the <see cref="DayTourActivity"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="DayTourActivity"/> object that is a copy of this instance.</returns>
        public DayTourActivity Clone()
        {
            return new DayTourActivity(this) {Id = Guid.NewGuid().ToString()};
        }

        #endregion


    }
}
