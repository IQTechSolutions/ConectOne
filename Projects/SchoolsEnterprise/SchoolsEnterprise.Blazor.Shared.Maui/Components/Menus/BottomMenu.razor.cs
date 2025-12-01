using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Menus
{
    /// <summary>
    /// The BottomMenu component displays a bottom navigation menu for a Blazor application,
    /// including configurable button labels fetched from the application’s configuration.
    /// </summary>
    public partial class BottomMenu
    {
        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Reads a button text from the application configuration 
        /// using the "ApplicationConfiguration:EversButtonText" key.
        /// </summary>
        private string EversButtonText => Configuration["ApplicationConfiguration:EversButtonText"];

        /// <summary>
        /// Provides localized strings for the bottom navigation menu.
        /// </summary>
        [Inject] public IStringLocalizer<BottomMenu> Localizer { get; set; } = null!;
    }
}