namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for formatting <see cref="DateTime"/> and nullable <see cref="DateTime"/> values in
    /// the PayFast date format (yyyy-MM-dd).
    /// </summary>
    /// <remarks>These methods are intended to simplify the process of converting date values to the string
    /// format required by the PayFast payment gateway API. The class is static and cannot be instantiated.</remarks>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="DateTime"/> value to a string formatted as a PayFast-compatible date
        /// ("yyyy-MM-dd").
        /// </summary>
        /// <remarks>This method is intended for formatting dates according to the requirements of the
        /// PayFast payment gateway, which expects dates in the "yyyy-MM-dd" format. The time component of the <paramref
        /// name="dateTime"/> value is not included in the output.</remarks>
        /// <param name="dateTime">The date and time value to convert to PayFast date format.</param>
        /// <returns>A string representation of the date in "yyyy-MM-dd" format, suitable for use with PayFast APIs.</returns>
        public static string ToPayFastDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Converts the specified nullable <see cref="DateTime"/> value to a string in the format required by PayFast
        /// ("yyyy-MM-dd").
        /// </summary>
        /// <param name="dateTime">The nullable date and time value to convert. If <see langword="null"/>, an empty string is returned.</param>
        /// <returns>A string representation of the date in "yyyy-MM-dd" format if <paramref name="dateTime"/> has a value;
        /// otherwise, an empty string.</returns>
        public static string ToPayFastDate(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("yyyy-MM-dd");
            }

            return string.Empty;
        }
    }
}
