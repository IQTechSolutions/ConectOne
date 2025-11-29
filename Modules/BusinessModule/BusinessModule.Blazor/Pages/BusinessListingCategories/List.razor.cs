using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace BusinessModule.Blazor.Pages.BusinessListingCategories;

/// <summary>
/// Represents a partial class that provides functionality for managing a list.
/// </summary>
/// <remarks>This class is designed to be extended with additional functionality in other parts of the
/// application. The <see cref="Configuration"/> property is injected and can be used to access application
/// configuration settings.</remarks>
public partial class List
{
    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;
}
