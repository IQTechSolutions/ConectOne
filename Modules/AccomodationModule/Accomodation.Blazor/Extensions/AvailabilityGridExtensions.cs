using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.RequestFeatures;

namespace Accomodation.Blazor.Extensions
{
	/// <summary>
	/// Provides extension methods for calculating dates related to room availability grids.
	/// </summary>
	public static class AvailabilityGridExtensions
	{
		/// <summary>
		/// Calculates a new date based on the provided model, date range, and day count.
		/// </summary>
		/// <remarks>If the date in <paramref name="model"/> is earlier than <paramref name="dateRange"/>.StartDate,
		/// the method  calculates a date by subtracting <paramref name="dayCount"/> days from the adapted start date of the
		/// range.  If the date in <paramref name="model"/> is later than <paramref name="dateRange"/>.EndDate, the method 
		/// calculates a date by adding <paramref name="dayCount"/> days to the adapted end date of the range.</remarks>
		/// <param name="model">The <see cref="RoomAvailabilityItemViewModel"/> containing the date to evaluate.</param>
		/// <param name="dateRange">The <see cref="LodgingAvailabilityDateRange"/> specifying the start and end dates for the calculation.</param>
		/// <param name="dayCount">The number of days to adjust the date range. Must be a positive integer.</param>
		/// <returns>A <see cref="DateTime"/> representing the calculated date. The returned date is adjusted based on the 
		/// relationship between <paramref name="model"/>.Date and the boundaries of <paramref name="dateRange"/>.</returns>
		public static DateTime GetCalculationDate(RoomAvailabilityItemViewModel model, LodgingAvailabilityDateRange dateRange, int dayCount)
		{
			DateTime newDate = new DateTime();

			if (model.Date < dateRange.StartDate)
			{
				if (dateRange.StartDate.AddDays(-dayCount) > DateTime.Now)
				{
					newDate = dateRange.AddaptedStartDate.AddDays(-dayCount);
				}
			}
			if (model.Date > dateRange.EndDate)
			{
				newDate = dateRange.AddaptedEndDate.AddDays(dayCount);
			}
			return newDate;
		}
	}
}
