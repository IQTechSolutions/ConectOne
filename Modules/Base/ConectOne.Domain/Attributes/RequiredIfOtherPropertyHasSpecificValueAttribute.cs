using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ConectOne.Domain.Attributes
{
    /// <summary>
    /// Specifies that a property is required if another property on the same object has a specific value.
    /// </summary>
    /// <remarks>This attribute is used to enforce conditional validation rules where the requirement of a
    /// property depends on the value of another property. For example, you can use this attribute to ensure that a
    /// field is provided only when a related field has a specific value.</remarks>
    /// <param name="dependentPropertyName">The name of the property whose value determines whether the decorated property is required.</param>
    /// <param name="dependentPropertyValue">The specific value of the dependent property that triggers the requirement for the decorated property.</param>
    public class RequiredIfOtherPropertyHasSpecificValueAttribute(string dependentPropertyName, object dependentPropertyValue) : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified value based on the condition of a dependent property.
        /// </summary>
        /// <remarks>This method checks whether the value of the property being validated is required
        /// based on the value of a dependent property. If the dependent property's value matches the specified
        /// condition, the validated property must not be null or empty.</remarks>
        /// <param name="value">The value of the property being validated.</param>
        /// <param name="validationContext">The context in which the validation is performed, including information about the object being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful.  Returns <see
        /// cref="ValidationResult.Success"/> if the validation passes; otherwise, a <see cref="ValidationResult"/> with
        /// an error message.</returns>
        /// <exception cref="ArgumentException">Thrown if the dependent property specified by the validation attribute cannot be found on the object being
        /// validated.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo dependentProperty = validationContext.ObjectType.GetProperty(dependentPropertyName);

            if (dependentProperty == null)
            {
                throw new ArgumentException($"Unknown property: {dependentPropertyName}");
            }

            object dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance);

            if (dependentValue != null && dependentValue.Equals(dependentPropertyValue))
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {dependentPropertyName} is {dependentPropertyValue}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
