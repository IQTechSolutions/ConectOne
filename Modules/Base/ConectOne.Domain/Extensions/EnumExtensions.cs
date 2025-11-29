using System.ComponentModel;
using System.Reflection;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for working with enum types, including parsing string values 
    /// into enums and retrieving their human-friendly description attributes.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Parses the specified string value to the enum of type <typeparamref name="T"/>.
        /// The comparison is case-insensitive.
        /// </summary>
        /// <typeparam name="T">
        /// The target enum type to parse.
        /// </typeparam>
        /// <param name="value">
        /// The string value representing the enum name or numeric value.
        /// </param>
        /// <returns>
        /// The parsed enum of type <typeparamref name="T"/>.
        /// </returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Retrieves the description text from a <see cref="DescriptionAttribute"/> assigned 
        /// to an enum field. If no attribute is found, returns null.
        /// </summary>
        /// <param name="value">The enum value whose description is being retrieved.</param>
        /// <returns>
        /// The description string from the <see cref="DescriptionAttribute"/>, 
        /// or null if not present.
        /// </returns>
        public static string GetDescription(this Enum value)
        {
            // Determine the type of the enum and fetch its name
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                // Retrieve the field corresponding to the enum name
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    // Get the DescriptionAttribute, if any
                    DescriptionAttribute attr =
                        Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                            as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            // Return null if no description attribute is found
            return null;
        }
    }
}
