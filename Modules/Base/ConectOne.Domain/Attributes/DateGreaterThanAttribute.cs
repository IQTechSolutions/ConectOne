using System.ComponentModel.DataAnnotations;

namespace ConectOne.Domain.Attributes
{
    /// <summary>
    /// Specifies that a date value must be greater than the value of another date property.
    /// </summary>
    /// <remarks>This attribute is used to validate that the value of the decorated property is later than the
    /// value of the specified comparison property. The comparison property must exist on the same object and must be of
    /// type <see cref="DateTime"/>. If the comparison property is not found or is not a <see cref="DateTime"/>, an
    /// exception will be thrown during validation.</remarks>
    /// <param name="comparisonProperty">The name of the property to compare against. This property must be of type <see cref="DateTime"/>.</param>
    public class DateGreaterThanAttribute(string comparisonProperty) : ValidationAttribute
    {
        /// <summary>
        /// Validates that the specified value is a <see cref="DateTime"/> and occurs after the value of a specified
        /// comparison property.
        /// </summary>
        /// <remarks>This method compares the value of the current property to the value of another
        /// property specified by the <c>comparisonProperty</c> field. The validation succeeds if the current property's
        /// value is a <see cref="DateTime"/> that occurs after the comparison property's value.</remarks>
        /// <param name="value">The value to validate. Must be a <see cref="DateTime"/>.</param>
        /// <param name="validationContext">The context in which the validation is performed, including information about the object being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation succeeded.  Returns <see
        /// cref="ValidationResult.Success"/> if the value is valid; otherwise, a <see cref="ValidationResult"/> with an
        /// error message.</returns>
        /// <exception cref="ArgumentException">Thrown if the comparison property specified in the validation context does not exist.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime currentDateTime)
            {
                var property = validationContext.ObjectType.GetProperty(comparisonProperty);

                if (property == null)
                {
                    throw new ArgumentException($"Property with name {comparisonProperty} not found.");
                }

                if (property.GetValue(validationContext.ObjectInstance) is DateTime comparisonDateTime)
                {
                    if (currentDateTime <= comparisonDateTime)
                    {
                        return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be after {property.Name}.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
