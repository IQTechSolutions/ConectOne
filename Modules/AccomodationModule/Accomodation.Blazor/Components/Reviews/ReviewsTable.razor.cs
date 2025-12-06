// ReSharper disable MustUseReturnValue

using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FeedbackModule.Domain.DataTransferObjects;
using FeedbackModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Components.Reviews
{
    /// <summary>
    /// Code-behind for the List Reviews page in the Blazor application.
    /// Handles the display and management of reviews, including pagination, sorting, and deletion.
    /// </summary>
    public partial class ReviewsTable
    {
        #region Injected Services
        
        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        [Inject] public IVacationReviewService VacationReviewService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display transient notification messages to the user.
        /// </summary>
        /// <remarks>Inject this property to provide access to snackbar notifications within the
        /// component. The service allows displaying brief messages, such as status updates or alerts, typically at the
        /// bottom of the UI.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs within the component.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings used for retrieving key-value pairs and other
        /// configuration data.
        /// </summary>
        /// <remarks>The configuration is typically provided by dependency injection and can be used to
        /// access settings from various sources, such as appsettings files, environment variables, or user
        /// secrets.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// The ID of the entity associated with the vacation review.
        /// </summary>
        [Parameter] public string EntityId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// Indicates whether the table should be displayed in a dense layout.
        /// </summary>
        private bool _dense;

        /// <summary>
        /// Indicates whether the table should have striped rows.
        /// </summary>
        private bool _striped = true;

        /// <summary>
        /// Indicates whether the table should have bordered cells.
        /// </summary>
        private bool _bordered;

        /// <summary>
        /// The MudBlazor table component for displaying reviews.
        /// </summary>
        private MudTable<ReviewDto> _table = null!;

        /// <summary>
        /// The request parameters for pagination and sorting.
        /// </summary>
        private ReviewPageParameters _pageParameters = new();

        #endregion

        #region Methods

        /// <summary>
        /// Clears the filters and reloads the table data.
        /// </summary>
        private void ClearFiltersAsync()
        {
            _pageParameters = new ReviewPageParameters();
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
        public async Task<TableData<ReviewDto>> GetVactionExtensionReviewsAsync(TableState state, CancellationToken token)
        {
            try
            {
                _pageParameters.PageNr = state.Page + 1;
                _pageParameters.PageSize = state.PageSize;

                _pageParameters.OrderBy = state.SortDirection switch
                {
                    SortDirection.Ascending => $"{state.SortLabel} desc",
                    SortDirection.Descending => $"{state.SortLabel} asc",
                    _ => null
                };

                var pagingResponse = await VacationReviewService.VacationReviewListAsync(EntityId);

                return new TableData<ReviewDto>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data };
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
                { x => x.ContentText, "Are you sure you want to remove this review from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await VacationReviewService.RemoveVacationReviewAsync(reviewId);
                if (!deletionResult.Succeeded) Snackbar.AddErrors(deletionResult.Messages);

                await _table.ReloadServerData();
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the component by setting up the entity ID and loading initial data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _pageParameters.EntityId = EntityId;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Initializes the component by setting up metadata and breadcrumbs.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                StateHasChanged();
            }
        }

        #endregion
    }
}


