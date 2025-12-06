using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.ContactUsViews
{
    public partial class CustomerQueryList
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> contains information about the user's identity
        /// and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of the <see cref="ISnackbar"/> service.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The configuration settings can be used to retrieve application-specific values, such
        /// as  connection strings, application settings, or environment variables. Ensure that the  configuration is
        /// properly initialized before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling vacation-related contact information.
        /// </summary>
        [Inject] public IVacationContactUsInfoService VacationContactUsInfoService { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// The MudBlazor table component for displaying reviews.
        /// </summary>
        private MudTable<VacationContactUsInfoDto> _table = null!;

        /// <summary>
        /// The request parameters for pagination and sorting.
        /// </summary>
        private RequestParameters _pageParameters = new();

        #endregion

        #region Methods

        /// <summary>
        /// Clears the filters and reloads the table data.
        /// </summary>
        private void ClearFiltersAsync()
        {
            _pageParameters = new RequestParameters();
            _table.ReloadServerData();
        }

        /// <summary>
        /// Searches and reloads the table data.
        /// </summary>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Retrieves the reviews data for the table with pagination and sorting.
        /// </summary>
        /// <param name="state">The state of the table, including pagination and sorting information.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The table data containing the reviews.</returns>
        public async Task<TableData<VacationContactUsInfoDto>> GetVacationContactUsInfosAsync(TableState state, CancellationToken token)
        {
            try
            {
                _pageParameters.PageNr = state.Page + 1;
                _pageParameters.PageSize = state.PageSize;

                //_pageParameters.OrderBy = state.SortDirection switch
                //{
                //    SortDirection.Ascending => $"{state.SortLabel} desc",
                //    SortDirection.Descending => $"{state.SortLabel} asc",
                //    _ => null
                //};

                var pagingResponse = await VacationContactUsInfoService.PagedVacationContactUsInfoListAsync(_pageParameters, token);

                return new TableData<VacationContactUsInfoDto>() { TotalItems = pagingResponse.TotalCount, Items = pagingResponse.Data };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Deletes a review by its ID after user confirmation.
        /// </summary>
        /// <param name="reviewId">The ID of the review to delete.</param>
        private async Task Delete(string reviewId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this query from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await VacationContactUsInfoService.RemoveVacationContactUsInfoAsync(reviewId);
                if (!deletionResult.Succeeded) SnackBar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the component by setting up metadata and breadcrumbs.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var currentUser = authState.User;
            await base.OnInitializedAsync();
        }
        
        #endregion
    }
}
