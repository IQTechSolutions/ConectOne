using AccomodationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Pages.Destinations.Partials
{
    /// <summary>
    /// Represents a control for displaying vacation destinations within a specified country.
    /// </summary>
    /// <remarks>This control is designed to display a collection of vacation destinations, provided as a list
    /// of <see cref="VacationDto"/> objects. The control also requires the name of the country to which the vacations
    /// belong.</remarks>
    public partial class DestinationVacationsControl
    {
        /// <summary>
        /// Gets or sets the collection of vacations.
        /// </summary>
        /// <remarks>This property is required and must be initialized with a valid collection. An empty
        /// collection can be used if no vacations are available.</remarks>
        [Parameter, EditorRequired] public ICollection<VacationDto> Vacations { get; set; } = [];

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated via dependency injection and provides access to
        /// application settings, such as connection strings, app-specific options, and other configuration
        /// values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        [Parameter, EditorRequired] public string CountryName { get; set; }
    }
}
