namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Represents the gender of an individual. 'All' is included as a special option 
    /// to indicate filtering or queries that should include both Male and Female.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Gender is not specified or cannot be determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Male gender.
        /// </summary>
        Male = 1,

        /// <summary>
        /// Female gender.
        /// </summary>
        Female = 2,

        /// <summary>
        /// Represents a choice to include all genders (primarily used for filtering scenarios).
        /// Not used for assigning to a single individual, but rather for queries or filtering.
        /// </summary>
        All = 3
    }

    /// <summary>
    /// Provides extension methods for the Gender enum, enabling conversions from 
    /// various string inputs to the Gender enum values.
    /// </summary>
    public static class GenderExtensions
    {
        /// <summary>
        /// Attempts to convert a given string representation of gender into a Gender enum value.
        /// Recognizes multiple cultural or language variants for "Male" and "Female".
        /// </summary>
        /// <param name="gender">A string representing the gender, e.g. "M", "F", "SEUN", "DOGTER", "BOY", or "GIRL".</param>
        /// <returns>
        /// Returns Gender.Male if the input matches any known male keyword,
        /// Gender.Female if it matches any known female keyword,
        /// and Gender.Unknown otherwise.
        /// </returns>
        public static Gender ToGender(string gender)
        {
            if (gender.ToUpper() == "M" || gender.ToUpper() == "SEUN/BOY" || gender.ToUpper() == "BOY" || gender.ToUpper() == "SEUN")
                return Gender.Male;
            else if (gender.ToUpper() == "F" || gender.ToUpper() == "DOGTER/GIRL" || gender.ToUpper() == "GIRL" || gender.ToUpper() == "DOGTER")
                return Gender.Female;

            // If the string does not match any known pattern, return Unknown.
            return Gender.Unknown;
        }
    }
}
