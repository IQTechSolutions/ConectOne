using Microsoft.AspNetCore.Components;

namespace KiburiOnline.Blazor.Shared.Pages.VacationHosts
{
    /// <summary>
    /// Represents a page for listing vacation hosts.
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Initializes the component and sets up the page metadata.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}