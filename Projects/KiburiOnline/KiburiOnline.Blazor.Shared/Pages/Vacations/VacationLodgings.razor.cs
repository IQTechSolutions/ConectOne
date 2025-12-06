using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying and managing lodgings associated with a vacation.
    /// </summary>
    public partial class VacationLodgings
    {
        #region Injections

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the lodgings belong.
        /// </summary>
        [Parameter] public string? VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation extension.
        /// </summary>
        [Parameter] public string? VacationExtensionId { get; set; } = null!;

        /// <summary>
        /// The URL of the banner image to display.
        /// </summary>
        [Parameter] public string BannerImageUrl { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the lodgings associated with the vacation.
        /// </summary>
        private List<LodgingDto> _lodgings = new();

        #endregion

        #region Overrides

        /// <summary>
        /// Executes logic after the component has rendered, with additional behavior for the first render.
        /// </summary>
        /// <remarks>This method overrides <see cref="ComponentBase.OnAfterRenderAsync(bool)"/> to perform
        /// custom initialization when the component is rendered for the first time. If <paramref name="firstRender"/>
        /// is <see langword="true"/>, it fetches lodging data based on the provided <c>VacationId</c> or
        /// <c>VacationExtensionId</c> and updates the component's state accordingly.  Any exceptions encountered during
        /// the initialization process are logged and rethrown.</remarks>
        /// <param name="firstRender">A value indicating whether this is the first time the component has rendered. If <see langword="true"/>,
        /// additional initialization logic is performed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);
                //if (firstRender)
                //{
                //    if (!string.IsNullOrEmpty(VacationId))
                //    {
                //        var inclusionResult = await Provider.GetAsync<IEnumerable<LodgingDto>>($"vacations/vacationlodgings/{VacationId}");
                //        inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
                //        {
                //            // Populate the lodgings list with the fetched data
                //            _lodgings = inclusionResult.Data.ToList();
                //        });
                //    }

                //    if (!string.IsNullOrEmpty(VacationExtensionId))
                //    {
                //        var inclusionResult = await Provider.GetAsync<IEnumerable<LodgingDto>>($"vacations/extensions/vacationlodgings/{VacationExtensionId}");
                //        inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
                //        {
                //            // Populate the lodgings list with the fetched data
                //            _lodgings = inclusionResult.Data.ToList();
                //        });
                //    }

                //    StateHasChanged();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        #endregion
    }
}