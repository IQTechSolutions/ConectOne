using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CalendarModule.Domain.Enums;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using IdentityModule.Domain.Entities;

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents an appointment or task scheduled at a certain time and possibly recurring.
    /// Inherits from EntityBase<string> to provide an ID and common entity fields.
    /// </summary>
    public class Appointment : EntityBase<string>
    {
        /// <summary>
        /// Specifies how often the appointment repeats (None, Daily, Weekly, Monthly, Yearly).
        /// Used by the CalculatedStartDate property to determine the next occurrence.
        /// </summary>
        public Recurrence RecurrenceRule { get; set; } = Recurrence.None;

        /// <summary>
        /// Optional formula specifying the recurrence pattern.
        /// </summary>
        public string? RecurrenceFormula { get; set; }

        /// <summary>
        /// Indicates the relative importance or urgency of the appointment (Low, Medium, High).
        /// This can be used to prioritize tasks or appointments.
        /// </summary>
        public Priority Priority { get; set; } = Priority.Medium;

        /// <summary>
        /// A required title or brief summary describing what the appointment is about.
        /// </summary>
        [Required] public string Heading { get; set; } = null!;

        /// <summary>
        /// Optional detailed description of the appointment, could include notes, instructions, or objectives.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the color associated with the object.
        /// </summary>
        public string Color { get; set; } = "#1e90ff";

        /// <summary>
        /// The start date of the appointment, defaulting to today's date.
        /// </summary>
        [DataType(DataType.Date)] public DateTime StartDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// The specific time of day the appointment begins, combined with StartDate for full start timestamp.
        /// Defaults to the current time.
        /// </summary>
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        /// <summary>
        /// The end date of the appointment. If the appointment spans multiple days, 
        /// this date will be after StartDate. Otherwise, it can be the same day.
        /// Defaults to today's date.
        /// </summary>
        [DataType(DataType.Date)] public DateTime EndDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// The end time of the appointment. Defaults to the end of the day (23:59:59).
        /// Combined with EndDate for full end timestamp.
        /// </summary>
        public TimeSpan EndTime { get; set; } = new TimeSpan(0, 23, 59, 59);
        

        /// <summary>
        /// The current status of the appointment (e.g., ToDo, InProgress, Completed, Cancelled).
        /// </summary>
        public TaskResultStatus Status { get; set; } = TaskResultStatus.ToDo;

        /// <summary>
        /// Controls who can access the appointment.
        /// </summary>
        public AppointmentAudienceType AudienceType { get; set; } = AppointmentAudienceType.Public;

        /// <summary>
        /// Convenience property indicating if the appointment's status is Completed.
        /// Returns true if Status == TaskResultStatus.Completed.
        /// </summary>
        public bool Completed => Status == TaskResultStatus.Completed;

        /// <summary>
        /// Convenience property indicating if the appointment's status is Cancelled.
        /// Returns true if Status == TaskResultStatus.Cancelled.
        /// </summary>
        public bool Canceled => Status == TaskResultStatus.Cancelled;

        /// <summary>
        /// Foreign key linking this appointment to a particular UserInfo entity (the user who owns this appointment).
        /// </summary>
        [DisplayName("User Id"), ForeignKey(nameof(User))] public string? UserId { get; set; }

        /// <summary>
        /// Navigation property for the user who owns this appointment.
        /// Allows retrieving details about the user to whom this appointment belongs.
        /// </summary>
        public UserInfo? User { get; set; }

        /// <summary>
        /// Calculates the appropriate start date based on the recurrence rule and the current date.
        /// If the original StartDate is in the past, and Recurrence is set, this logic calculates 
        /// the next occurrence date. For example:
        /// - Daily: Adds enough days to reach the next occurrence after today.
        /// - Weekly: Calculates the next week's matching day of the week.
        /// - Monthly: Finds the next month that matches the start date day.
        /// - Yearly: Moves to the next year if needed.
        /// If no recurrence or StartDate is already in the future, just returns StartDate.
        /// </summary>
        public DateTime CalculatedStartDate
        {
            get
            {
                if (StartDate < DateTime.Now)
                {
                    // The event start date is in the past. We need to adjust based on Recurrence.
                    var daysDifference = (DateTime.Now - StartDate).TotalDays;

                    switch (RecurrenceRule)
                    {
                        case Recurrence.Daily:
                            // Move to next day occurrence if start was yesterday or before
                            return StartDate.AddDays(daysDifference + 1);

                        case Recurrence.Weekly:
                            // Calculate how many days to add to align next occurrence with a weekday cycle.
                            int extraDays;
                            if (StartDate.DayOfWeek > DateTime.Now.DayOfWeek)
                            {
                                // If the start weekday is after today's weekday, wrap around to next week
                                extraDays = 7 - (DateTime.Now.DayOfWeek - StartDate.DayOfWeek);
                            }
                            else
                            {
                                // Direct calculation if today's weekday is after start's weekday
                                extraDays = StartDate.DayOfWeek - DateTime.Now.DayOfWeek;
                            }
                            return StartDate.AddDays(daysDifference + extraDays);

                        case Recurrence.Monthly:
                            // Monthly events occur on the same day each month
                            var monthsToAdd = 0;
                            if (StartDate.Month < DateTime.Now.Month)
                            {
                                monthsToAdd = DateTime.Now.Month - StartDate.Month;
                            }

                            // If after adding those months, date is still in the past, add another month
                            if (StartDate.AddMonths(monthsToAdd) < DateTime.Now)
                            {
                                monthsToAdd++;
                            }
                            return StartDate.AddMonths(monthsToAdd);

                        case Recurrence.Yearly:
                            // Yearly events repeat on the same date each year
                            var yearsToAdd = 0;
                            if (StartDate.Year < DateTime.Now.Year)
                            {
                                yearsToAdd = DateTime.Now.Year - StartDate.Year;
                            }

                            // If after adding those years, date is still in the past, add another year
                            if (StartDate.AddYears(yearsToAdd) < DateTime.Now)
                            {
                                yearsToAdd++;
                            }
                            return StartDate.AddYears(yearsToAdd);

                        default:
                            // If no recurrence or unhandled recurrence, return StartDate
                            return StartDate;
                    }
                }

                // If StartDate is not in the past (or no recurrence), no need to adjust
                return StartDate;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the event spans the entire day.
        /// </summary>
        public bool FullDayEvent { get; set; }

        /// <summary>
        /// Gets or sets the invitations that target individual users.
        /// </summary>
        public ICollection<AppointmentUserInvite> UserInvites { get; set; } = new List<AppointmentUserInvite>();

        /// <summary>
        /// Gets or sets the invitations that target specific roles.
        /// </summary>
        public ICollection<AppointmentRoleInvite> RoleInvites { get; set; } = new List<AppointmentRoleInvite>();
    }
}
