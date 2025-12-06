using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a custom variable tag with a name, description, and value,  along with a placeholder format for use
    /// in templated content.
    /// </summary>
    /// <remarks>The <see cref="CustomVariableTag"/> class is designed to store metadata for a custom
    /// variable,  including its name, description, and value. The <see cref="VaiablePlaceHolder"/> property provides  a
    /// formatted placeholder string that can be used in templated content to reference the variable.</remarks>
    public class CustomVariableTag : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the value represented by this instance.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets a placeholder string formatted with the current value of the <see cref="Name"/> property.
        /// </summary>
        public string VaiablePlaceHolder => $"<---{Name}--->";
    }
}
