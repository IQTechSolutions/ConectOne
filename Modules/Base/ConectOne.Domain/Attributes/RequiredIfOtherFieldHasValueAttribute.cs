using System.ComponentModel.DataAnnotations;

namespace ConectOne.Domain.Attributes
{
    /// <summary>
    /// Specifies that a property is required if another property on the same object has a non-null or non-empty value.
    /// </summary>
    /// <remarks>This attribute is used to enforce conditional validation rules where the presence of a value
    /// in one property requires another property to also have a value. For example, if a "Country" property is set, a
    /// "State" property might be required.</remarks>
    /// <param name="otherFieldName">The name of the other property whose value determines whether the current property is required.</param>
    public class RequiredIfOtherPropertyHasValueAttribute(string otherFieldName) : ValidationAttribute
    {
        /// <summary>
        /// Validates the current field based on the value of another field in the same object.
        /// </summary>
        /// <remarks>This method ensures that the current field is required only when the specified
        /// dependent field has a value.  If the dependent field is null or empty, the current field is not
        /// validated.</remarks>
        /// <param name="value">The value of the field being validated.</param>
        /// <param name="validationContext">The context in which the validation is performed, including information about the object being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful.  Returns <see
        /// cref="ValidationResult.Success"/> if the validation passes; otherwise, a <see cref="ValidationResult"/> with
        /// an error message.</returns>
        /// <exception cref="ArgumentException">Thrown if the property specified by the dependent field name does not exist on the object being validated.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherFieldProperty = validationContext.ObjectType.GetProperty(otherFieldName);

            if (otherFieldProperty == null)
            {
                throw new ArgumentException($"Property '{otherFieldName}' not found on type '{validationContext.ObjectType.Name}'.");
            }

            var otherFieldValue = otherFieldProperty.GetValue(validationContext.ObjectInstance);

            // If the other field has a value (is not null for nullable types/references)
            if (otherFieldValue == null || string.IsNullOrWhiteSpace(otherFieldValue.ToString()))
                return ValidationResult.Success;
            // And the current field is null or empty, then it's invalid
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {otherFieldName} has a value.");
            }

            return ValidationResult.Success;
        }
    }
}
