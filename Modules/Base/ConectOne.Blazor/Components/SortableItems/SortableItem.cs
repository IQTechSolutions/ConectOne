namespace ConectOne.Blazor.Components.SortableItems
{
    /// <summary>
    /// Represents a collection of items grouped by a selector value, enabling sorting or categorization based on the
    /// selector.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the collection.</typeparam>
    public class SortableItem<T>
    {
        /// <summary>
        /// Initializes a new instance of the SortableItem class with the specified selector and items.
        /// </summary>
        /// <param name="selector">The selector string used to identify or filter the items for sorting. Cannot be null.</param>
        /// <param name="items">The list of items to be associated with this SortableItem. Cannot be null.</param>
        public SortableItem(string selector, List<T> items)
        {
            Selector = selector;
            Items = items;
        }

        /// <summary>
        /// Gets or sets the selector string used to identify elements or data within the context.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the collection of items contained in the list.
        /// </summary>
        public List<T> Items { get; set; } = new();
    }
}
