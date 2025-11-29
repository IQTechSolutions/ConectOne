namespace ConectOne.Blazor.Components
{
    /// <summary>
    /// Represents a selection item in a table, encapsulating the checked state and the associated row item.
    /// </summary>
    /// <typeparam name="T">The type of the row item associated with the selection.</typeparam>
    public class TableSelectionItem<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableSelectionItem{T}"/> class.
        /// </summary>
        /// <param name="isChecked">Indicates whether the item is checked.</param>
        /// <param name="rowItem">The row item associated with the selection.</param>
        public TableSelectionItem(bool isChecked, T rowItem)
        {
            IsChecked = isChecked;
            RowItem = rowItem;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the row item associated with the selection.
        /// </summary>
        public T RowItem { get; set; }
    }
}