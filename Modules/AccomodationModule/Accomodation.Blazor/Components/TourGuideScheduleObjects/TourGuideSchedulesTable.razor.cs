using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace Accomodation.Blazor.Components.TourGuideScheduleObjects
{
    /// <summary>
    /// Component for displaying and managing vacation packages in a table format.
    /// </summary>
    public partial class TourGuideSchedulesTable
    {
        #region Parameters

        /// <summary>
        /// The ID of the vacation host associated with the vacation extensions.
        /// </summary>
        [Parameter] public string VacationHostId { get; set; } = null!;

        #endregion

        #region Private Fields

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private MudTable<VacationViewModel> _table = null!;
        private VacationPageParameters _pageParameters = new();

        #endregion

        #region Injections

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure
        /// that a valid implementation  of <see cref="IVacationService"/> is provided before using this
        /// property.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// control application behavior. This property is typically populated by dependency injection.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Clears all filters and reloads the table data.
        /// </summary>
        private void ClearFiltersAsync()
        {
            _pageParameters = new VacationPageParameters();
            _table.ReloadServerData();
        }

        /// <summary>
        /// Triggers a search and reloads the table data.
        /// </summary>
        private async Task Search()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Retrieves a paginated list of vacations based on the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        public async Task<TableData<VacationViewModel>> GetVacationsAsync(TableState state, CancellationToken token)
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

                var pagingResponse = await VacationService.PagedAsync(_pageParameters, token);

                return new TableData<VacationViewModel>()
                {
                    TotalItems = pagingResponse.Data.Count(),
                    Items = pagingResponse.Data.Select(c => new VacationViewModel(c))
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the component by setting up the vacation host ID and loading initial data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _pageParameters.VacationHostId = VacationHostId;
            await base.OnInitializedAsync();
        }
    }
}
