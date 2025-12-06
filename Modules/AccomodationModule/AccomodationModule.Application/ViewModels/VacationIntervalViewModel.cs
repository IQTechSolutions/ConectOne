
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a vacation interval, encapsulating details such as dates, room type, and meal plan.
    /// </summary>
    /// <remarks>This class provides properties to manage and display information about a specific interval
    /// within a vacation, including start and end dates, accommodation details, and additional notes. It can be
    /// initialized with default values or by using a <see cref="VacationIntervalDto"/> to populate its
    /// properties.</remarks>
    public class VacationIntervalViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationIntervalViewModel"/> class with default values.
        /// </summary>
        public VacationIntervalViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationIntervalViewModel"/> class using a <see cref="VacationIntervalDto"/>.
        /// Copies the properties from the DTO to the view model.
        /// </summary>
        /// <param name="dto">The data transfer object containing vacation interval details.</param>
        public VacationIntervalViewModel(VacationIntervalDto dto)
        {
            VacationIntervalId = dto.VacationIntervalId;
            SortOrderNr = dto.SortOrderNr;
            NightCount = dto.NightCount;
            CheckInTime = dto.CheckInTime;
            CheckOutTime = dto.CheckOutTime;
            VacationId = dto.VacationId;
            RoomType = dto.RoomType;
            MealPlan = dto.MealPlan;
            Description = dto.Description;
            Notes = string.IsNullOrEmpty(dto.Notes) ? "" : dto.Notes;
            Lodging = dto.Lodging;
            Destination = dto.Destination ?? new DestinationDto();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the vacation interval ID.
        /// </summary>
        public string? VacationIntervalId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The sort order number for the vacation interval, used to determine the display order in a list.
        /// </summary>
        public int SortOrderNr { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of nights for a reservation or stay.
        /// </summary>
        public int NightCount { get; set; }

        /// <summary>
        /// Gets or sets the check-in time for the vacation interval.
        /// </summary>
        public TimeSpan? CheckInTime { get; set; } = new TimeSpan(16, 0, 0);

        /// <summary>
        /// Gets or sets the check-out time for the vacation interval.
        /// </summary>
        public TimeSpan? CheckOutTime { get; set; } = new TimeSpan(9, 0, 0);

        /// <summary>
        /// Gets or sets the type of room for the vacation interval.
        /// </summary>
        public string RoomType { get; set; } = "Standard";

        /// <summary>
        /// Gets or sets the meal plan type for the vacation interval.
        /// </summary>
        public MealPlanTypes MealPlan { get; set; }

        /// <summary>
        /// Gets or sets the description for the interval.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notes for the itinerary.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// The identity of the vacation that this interval belongs to
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the lodging of the associated vacation.
        /// </summary>
        public LodgingDto Lodging { get; set; } = new();

        /// <summary>
        /// Gets or sets the destination of the associated vacation.
        /// </summary>
        public DestinationDto Destination { get; set; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the adjusted start date of a vacation based on the provided vacation details and the current sort
        /// order.
        /// </summary>
        /// <remarks>If the current sort order number is 1, or if there are no vacation intervals with a
        /// sort order number  less than the current sort order, the method returns the original start date of the
        /// vacation.  Otherwise, the start date is adjusted by adding the night counts of the relevant vacation
        /// intervals  in ascending order of their sort order numbers.</remarks>
        /// <param name="vacation">The vacation details, including the initial start date and any associated vacation intervals.  This
        /// parameter can be <see langword="null"/>.</param>
        /// <returns>The adjusted start date of the vacation as a <see cref="DateTime"/> if the calculation is successful; 
        /// otherwise, <see langword="null"/> if the input vacation or its start date is <see langword="null"/>.</returns>
        public DateTime? StartDate(VacationViewModel? vacation)
        {
            if (vacation?.StartDate == null) return null;
            if (SortOrderNr == 1 || vacation.VacationIntervals is null || !vacation.VacationIntervals.Any(c => c.SortOrderNr < SortOrderNr)) return vacation.StartDate;
            var vacationStartDate = vacation.StartDate;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr).OrderBy(c => c.SortOrderNr))
            {
                vacationStartDate = vacationStartDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
            }
            return vacationStartDate;

        }

        /// <summary>
        /// Calculates and returns the start date string for the current vacation interval based on the provided
        /// vacation details.
        /// </summary>
        /// <remarks>The method determines the start date of the current vacation interval by considering
        /// the <c>SortOrderNr</c> of the intervals. If no prior intervals exist or the start date is not available, it
        /// defaults to "Day 1" or calculates the day number based on the intervals.</remarks>
        /// <param name="vacation">The <see cref="VacationViewModel"/> containing vacation details, including start date and vacation
        /// intervals. Can be <see langword="null"/>.</param>
        /// <returns>A string representing the start date of the current vacation interval in the format "dd-MMM-yyyy", or "Day
        /// X" if the start date cannot be determined. Returns an empty string if <paramref name="vacation"/> is <see
        /// langword="null"/>.</returns>
        public string StartDateString(VacationViewModel? vacation)
        {
            if (vacation == null) return string.Empty;
            if (SortOrderNr == 1 || vacation.VacationIntervals is null || !vacation.VacationIntervals.Any(c => c.SortOrderNr < SortOrderNr)) return vacation.StartDate is not null ? vacation.StartDate.Value.ToString("dd-MMM-yyyy") : "Day 1";
            if (vacation.StartDate != null)
            {
                var vacationStartDate = vacation.StartDate;
                foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr).OrderBy(c => c.SortOrderNr))
                {
                    vacationStartDate = vacationStartDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
                }
                return vacationStartDate.Value.ToString("dd-MMM-yyyy");
            }

            var dayNr = 1;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr < SortOrderNr).OrderBy(c => c.SortOrderNr))
            {
                dayNr = dayNr + interval.NightCount > 0 ? interval.NightCount : 0;
            }
            return $"Day {dayNr}";
        }

        /// <summary>
        /// Calculates the end date of a vacation based on its start date and associated intervals.
        /// </summary>
        /// <remarks>The method iterates through the vacation intervals up to the current sort order
        /// number, adding the specified number of nights to the start date to compute the end date. Intervals with a
        /// non-positive night count are ignored.</remarks>
        /// <param name="vacation">The vacation model containing the start date and a collection of vacation intervals.</param>
        /// <returns>The calculated end date of the vacation, or <see langword="null"/> if the start date is not set or if the
        /// start date cannot be determined.</returns>
        public DateTime? EndDate(VacationViewModel? vacation)
        {
            if (vacation?.StartDate == null) return null;
            if (StartDate(vacation) == null) return null;
            var vacationEndDate = vacation.StartDate;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr <= SortOrderNr).OrderBy(c => c.SortOrderNr))
            {
                vacationEndDate = vacationEndDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
            }
            return vacationEndDate.Value;
        }

        /// <summary>
        /// Calculates and returns the end date of a vacation as a formatted string.
        /// </summary>
        /// <remarks>The method calculates the end date by iterating through the vacation intervals  with
        /// a sort order number less than or equal to the current instance's <c>SortOrderNr</c>.  If the start date is
        /// not available, the method calculates the day number instead.</remarks>
        /// <param name="vacation">The <see cref="VacationViewModel"/> instance containing the vacation details.  If <paramref
        /// name="vacation"/> is <see langword="null"/>, an empty string is returned.</param>
        /// <returns>A string representing the end date of the vacation. If the start date is not set,  the method returns "Day
        /// 1" or a calculated day number based on the vacation intervals.  If the start date is set, the method returns
        /// the end date formatted as "dd-MMM-yyyy".</returns>
        public string EndDateString(VacationViewModel? vacation)
        {
            if (vacation == null) return string.Empty;
            if (string.IsNullOrEmpty(StartDateString(vacation))) return "Day 1";
            if (vacation.StartDate != null)
            {
                var vacationEndDate = vacation.StartDate;
                foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr <= SortOrderNr).OrderBy(c => c.SortOrderNr))
                {
                    vacationEndDate = vacationEndDate.Value.AddDays(interval.NightCount > 0 ? interval.NightCount : 0);
                }
                return vacationEndDate.Value.ToString("dd-MMM-yyyy");
            }
            var dayNr = 1;
            foreach (var interval in vacation.VacationIntervals.Where(c => c.SortOrderNr <= SortOrderNr).OrderBy(c => c.SortOrderNr))
            {
                dayNr = dayNr + interval.NightCount > 0 ? interval.NightCount : 0;
            }
            return $"Day {dayNr}";
        }

        /// <summary>
        /// Converts the current <see cref="VacationInterval"/> instance to a <see cref="VacationIntervalDto"/> object.
        /// </summary>
        /// <remarks>This method maps all relevant properties from the <see cref="VacationInterval"/>
        /// instance to a new <see cref="VacationIntervalDto"/> object. Use this method to transfer vacation interval
        /// data in a format suitable for data transfer or serialization.</remarks>
        /// <returns>A <see cref="VacationIntervalDto"/> object that represents the current vacation interval, including its
        /// associated properties.</returns>
        public VacationIntervalDto ToDto()
        {
            return new VacationIntervalDto()
            {
                VacationIntervalId = VacationIntervalId,
                SortOrderNr = SortOrderNr,
                NightCount = NightCount,
                CheckInTime = CheckInTime,
                CheckOutTime = CheckOutTime,
                RoomType = RoomType,
                MealPlan = MealPlan,
                Description = Description,
                Notes = Notes,
                Lodging = Lodging,
                Destination = Destination,
                VacationId = VacationId
            };
        }

        #endregion

    }
}
