using GroupingModule.Application.ViewModels;

namespace SchoolsModule.Blazor.Components.ActivityCategories
{
	/// <summary>
	/// Provides data for the event that occurs when a category selection changes.
	/// </summary>
	/// <param name="category">The category associated with the selection change.</param>
	/// <param name="action">The action that describes how the selection changed.</param>
	public class CategorySelectionChangedEventArgs(CategoryViewModel category, CategorySelectionAction action)
	{
		/// <summary>
		/// Gets or sets the category associated with the current view model.
		/// </summary>
		public CategoryViewModel Category { get; set; } = category;

		/// <summary>
		/// Gets or sets the action to perform when a category is selected.
		/// </summary>
		public CategorySelectionAction Action { get; set; } = action;
	}

	/// <summary>
	/// Specifies the action to perform when modifying a category selection.
	/// </summary>
	public enum CategorySelectionAction
	{
		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		Add,

		/// <summary>
		/// Removes the specified element from the collection.
		/// </summary>
		Remove
	}
}
