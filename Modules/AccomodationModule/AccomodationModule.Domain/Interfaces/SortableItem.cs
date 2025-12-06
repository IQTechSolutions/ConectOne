namespace NeuralTech.Blazor.Components.SortableItems.Objects
{
    public class SortableItem<T>
    {
        public SortableItem(string selector, List<T> items)
        {
            Selector = selector;
            Items = items;
        }

        public string Selector { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
