using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile.RegistrationUpgrades
{
    /// <summary>
    /// The Categories component displays a loading state for a short period,
    /// then shows the category listings (or other content).
    /// It also allows navigation back to the commerce index or to a wallet page.
    /// </summary>
    public partial class RoleRegistrations
    {
        public bool _loaded;
        private IEnumerable<ServiceTierDto> _tiers;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The HTTP provider is typically injected and used to manage HTTP communication within
        /// the application. Ensure that a valid implementation of <see cref="IBaseHttpProvider"/> is provided before
        /// using this property.</remarks>
        [Inject] public IServiceTierService ServiceTierService { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides methods for showing
        /// snack bar notifications, such as alerts or status messages, in the user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the type of advertisement to be displayed.
        /// </summary>
        [Parameter] public string RoleId { get; set; }

        /// <summary>
        /// Called by the Blazor runtime when the component initializes.
        /// Simulates a loading/data retrieval process via Task.Delay.
        /// </summary>
        protected override async Task OnInitializedAsync()
        { 
            var pagingResponse = await ServiceTierService.AllEntityServiceTiersAsync(RoleId); 
            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }
            else
            {
                _tiers = pagingResponse.Data;
            }
            _loaded = true;
        }

        /// <summary>
        /// Navigates the user back to the commerce index page.
        /// </summary>
        public void NavigateToCategoryPage(string categoryId)
        {
            NavigationManager.NavigateTo($"/Accounts/Registrations/{RoleId}");
        }

    }
}
