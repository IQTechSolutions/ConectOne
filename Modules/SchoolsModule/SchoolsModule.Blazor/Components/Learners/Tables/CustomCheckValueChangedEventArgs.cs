namespace SchoolsModule.Blazor.Components.Learners.Tables
{
    /// <summary>
    /// Represents the event arguments for a custom checkbox value change event.
    /// </summary>
    /// <typeparam name="T">The type of the extra parameter associated with the event.</typeparam>
    public class CustomCheckValueChangedEventArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCheckValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="isChecked">Indicates whether the checkbox is checked.</param>
        /// <param name="learner">The extra parameter associated with the event.</param>
        public CustomCheckValueChangedEventArgs(bool isChecked, T? learner)
        {
            IsChecked = isChecked;
            Item = learner;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the extra parameter associated with the event.
        /// </summary>
        public T? Item { get; set; }
    }
}
