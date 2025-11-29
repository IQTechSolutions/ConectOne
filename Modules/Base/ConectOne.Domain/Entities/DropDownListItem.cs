namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents an item in a drop-down list, containing an identifier and a display value.
    /// </summary>
    public class DropDownListItem
    {
        /// <summary>
        /// Initializes a new instance of the DropDownListItem class with the specified identifier and display value.
        /// </summary>
        /// <param name="id">The unique identifier for the drop-down list item. Cannot be null.</param>
        /// <param name="value">The display value associated with the drop-down list item. Cannot be null.</param>
        public DropDownListItem(string id, string value)
        {
            Id = id;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the string value associated with this instance.
        /// </summary>
        public string Value { get; set; }
    }
}
