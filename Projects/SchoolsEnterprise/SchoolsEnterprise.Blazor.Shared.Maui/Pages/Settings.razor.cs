using Microsoft.AspNetCore.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages
{
    public partial class Settings
    {
        private bool _loaded;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs, retrieving the current URI, and handling navigation events. This property is typically
        /// injected by the Blazor framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Asynchronously performs component initialization logic when the component is first rendered.
        /// </summary>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1500);
            _loaded = true;
        }

        /// <summary>
        /// Navigates the application to the commerce dashboard page.
        /// </summary>
        /// <remarks>This method changes the current route to the commerce dashboard. It can be used to
        /// programmatically redirect users to the commerce section of the application.</remarks>
        public void NavigateToCommercePage()
        {
            NavigationManager.NavigateTo("/dashboard-commerce");
        }

        /// <summary>
        /// Navigates the application to the wallet dashboard page.
        /// </summary>
        public void NavigateToWalletDashboard()
        {
            NavigationManager.NavigateTo("/dashboard-wallet");
        }
    }
}
