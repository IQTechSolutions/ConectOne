using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object for a vacation interval, encapsulating details such as dates, lodging, and
    /// meal plans.
    /// </summary>
    /// <remarks>This DTO is used to transfer vacation interval data between different layers of the
    /// application. It can be initialized with default values or populated from a <see cref="VacationInterval"/> entity.</remarks>
    public record VacationIntervalDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationIntervalDto"/> class with default values.
        /// </summary>
        public VacationIntervalDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationIntervalDto"/> class using a <see cref="VacationIntervalViewModel"/>.
        /// Copies the properties from the view model to the DTO.
        /// </summary>
        /// <param name="model">The view model containing vacation interval details.</param>
        public VacationIntervalDto(VacationInterval model)
        {
            VacationIntervalId = model.Id;
            SortOrderNr = model.SortOrderNr;
            NightCount = model.NightCount;
            CheckInTime = model.CheckInTime;
            CheckOutTime = model.CheckOutTime;
            RoomType = model.RoomType;
            MealPlan = model.MealPlan;
            Description = model.Description;
            Notes = model.Notes;

            if(model.Lodging is not null)
                Lodging = new LodgingDto(model.Lodging, new PricingDto());

            if(model.Destination is not null)
                Destination = new DestinationDto(model.Destination);
        }

        #endregion

        /// <summary>
        /// Gets or sets the vacation interval ID.
        /// </summary>
        public string? VacationIntervalId { get; init; } = null!;
        
        /// <summary>
        /// The sort order number for the vacation interval, used to determine the display order in a list.
        /// </summary>
        public int SortOrderNr { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of nights for a reservation or stay.
        /// </summary>
        public int NightCount { get; init; }

        /// <summary>
        /// Gets or sets the check-in time for the vacation interval.
        /// </summary>
        public TimeSpan? CheckInTime { get; init; }

        /// <summary>
        /// Gets or sets the check-out time for the vacation interval.
        /// </summary>
        public TimeSpan? CheckOutTime { get; init; }

        /// <summary>
        /// Gets or sets the type of room for the vacation interval.
        /// </summary>
        public string? RoomType { get; init; } = "Standard";

        /// <summary>
        /// Gets or sets the meal plan type for the vacation interval.
        /// </summary>
        public MealPlanTypes MealPlan { get; init; }

        public bool BreakfastIncluded => MealPlan == MealPlanTypes.BedAndBreakfast || MealPlan == MealPlanTypes.DinnerBedAndBreakfast || MealPlan == MealPlanTypes.FullBoard || MealPlan == MealPlanTypes.AllInclusive;

        /// <summary>
        /// Gets or sets the description for the itinerary.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notes for the itinerary.
        /// </summary>
        public string? Notes { get; init; }

        /// <summary>
        /// Gets or sets the lodging information for the vacation interval.
        /// </summary>
        public LodgingDto? Lodging { get; init; } = null!;

        /// <summary>
        /// Gets or sets the destination information for the vacation interval.
        /// </summary>
        public DestinationDto? Destination { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string? VacationId { get; set; }

        #region Methods

        /// <summary>
        /// Calculates the adjusted start date of a vacation based on the provided vacation details and the current sort
        /// order.
        /// </summary>
        /// <remarks>If the current sort order is 1, or if there are no vacation intervals with a sort
        /// order less than the current sort order,  the method returns the original start date. Otherwise, the start
        /// date is adjusted by adding the night counts of the relevant intervals.</remarks>
        /// <param name="vacation">The vacation details, including the initial start date and any associated vacation intervals.  This
        /// parameter can be <see langword="null"/>.</param>
        /// <returns>The adjusted start date of the vacation as a <see cref="DateTime"/> if the calculation is successful; 
        /// otherwise, <see langword="null"/> if the input vacation or its start date is <see langword="null"/>.</returns>
        public DateTime? StartDate(VacationDto? vacation)
        {
            if (vacation?.StartDate == null) return null;
            if (SortOrderNr == 1 || vacation.VacationIntervals is null || !vacation.VacationIntervals.Any(c => c.SortOrderNr < SortOrderNr && SortOrderNr > 0)) return vacation.StartDate;
            var vacationStartDate = vacation.StartDate;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr && SortOrderNr > 0).OrderBy(c => c.SortOrderNr))
            {
                vacationStartDate = vacationStartDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
            }
            return vacationStartDate;

        }

        /// <summary>
        /// Calculates the start date string for the current vacation interval based on the provided vacation data.
        /// </summary>
        /// <remarks>The method determines the start date string by evaluating the vacation's start date
        /// and intervals relative to the current interval's sort order. If no prior intervals exist or the start date
        /// is not set, it defaults to "Day 1".</remarks>
        /// <param name="vacation">The <see cref="VacationViewModel"/> containing vacation details, including start date and intervals. Can be
        /// <see langword="null"/>.</param>
        /// <returns>A string representing the start date of the current vacation interval in the format "dd-MMM-yyyy", or "Day
        /// X" if the start date cannot be determined. Returns an empty string if <paramref name="vacation"/> is <see
        /// langword="null"/>.</returns>
        public string StartDateString(VacationDto? vacation)
        {
            if (vacation == null) return string.Empty;
            if (SortOrderNr == 1 || vacation.VacationIntervals is null || !vacation.VacationIntervals.Any(c => c.SortOrderNr < SortOrderNr && SortOrderNr > 0)) return vacation?.StartDate is not null ? vacation.StartDate.Value.ToString("dd-MMM-yyyy") : "Day 1";
            if (vacation.StartDate != null)
            {
                var vacationStartDate = vacation.StartDate;
                foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr && SortOrderNr > 0).OrderBy(c => c.SortOrderNr))
                {
                    vacationStartDate = vacationStartDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
                }
                return vacationStartDate.Value.ToString("dd-MMM-yyyy");
            }

            var dayNr = 1;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr && SortOrderNr > 0).OrderBy(c => c.SortOrderNr))
            {
                dayNr = dayNr + (interval.NightCount > 0 ? interval.NightCount : 0);
            }
            return $"Day {dayNr}";
        }

        /// <summary>
        /// Calculates the end date of a vacation based on its start date and associated vacation intervals.
        /// </summary>
        /// <remarks>The method iterates through the vacation intervals up to the current sort order
        /// number,  adding the specified number of nights to the start date to calculate the end date.  Intervals with
        /// a non-positive night count are ignored.</remarks>
        /// <param name="vacation">The vacation model containing the start date and a collection of vacation intervals.  The <paramref
        /// name="vacation"/> parameter must not be null, and its <see cref="VacationViewModel.StartDate"/> property
        /// must be set.</param>
        /// <returns>The calculated end date of the vacation as a <see cref="DateTime"/> if the start date and intervals are
        /// valid;  otherwise, <see langword="null"/>.</returns>
        public DateTime? EndDate(VacationDto? vacation)
        {
            return StartDate(vacation) is null ? null : StartDate(vacation)?.AddDays(NightCount);
        }

        /// <summary>
        /// Calculates and returns the end date of a vacation as a formatted string.
        /// </summary>
        /// <remarks>The method calculates the end date by iterating through the vacation intervals  with
        /// a sort order number less than or equal to the current instance's <c>SortOrderNr</c>.  If the start date is
        /// not available, the method calculates the day number instead.</remarks>
        /// <param name="vacation">The <see cref="VacationViewModel"/> instance containing vacation details.  If <paramref name="vacation"/> is
        /// <see langword="null"/>, an empty string is returned.</param>
        /// <returns>A string representing the end date of the vacation. If the start date is not set,  the method returns "Day
        /// 1" or a calculated day number based on the vacation intervals.  Otherwise, it returns the end date formatted
        /// as "dd-MMM-yyyy".</returns>
        public string? EndDateString(VacationDto? vacation)
        {
            if (vacation.StartDate is null)
            {
                var dayNr = 1;
                foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr <= SortOrderNr && SortOrderNr > 0).OrderBy(c => c.SortOrderNr))
                {
                    dayNr = dayNr + (interval.NightCount > 0 ? interval.NightCount : 0);
                }
                return $"Day {dayNr}";
            }
            if (EndDate(vacation) is null || !vacation.VacationIntervals.Any()) return "Day 1";
            return EndDate(vacation)!.Value.ToString("dd-MMM-yyyy");
        }

        /// <summary>
        /// Converts the current DTO into a <see cref="VacationInterval"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="VacationInterval"/> entity with the DTO's data.</returns>
        public VacationInterval ToVacationInterval()
        {
            return new VacationInterval()
            {
                Id = VacationIntervalId,
                SortOrderNr = SortOrderNr,
                NightCount = NightCount,
                CheckInTime = CheckInTime ?? new TimeSpan(16, 0, 0),
                CheckOutTime = CheckOutTime ?? new TimeSpan(9, 0, 0),
                RoomType = RoomType,
                MealPlan = MealPlan,
                Description = Description,
                Notes = string.IsNullOrEmpty(Notes) ? "" : Notes,
                LodgingId = Lodging.ProductId,
                DestinationId = Destination?.DestinationId,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="VacationInterval"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="interval">The vacation interval entity to update.</param>
        public void UpdateVacationIntervalValues(in VacationInterval interval)
        {
            interval.Id = VacationIntervalId;
            interval.SortOrderNr = SortOrderNr;
            interval.NightCount = NightCount;
            interval.CheckInTime = CheckInTime ?? new TimeSpan(16,0,0);
            interval.CheckOutTime = CheckOutTime ?? new TimeSpan(9, 0, 0); 
            interval.RoomType = RoomType;
            interval.MealPlan = MealPlan;
            interval.Notes = string.IsNullOrEmpty(Notes) ? "" : Notes;
            interval.Description = Description;
            interval.LodgingId = Lodging.ProductId;
            interval.DestinationId = Destination?.DestinationId;
        }

        #endregion
    }
}
