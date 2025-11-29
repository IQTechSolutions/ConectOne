namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for object instances to facilitate type conversions and other common operations.
    /// </summary>
    /// <remarks>The methods in this class are intended to simplify working with objects by adding utility
    /// functions as extension methods. These methods can be called directly on any object instance. All methods are
    /// static and thread-safe.</remarks>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts the specified value to the enumeration type T.
        /// </summary>
        /// <remarks>The conversion is case-sensitive and requires that the value corresponds to a valid
        /// name or underlying value of the enumeration type. If the value does not match a valid enumeration member, an
        /// exception is thrown.</remarks>
        /// <typeparam name="T">The enumeration type to convert the value to. Must be an enum.</typeparam>
        /// <param name="value">The value to convert to the enumeration type. The value is converted to a string and parsed as the name or
        /// numeric value of an enumeration member.</param>
        /// <returns>An instance of type T that represents the enumeration value parsed from the specified value.</returns>
        public static T ToEnum<T>(this object value)
        {
            T enumVal = (T)Enum.Parse(typeof(T), value.ToString());
            return enumVal;
        }
    }
}
