using System.ComponentModel.DataAnnotations;

namespace ConectOne.Domain.Attributes
{
    /// <summary>
    /// Specifies that a property value must be greater than the value of another property.
    /// </summary>
    /// <remarks>This attribute is typically used to enforce validation rules where the value of one property
    /// must exceed the value of another property within the same object. The comparison is performed using integer
    /// values, and both properties must be convertible to integers.</remarks>
    /// <param name="comparerPropertyName">The name of the property to compare against. This property must exist on the same object as the property being
    /// validated.</param>
    public sealed class GreaterThanAttribute(string comparerPropertyName) : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified value against a comparison property to ensure it meets the defined condition.
        /// </summary>
        /// <remarks>This method compares the value of the property being validated with the value of
        /// another property, specified by the comparer property name. The validation fails if the value of the property
        /// being validated is not greater than the value of the comparison property.</remarks>
        /// <param name="value">The value to validate. This is typically the value of the property being validated.</param>
        /// <param name="validationContext">The context in which the validation is performed, including information about the object being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful.  Returns <see
        /// langword="null"/> if the value is valid; otherwise, a <see cref="ValidationResult"/> containing an error
        /// message.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
	        try
	        {
		        var propertyTestedInfo = validationContext.ObjectType.GetProperty(comparerPropertyName);
		        if (propertyTestedInfo == null)
		        {
			        return new ValidationResult($"Unknown Property '{comparerPropertyName}'");
		        }
		        var propertyTestedValue = Convert.ToInt32(propertyTestedInfo.GetValue(validationContext.ObjectInstance, null));
		        var thisValue = Convert.ToInt32(value);

		        if (propertyTestedValue > thisValue)
		        {
			        return new ValidationResult($"Value '{thisValue}' must be greater than '{comparerPropertyName} : {propertyTestedValue}'");
		        }

		        return base.IsValid(value, validationContext);
			}
	        catch (Exception e)
	        {
		        Console.WriteLine(e);
		        throw;
	        }
            
        }
    }

    /// <summary>
    /// Specifies that the value of the decorated property must be smaller than the value of another property within the
    /// same object.
    /// </summary>
    /// <remarks>This attribute is used to enforce a validation rule where the value of the decorated property
    /// must be less than the value of a specified property in the same object. The comparison is performed using
    /// integer values.</remarks>
    /// <param name="comparerPropertyName">The name of the property to compare against. This property must exist in the same object as the decorated
    /// property.</param>
    public sealed class SmallerThanAttribute(string comparerPropertyName) : ValidationAttribute
    {
        /// <summary>
        /// Validates that the value of the current property is less than or equal to the value of a specified
        /// comparison property.
        /// </summary>
        /// <remarks>This method compares the value of the current property to the value of another
        /// property specified by the validation attribute. The comparison property must exist on the object being
        /// validated and must be convertible to an integer. If the comparison property value is less than the current
        /// property value, the validation fails.</remarks>
        /// <param name="value">The value of the property being validated. This can be <see langword="null"/>.</param>
        /// <param name="validationContext">The context in which the validation is performed, including information about the object being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation succeeded.  Returns <see
        /// langword="null"/> or <see cref="ValidationResult.Success"/> if the validation passes; otherwise, a <see
        /// cref="ValidationResult"/> with an error message.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(comparerPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult($"Unknown Property '{comparerPropertyName}'");
            }
            var propertyTestedValue = Convert.ToInt32(propertyTestedInfo.GetValue(validationContext.ObjectInstance, null));
            var thisValue = Convert.ToInt32(value);

            if (propertyTestedValue < thisValue)
            {
                return new ValidationResult(string.Format($"Value '{0}' must be smaller than '{comparerPropertyName} :  {propertyTestedValue}'", thisValue, propertyTestedValue));
            }

            return base.IsValid(value, validationContext);
        }
    }
}
