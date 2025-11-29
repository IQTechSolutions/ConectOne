using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using BusinessModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using IdentityModule.Domain.Constants;

namespace BusinessModule.Blazor.Components.BusinessListings
{
    /// <summary>
    /// Represents a table component for reviewing business listings, allowing users to view, approve, or reject
    /// advertisements.
    /// </summary>
    /// <remarks>This component is designed to display a paginated list of business listings based on the
    /// specified <see cref="ReviewStatus"/> and <see cref="UserId"/>. It supports server-side data loading and provides
    /// actions for approving or rejecting advertisements.</remarks>
    public partial class BusinessListingsReviewTable
    {
        private IEnumerable<BusinessListingDto> _listings = new List<BusinessListingDto>();
        private MudTable<BusinessListingDto> _table = null!;
        private int _totalItems;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private ClaimsPrincipal _user;

        /// <summary>
        /// Provides the current authentication state of the user. Injected as a cascading parameter.
        /// Used to determine the user's roles and ID for filtering data.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query the business directory.
        /// </summary>
        /// <remarks>This property is automatically injected and must be set before using any
        /// functionality that depends on querying the business directory.</remarks>
        [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to the business directory.
        /// </summary>
        [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service must be injected and is typically used for
        /// displaying brief,  non-intrusive notifications in the user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the review status of the current item.
        /// </summary>
        [Parameter] public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// Retrieves and reloads a paginated list of business listings based on the specified table state and
        /// user-defined parameters.
        /// </summary>
        /// <remarks>This method queries the business directory service for listings that match the
        /// specified parameters, updates the internal state of the table,  and returns the results. If the operation
        /// fails, error messages are added to the snack bar for user feedback.</remarks>
        /// <param name="state">The current state of the table, including sorting and pagination information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of items and the current page of business listings.</returns>
        private async Task<TableData<BusinessListingDto>> ServerReload(TableState state, CancellationToken token)
        {
            var args = new BusinessListingPageParameters() { Status = ReviewStatus };

            if (!_user.IsInRole(RoleConstants.SuperUser) && !_user.IsInRole(RoleConstants.Administrator))
            {
                args.UserId = UserId;
            }

            var result = await BusinessDirectoryQueryService.PagedListingsAsync(args);
            if (result.Succeeded)
            {
                _totalItems = result.Data.Count();
                _listings = result.Data.ToList(); 
            }
            SnackBar.AddErrors(result.Messages);
            return new TableData<BusinessListingDto> { TotalItems = _totalItems, Items = _listings };
        }

        /// <summary>
        /// Reloads the server data for the table asynchronously.
        /// </summary>
        /// <remarks>This method invokes the server to refresh the data associated with the table.  It is
        /// an asynchronous operation and should be awaited to ensure the data is fully reloaded before
        /// proceeding.</remarks>
        private async Task Reload()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Approves the advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to approve the advertisement and processes the response. 
        /// If the operation is successful, the advertisement is reloaded, and a success message is displayed.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to approve. Cannot be <see langword="null"/> or empty.</param>
        private async Task ApproveAdvertisement(string advertisementId)
        {
            var request = await BusinessDirectoryCommandService.ApproveAsync(advertisementId);
            request.ProcessResponseForDisplay(SnackBar, async () =>
            {
                await Reload();
                SnackBar.AddSuccess("Advertisement Approved");
            });
        }

        /// <summary>
        /// Rejects an advertisement with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a request to reject the advertisement and processes the response. 
        /// If the operation is successful, the advertisement list is reloaded, and a success message is
        /// displayed.</remarks>
        /// <param name="advertisementId">The unique identifier of the advertisement to reject. Cannot be <see langword="null"/> or empty.</param>
        private async Task RejectAdvertisement(string advertisementId)
        {
            var request = await BusinessDirectoryCommandService.RejectAsync(advertisementId);
            request.ProcessResponseForDisplay(SnackBar, async () =>
            {
                await Reload();
                SnackBar.AddSuccess("Advertisement Rejected");
            });
        }

        #region LifeCycle Methods

        /// <summary>
        /// Invoked when the component is initialized. Sets up any required state or data for the component.
        /// </summary>
        /// <remarks>This method is called once during the component's lifecycle, before rendering. It
        /// sets the initial state of the component and then calls the base implementation.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Domain.Constants.Permissions.BusinessReviewPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Domain.Constants.Permissions.BusinessReviewPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Domain.Constants.Permissions.BusinessReviewPermissions.Delete)).Succeeded;

            _loaded = true;
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
