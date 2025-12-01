using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile
{
    /// <summary>
    /// Represents the options and parameters required to upgrade an account's service tier.
    /// </summary>
    /// <remarks>This class is used to manage the upgrade process for a user's account tier, including the 
    /// necessary role and user identifiers. It also interacts with a service provider to retrieve  available service
    /// tiers.</remarks>
    public partial class UpgradeAccountTierOptions
    {
        private IEnumerable<ServiceTierDto> _serviceTiers = [];

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IServiceTierService ServiceTierService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the identifier for the role.
        /// </summary>
        [Parameter] public string? RoleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        /// <summary>
        /// Upgrades the current user or entity to the next available tier.
        /// </summary>
        /// <remarks>This method transitions the user or entity to a higher tier, if applicable.  Ensure
        /// that the current tier allows for an upgrade before calling this method.</remarks>
        private void UpgradeToTier()
        {

        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves the service tiers associated with the specified role.
        /// </summary>
        /// <remarks>This method fetches a list of service tiers for the role identified by <see
        /// cref="RoleId"/>  and assigns the result to the local field. If the retrieval operation fails, the service
        /// tiers  list remains unmodified. This method also invokes the base implementation of <see
        /// cref="OnInitializedAsync"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var serviceTierResult = await ServiceTierService.AllEntityServiceTiersAsync(RoleId);
            if (serviceTierResult.Succeeded)
                _serviceTiers = serviceTierResult.Data;

            await base.OnInitializedAsync();
        }
    }
}
