using Microsoft.AspNetCore.Components;

namespace KiburiOnline.Blazor.Shared.Pages.Destinations
{
    /// <summary>
    /// Represents a Blazor component that manages page initialization, metadata, and navigation for a list view.
    /// </summary>
    /// <remarks>This component is intended to be used as a base for list pages in Blazor applications. It
    /// configures page metadata and breadcrumb navigation using the provided services. The component relies on
    /// dependency injection for navigation management and is designed to be extended or used as part of a larger
    /// application structure.</remarks>
	public partial class List
    {
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and sets up page metadata and breadcrumbs.
        /// </summary>
        /// <remarks>This method configures the <see cref="MetaDataTransferService.PageDetails"/> property
        /// with details  about the current page, including its title, URI, and breadcrumb navigation links. It then
        /// calls  the base implementation of <see cref="OnInitializedAsync"/> to complete the initialization
        /// process.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
		{
            await base.OnInitializedAsync();
        }
	}
}
