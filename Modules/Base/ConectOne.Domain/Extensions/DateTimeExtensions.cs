using System.Globalization;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for working with dates and times, including formatting, date calculations, and
    /// conversions between different date representations.
    /// </summary>
    /// <remarks>This static class contains utility methods that extend types such as DateTime, string, and
    /// int to simplify common date and time operations. Methods include formatting dates to abbreviated forms,
    /// calculating ages, determining week numbers, generating date ranges, and converting timestamps. These extensions
    /// are intended to streamline date-related logic in applications and promote code reuse.</remarks>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the specified date and time to a string in abbreviated date format, including the day, abbreviated
        /// month, year, and abbreviated day of the week.
        /// </summary>
        /// <param name="dateTime">The date and time value to convert to an abbreviated date string.</param>
        /// <returns>A string representing the date in the format "d MMM yyyy, ddd", where the month and day of the week are
        /// abbreviated.</returns>
        public static string DateToAbreviatedDate(this DateTime dateTime)
        {
            return $"{dateTime.Day} {dateTime.Month.GetAbbreviatedMonth()} {dateTime.Year}, {DateToAbreviatedDay(dateTime.DayOfWeek)}";
        }

        /// <summary>
        /// Returns the three-letter English abbreviation for the specified day of the week.
        /// </summary>
        /// <param name="dateTime">The day of the week to convert to its abbreviated form.</param>
        /// <returns>A string containing the three-letter abbreviation for the specified day of the week (e.g., "Mon" for
        /// Monday).</returns>
        public static string DateToAbreviatedDay(DayOfWeek dateTime)
        {
            switch (dateTime)
            {
                case DayOfWeek.Sunday:
                    return "Sun";
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Tuesday:
                    return "Tue";
                case DayOfWeek.Wednesday:
                    return "Wed";
                case DayOfWeek.Thursday:
                    return "Thu";
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Saturday:
                    return "Sat";
                default: return "Sun";
            }            
        }

        /// <summary>
        /// Returns the abbreviated month name corresponding to the specified full month name, using the current
        /// culture.
        /// </summary>
        /// <remarks>The method uses the current culture to interpret the month name. If the input does
        /// not match a valid month name in the current culture, the method returns null.</remarks>
        /// <param name="monthName">The full name of the month to abbreviate. The value is interpreted using the current culture. Cannot be
        /// null.</param>
        /// <returns>A three-letter abbreviated month name if the input is a valid full month name; otherwise, null.</returns>
        public static string GetAbbreviatedMonth(this string monthName)
        {
            DateTime month;
            return DateTime.TryParseExact(monthName, "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out month) ? month.ToString("MMM") : null;
        }

        /// <summary>
        /// Returns the abbreviated month name corresponding to the specified month number, using the current culture.
        /// </summary>
        /// <remarks>The abbreviated month name is formatted according to the current culture settings. If
        /// the value of monthNr is outside the range 1 through 12, the method returns null.</remarks>
        /// <param name="monthNr">The month number to convert to an abbreviated month name. Must be in the range 1 through 12.</param>
        /// <returns>A three-letter abbreviated month name (such as "Jan" or "Feb") if the month number is valid; otherwise,
        /// null.</returns>
        public static string GetAbbreviatedMonth(this int monthNr)
        {
            DateTime month;
            return DateTime.TryParseExact(monthNr.GetMonthFromMonthNr(), "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out month) ? month.ToString("MMM") : null;
        }

        /// <summary>
        /// Returns the full month name that corresponds to the specified month number, using the current culture's
        /// formatting conventions.
        /// </summary>
        /// <param name="monthNr">The one-based month number (1 for January through 12 for December).</param>
        /// <returns>A string containing the full name of the month corresponding to the specified number. Returns an empty
        /// string if the month number is outside the range 1 through 12.</returns>
        public static string GetMonthFromMonthNr(this int monthNr)
        {
            

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            return mfi.GetMonthName(monthNr);
        }

        /// <summary>
        /// Returns the full month name and year of the specified date as a formatted string.
        /// </summary>
        /// <remarks>The month name is formatted using the current culture's default month names. To
        /// customize the culture, use the appropriate overloads or formatting methods.</remarks>
        /// <param name="date">The date from which to extract the month and year.</param>
        /// <returns>A string containing the full month name followed by the year, such as "January 2023".</returns>
        public static string GetMonthAndYearFromDate(this DateTime date)
        {
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            return $"{mfi.GetMonthName(date.Month)} {date.Year}";
        }

        /// <summary>
        /// Extracts the date of birth from the specified identification number string.
        /// </summary>
        /// <remarks>The method assumes that the identification number encodes the date of birth in the
        /// first six digits as YYMMDD, with the year interpreted as 2000 plus the two-digit year. If the input is not
        /// in the expected format or contains invalid date values, the method returns the current date and
        /// time.</remarks>
        /// <param name="idNumber">The identification number from which to extract the date of birth. Must contain a valid date in the first
        /// six digits in the format YYMMDD.</param>
        /// <returns>A DateTime representing the date of birth extracted from the identification number. If the input is invalid
        /// or does not contain a valid date, returns the current date and time.</returns>
        public static DateTime GetDateOfBirth(this string idNumber)
        {
            try
            {
                idNumber = idNumber.Trim().Replace(" ", "");
                int year = Convert.ToInt32(idNumber.Substring(0, 2))+2000;
                int month = Convert.ToInt32(idNumber.Substring(2, 2));
                int day = Convert.ToInt32(idNumber.Substring(4, 2));

                var date = new DateTime(year, month, day);


                return date;
            }
            catch (Exception ex)
            {

                return DateTime.Now;
            }            
        }

        /// <summary>
        /// Calculates the age in years based on the date of birth encoded in the specified identification number.
        /// </summary>
        /// <remarks>If the identification number is invalid or does not contain a recognizable date of
        /// birth, the method returns 0.</remarks>
        /// <param name="idNumber">The identification number from which to extract the date of birth. Must be in a format recognized by the
        /// GetDateOfBirth extension method.</param>
        /// <returns>The calculated age in years. Returns 0 if the date of birth cannot be determined from the identification
        /// number.</returns>
        public static int GetAge(this string idNumber)
        {
            try
            {
                DateTime dob = idNumber.GetDateOfBirth();
                return DateTime.Now.Year - dob.Year;
            }
            catch (Exception ex)
            {

                return 0;
            }
            
        }

        /// <summary>
        /// Calculates the age in years based on the specified date of birth.
        /// </summary>
        /// <param name="dob">The date of birth to calculate the age from.</param>
        /// <returns>The number of complete years between the specified date of birth and the current date.</returns>
        public static int GetAge(this DateTime dob)
        {
            return DateTime.Now.Year - (dob.Year + 1900);
        }

        /// <summary>
        /// Returns a list of dates representing each day between the specified start and end dates, inclusive.
        /// </summary>
        /// <param name="startDate">The first date in the range. Must be less than or equal to <paramref name="endDate"/>.</param>
        /// <param name="endDate">The last date in the range. Must be greater than or equal to <paramref name="startDate"/>.</param>
        /// <returns>A list of <see cref="DateTime"/> values, each representing a date from <paramref name="startDate"/> to
        /// <paramref name="endDate"/>, inclusive. If <paramref name="startDate"/> is after <paramref name="endDate"/>,
        /// the returned list will be empty.</returns>
        public static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates;

        }

        /// <summary>
        /// Returns a new DateTime representing the start of the week for the specified date, with Monday as the first
        /// day of the week.
        /// </summary>
        /// <remarks>The returned DateTime has the same kind (Local, UTC, or Unspecified) as the input.
        /// This method treats Monday as the first day of the week, regardless of the current culture.</remarks>
        /// <param name="input">The date for which to determine the start of the week.</param>
        /// <returns>A DateTime set to 00:00:00 on the Monday of the week containing the specified date.</returns>
        public static DateTime GetStartOfWeek(this DateTime input)
        {
            // Using +6 here leaves Monday as 0, Tuesday as 1 etc.
            int dayOfWeek = ((int)input.DayOfWeek + 6) % 7;
            return input.Date.AddDays(-dayOfWeek);
        }

        /// <summary>
        /// Calculates the ISO 8601 week number for the specified date, using the invariant culture calendar.
        /// </summary>
        /// <remarks>This method uses the rules of the ISO 8601 standard, where weeks begin on Monday and
        /// the first week of the year is the one that contains at least four days. The calculation is culture-invariant
        /// and does not depend on the current thread's culture.</remarks>
        /// <param name="time">The date for which to determine the week number.</param>
        /// <returns>The ISO 8601 week number of the year that contains the specified date. The first week of the year is the one
        /// with at least four days, and weeks start on Monday.</returns>
        public static int GetWeekNumber(this DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Calculates the number of full weeks between two dates, inclusive of the start and end weeks.
        /// </summary>
        /// <remarks>Both dates are considered as belonging to their respective weeks, regardless of the
        /// specific day. The calculation is based on the start of the week as determined by the GetStartOfWeek
        /// method.</remarks>
        /// <param name="start">The start date of the range. The calculation includes the week containing this date.</param>
        /// <param name="end">The end date of the range. The calculation includes the week containing this date.</param>
        /// <returns>The total number of weeks between the start and end dates, counting both the week of the start date and the
        /// week of the end date.</returns>
        public static int GetWeeks(DateTime start, DateTime end)
        {
            start = start.GetStartOfWeek();
            end = end.GetStartOfWeek();
            int days = (int)(end - start).TotalDays;
            return days / 7 + 1; // Adding 1 to be inclusive
        }

        /// <summary>
        /// Returns a human-readable string that describes the time interval between the specified date and the current
        /// date and time.
        /// </summary>
        /// <remarks>The returned string format varies based on the difference between the specified date
        /// and the current date. For dates within the current day, the result is in minutes or hours. For dates within
        /// the current month, it may return "Yesterday" or the number of days ago. For earlier dates, the full date
        /// string is returned.</remarks>
        /// <param name="date">The date and time to compare to the current date and time.</param>
        /// <returns>A string representing the elapsed time since the specified date. Returns a value such as "X minutes ago", "X
        /// hours ago", "Yesterday", or the full date string, depending on how much time has passed.</returns>
        public static string DisplayDateTimeIntervalString(this DateTime date)
        {
            if (date.Date == DateTime.Now.Date)
            {
                var val = DateTime.Now.TimeOfDay - date.TimeOfDay;

                if (val.Hours <= 1)
                {
                    return val.Minutes + " minutes ago";
                }

                return val.Hours + " hours ago";
            }

            if (date.Month == DateTime.Now.Month)
            {
                if (date.Date.AddDays(1) < DateTime.Now.Date)
                {
                    return "Yesterday";

                }

                return (DateTime.Now.Date - date.Date).TotalDays + " days ago";
            }

            return date.ToLongDateString();
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> that adds the specified number of days, months, and years to the given
        /// date.
        /// </summary>
        /// <remarks>If the resulting date is not valid (for example, adding months to a date with a day
        /// value that does not exist in the resulting month), the day is adjusted to the last valid day of the
        /// resulting month.</remarks>
        /// <param name="dateToProcess">The date to which the days, months, and years will be added.</param>
        /// <param name="days">The number of days to add. Can be negative to subtract days. The default is 0.</param>
        /// <param name="months">The number of months to add. Can be negative to subtract months. The default is 0.</param>
        /// <param name="years">The number of years to add. Can be negative to subtract years. The default is 0.</param>
        /// <returns>A <see cref="DateTime"/> that is the result of adding the specified days, months, and years to <paramref
        /// name="dateToProcess"/>.</returns>
        public static DateTime GetNewDate(this DateTime dateToProcess, int days = 0, int months = 0, int years = 0)
        {
            return dateToProcess.AddDays(days).AddMonths(months).AddYears(years);
        }

        /// <summary>
        /// Rounds the specified <see cref="DateTime"/> value up to the nearest multiple of the given time interval.
        /// </summary>
        /// <remarks>If <paramref name="dt"/> is already an exact multiple of <paramref name="d"/>, the
        /// original value is returned. The <see cref="DateTime.Kind"/> property of the returned value matches that of
        /// <paramref name="dt"/>.</remarks>
        /// <param name="dt">The date and time value to round up.</param>
        /// <param name="d">The time interval to which to round up. Must be a positive <see cref="TimeSpan"/>.</param>
        /// <returns>A <see cref="DateTime"/> that represents the smallest value greater than or equal to <paramref name="dt"/>
        /// that is an exact multiple of <paramref name="d"/>.</returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            var modTicks = dt.Ticks % d.Ticks;
            var delta = modTicks != 0 ? d.Ticks - modTicks : 0;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        /// <summary>
        /// Rounds the specified date and time down to the nearest whole interval of the given time span.
        /// </summary>
        /// <remarks>The returned DateTime preserves the <see cref="DateTime.Kind"/> property of the input
        /// value. If <paramref name="d"/> does not evenly divide one day, the result may not align with calendar
        /// boundaries.</remarks>
        /// <param name="dt">The date and time value to round down.</param>
        /// <param name="d">The time interval to which to round down. Must be a positive time span.</param>
        /// <returns>A new DateTime value representing the largest date and time less than or equal to <paramref name="dt"/> that
        /// is a whole multiple of <paramref name="d"/>.</returns>
        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        /// <summary>
        /// Converts a Unix timestamp, expressed as the number of seconds since January 1, 1970 (UTC), to a local
        /// DateTime value.
        /// </summary>
        /// <remarks>The returned DateTime is adjusted to the local time zone of the current system. If
        /// the value of dt is outside the range supported by DateTime, an exception may be thrown.</remarks>
        /// <param name="dt">The Unix timestamp to convert, representing the number of seconds that have elapsed since January 1, 1970
        /// (UTC).</param>
        /// <returns>A DateTime value representing the local date and time equivalent of the specified Unix timestamp.</returns>
        public static DateTime TimeStampToDateTime(this double dt)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(dt).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Converts a Unix timestamp, expressed as the number of seconds since January 1, 1970 (UTC), to a local
        /// DateTime value.
        /// </summary>
        /// <remarks>The returned DateTime is converted to the local time zone of the current system.
        /// Fractional values in the timestamp represent partial seconds.</remarks>
        /// <param name="dt">The Unix timestamp as a number of seconds since January 1, 1970 (UTC).</param>
        /// <returns>A DateTime value representing the local date and time corresponding to the specified Unix timestamp.</returns>
        public static DateTime TimeStampToDateTime(this float dt)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(dt).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Converts a Unix timestamp, expressed as the number of seconds since January 1, 1970 (UTC), to a local
        /// DateTime value.
        /// </summary>
        /// <remarks>The returned DateTime is converted to the local time zone of the current system. If
        /// the value of dt is outside the range supported by DateTime, an exception may be thrown.</remarks>
        /// <param name="dt">The Unix timestamp to convert, representing the number of seconds elapsed since 00:00:00 UTC on January 1,
        /// 1970.</param>
        /// <returns>A DateTime value representing the local date and time equivalent of the specified Unix timestamp.</returns>
        public static DateTime TimeStampToDateTime(this int dt)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(dt).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Gets the current date and time on the local computer.
        /// </summary>
        /// <remarks>The value returned represents the local time, not Coordinated Universal Time (UTC).
        /// The precision and accuracy of the returned value depend on the system clock.</remarks>
        public static DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the current date with the time component set to 00:00:00.
        /// </summary>
        public static DateTime Today => DateTime.Today;

        /// <summary>
        /// Determines whether the specified date is in the future relative to today.
        /// </summary>
        /// <param name="date">The date to evaluate. The time component is ignored; only the date part is considered.</param>
        /// <returns>true if the specified date is after today; otherwise, false.</returns>
        public static bool IsFuture(DateTime date) => date.Date > Today;

        /// <summary>
        /// Determines whether the specified date is earlier than today.
        /// </summary>
        /// <param name="date">The date to compare to the current date. The time component is ignored.</param>
        /// <returns>true if the specified date is before today; otherwise, false.</returns>
        public static bool IsPast(DateTime date) => date.Date < Today;

        /// <summary>
        /// Determines whether the specified date is today, using the system's current date.
        /// </summary>
        /// <param name="date">The date to compare to the current date. The time component is ignored.</param>
        /// <returns>true if the specified date is the same as today's date; otherwise, false.</returns>
        public static bool IsToday(DateTime date) => date.Date == Today;

        /// <summary>
        /// Determines whether the specified date is today or a future date.
        /// </summary>
        /// <param name="date">The date to compare to the current date. The time component is ignored.</param>
        /// <returns>true if the specified date is today or a future date; otherwise, false.</returns>
        public static bool IsOnOrAfterToday(DateTime date) => date.Date >= Today;
    }
}
