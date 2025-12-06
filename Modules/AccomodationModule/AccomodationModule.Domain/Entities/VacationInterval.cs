using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Constants;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a vacation interval, including details about the start and end dates, check-in and check-out times, meal plan type, and associated vacation and lodging.
    /// </summary>
    public class VacationInterval : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInterval"/> class.
        /// </summary>
        public VacationInterval() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInterval"/> class by copying the values from an
        /// existing instance.
        /// </summary>
        /// <param name="vacationInterval">The <see cref="VacationInterval"/> instance from which to copy values. Cannot be <see langword="null"/>.</param>
        public VacationInterval(VacationInterval vacationInterval)
        {
            SortOrderNr = vacationInterval.SortOrderNr;
            NightCount = vacationInterval.NightCount;
            CheckInTime = vacationInterval.CheckInTime;
            CheckOutTime = vacationInterval.CheckOutTime;
            RoomType = vacationInterval.RoomType;
            MealPlan = vacationInterval.MealPlan;
            Description = vacationInterval.Description;
            Notes = vacationInterval.Notes;
            LodgingId = vacationInterval.LodgingId;
            DestinationId = vacationInterval.DestinationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The sort order number for the vacation interval, used to determine the display order in a list.
        /// </summary>
        public int SortOrderNr { get; set; } = 1;

        /// <summary>
        /// Gets or sets the starting day number for the schedule.
        /// </summary>
        public int StartDayNr { get; set; }

        /// <summary>
        /// Gets or sets the number of nights for a reservation or stay.
        /// </summary>
        public int NightCount { get; set; }

        /// <summary>
        /// Gets or sets the check-in time for the vacation interval.
        /// </summary>
        public TimeSpan CheckInTime { get; set; }

        /// <summary>
        /// Gets or sets the check-out time for the vacation interval.
        /// </summary>
        public TimeSpan CheckOutTime { get; set; }

        /// <summary>
        /// Gets or sets the type of room for the vacation interval.
        /// </summary>
        public string RoomType { get; set; } = "Standard";

        /// <summary>
        /// Gets or sets the meal plan type for the vacation interval.
        /// </summary>
        public MealPlanTypes MealPlan { get; set; }

        /// <summary>
        /// Gets or sets the description for the itinerary.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notes for the itinerary.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        #endregion

        #region Read Only

        /// <summary>
        /// Gets a value indicating whether breakfast is included in the meal plan.
        /// </summary>
        public bool BreakfastIncluded => (MealPlan == MealPlanTypes.BedAndBreakfast || MealPlan == MealPlanTypes.DinnerBedAndBreakfast || MealPlan == MealPlanTypes.FullBoard || MealPlan == MealPlanTypes.AllInclusive);
        
        /// <summary>
        /// Gets a value indicating whether lunch is included in the meal plan.
        /// </summary>
        public bool LunchIncluded => (MealPlan == MealPlanTypes.FullBoard || MealPlan == MealPlanTypes.AllInclusive);

        /// <summary>
        /// Gets a value indicating whether dinner is included in the meal plan.
        /// </summary>
        public bool DinnerIncluded => (MealPlan == MealPlanTypes.DinnerBedAndBreakfast || MealPlan == MealPlanTypes.FullBoard || MealPlan == MealPlanTypes.AllInclusive);


        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        /// <summary>
        /// Gets or sets the ID of the default lodging for the itinerary.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string? LodgingId { get; set; }

        /// <summary>
        /// Navigation property to the default lodging for the itinerary.
        /// </summary>
        public Lodging? Lodging { get; set; }

        /// <summary>
        /// Gets or sets the ID of the default lodging for the destination.
        /// </summary>
        [ForeignKey(nameof(Destination))] public string? DestinationId { get; set; }

        /// <summary>
        /// Navigation property to the default lodging for the destination.
        /// </summary>
        public Destination? Destination { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="VacationInterval"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="VacationInterval"/> object that is a copy of this instance, with a unique identifier.</returns>
        public VacationInterval Clone()
        {
            return new VacationInterval(this)
            {
                Id = Guid.NewGuid().ToString()
            };
        }

        #endregion
    }
}
