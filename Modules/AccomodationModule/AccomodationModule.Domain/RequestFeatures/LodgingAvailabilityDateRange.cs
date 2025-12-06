using ConectOne.Domain.Extensions;

namespace AccomodationModule.Domain.RequestFeatures
{
	/// <summary>
	/// Represents a date range for lodging availability, including methods and properties to adapt and evaluate the range.
	/// </summary>
	/// <remarks>This class provides functionality to define a start and end date for lodging availability, with
	/// additional adapted date calculations and utilities for checking date availability within the range.</remarks>
	public class LodgingAvailabilityDateRange
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LodgingAvailabilityDateRange"/> class.
		/// </summary>
		public LodgingAvailabilityDateRange() { }

		/// <summary>
		/// Represents a date range for lodging availability.
		/// </summary>
		/// <remarks>This class is used to define a specific range of dates during which lodging is available. Ensure
		/// that the <paramref name="start"/> date is not later than the <paramref name="end"/> date to avoid invalid
		/// ranges.</remarks>
		/// <param name="start">The start date of the availability range. Must be earlier than or equal to <paramref name="end"/>.</param>
		/// <param name="end">The end date of the availability range. Must be later than or equal to <paramref name="start"/>.</param>
		public LodgingAvailabilityDateRange(DateTime start, DateTime end) 
		{
			StartDate = start;
			EndDate = end;
		}

		#endregion

		/// <summary>
		/// Gets the adjusted start date based on the current date and predefined conditions.
		/// </summary>
		public DateTime AddaptedStartDate
		{
			get
			{
				if (DateTime.Now.AddDays(1).Date < StartDate.AddDays(-2))
					return StartDate.AddDays(-2);
				if (DateTime.Now.AddDays(1).Date < StartDate.AddDays(-1))
					return StartDate.AddDays(-1);
				return StartDate;
			}
		}

		/// <summary>
		/// Gets or sets the start date for the event or operation.
		/// </summary>
		public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1).Date;

		/// <summary>
		/// Gets or sets the end date for the operation or event.
		/// </summary>
		public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1).Date;

		/// <summary>
		/// Gets the adjusted end date, which is calculated by adding two days to the original end date.
		/// </summary>
		public DateTime AddaptedEndDate => EndDate.AddDays(2);

		/// <summary>
		/// Gets a list of dates between the adapted start date and the adapted end date.
		/// </summary>
		/// <remarks>The list is generated dynamically based on the values of <c>AddaptedStartDate</c> and
		/// <c>AddaptedEndDate</c>.  Ensure that these dates are properly set before accessing this property.</remarks>
		public List<DateTime> DateList => DateTimeExtensions.GetDatesBetween(AddaptedStartDate, AddaptedEndDate);

		/// <summary>
		/// Determines whether the specified date falls within the range defined by the start and end dates.
		/// </summary>
		/// <remarks>The range is inclusive of the start date and exclusive of the end date.</remarks>
		/// <param name="date">The date to evaluate.</param>
		/// <returns><see langword="true"/> if the specified date is greater than or equal to the start date and less than the end
		/// date;  otherwise, <see langword="false"/>.</returns>
		public bool IsDateAvailableBetweenStartAndEndDates(DateTime date)
		{
			return date >= StartDate && date < EndDate;
		}
	}
}
