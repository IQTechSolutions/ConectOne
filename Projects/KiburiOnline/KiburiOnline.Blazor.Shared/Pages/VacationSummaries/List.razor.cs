using Microsoft.AspNetCore.Components;

namespace KiburiOnline.Blazor.Shared.Pages.VacationSummaries
{
	/// <summary>
	/// Represents a component that initializes and manages metadata for a page, including breadcrumbs, URL, and other page
	/// details.
	/// </summary>
	/// <remarks>This component is designed to set up page metadata, such as breadcrumbs, URL, and SEO-related
	/// properties,  using the <see cref="MetaDataTransferService"/>. It is typically used in Blazor applications to manage
	/// navigation and metadata for pages.</remarks>
	public partial class List
	{
		/// <summary>
		/// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in
		/// Blazor applications.
		/// </summary>
		[Inject] public NavigationManager NavigationManager { get; set; } = null!;

		/// <summary>
		/// Asynchronously initializes the component and sets up page metadata.
		/// </summary>
		/// <remarks>This method configures the page details, including breadcrumbs, URL, and metadata settings such
		/// as  whether robots are allowed to index the page. It also invokes the base implementation to ensure  proper
		/// initialization of the component.</remarks>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}
    }
}
