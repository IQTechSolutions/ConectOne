using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for displaying and managing golf courses associated with a vacation.
    /// </summary>
    public partial class VacationGolfCourses
    {
        #region Injections

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve golf course data.
        /// </summary>
        [Inject] public IGolfCoursesService GolfCoursesService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the vacation to which the golf courses belong.
        /// </summary>
        [Parameter, EditorRequired] public string VacationId { get; set; } = null!;

        /// <summary>
        /// The URL of the banner image to display.
        /// </summary>
        [Parameter] public string BannerImageUrl { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// List to hold the golf courses associated with the vacation.
        /// </summary>
        private List<GolfCourseDto> _golfCourses = new();

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component and loads the golf courses associated with the vacation.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the golf courses associated with the vacation
            //var inclusionResult = await GolfCoursesService. Provider.GetAsync<IEnumerable<GolfCourseDto>>($"vacations/vacationgolfcourses/{VacationId}");
            //inclusionResult.ProcessResponseForDisplay(SnackBar, () =>
            //{
            //    // Populate the golf courses list with the fetched data
            //    _golfCourses = inclusionResult.Data.ToList();
            //});

            await base.OnInitializedAsync();
        }

        #endregion
    }
}