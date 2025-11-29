using SchoolsModule.Application.ViewModels;

namespace SchoolsModule.Blazor.Components.Parents
{
    /// <summary>
    /// Provides data for the event that occurs when a parent selection changes in a view model.
    /// </summary>
    /// <remarks>Use this class to access information about the parent involved in the selection change and
    /// the type of selection action performed. This event argument is typically used in event handlers responding to
    /// selection changes in hierarchical or parent-child data structures.</remarks>
    public class ParentSelectionChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ParentSelectionChangedEventArgs class with the specified parent and
        /// selection action.
        /// </summary>
        /// <param name="parent">The parent view model associated with the selection change. Cannot be null.</param>
        /// <param name="action">The action that describes how the parent selection was changed.</param>
        public ParentSelectionChangedEventArgs(ParentViewModel parent, ParentSelectionAction action)
        {
            Parent = parent;
            Action = action;
        }

        /// <summary>
        /// Gets or sets the parent view model associated with this instance.
        /// </summary>
        public ParentViewModel Parent { get; set; }

        /// <summary>
        /// Gets or sets the action to perform when selecting a parent element.
        /// </summary>
        public ParentSelectionAction Action { get; set; }
    }

    /// <summary>
    /// Specifies the action to perform when modifying a parent-child relationship, such as adding or removing a parent
    /// from a collection.
    /// </summary>
    /// <remarks>Use this enumeration to indicate whether a parent should be added to or removed from a
    /// relationship. The values correspond to the supported operations for managing parent associations.</remarks>
    public enum ParentSelectionAction
    {
        Add,
        Remove
    }
}
