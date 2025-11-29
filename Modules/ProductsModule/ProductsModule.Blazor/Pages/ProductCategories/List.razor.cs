using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace ProductsModule.Blazor.Pages.ProductCategories;

/// <summary>
/// Represents a partial class that provides functionality for managing a list.
/// </summary>
/// <remarks>This class is marked as partial, allowing its functionality to be extended across multiple files. The
/// <see cref="Configuration"/> property is injected and can be used to access application configuration
/// settings.</remarks>
public partial class List
{
    /// <summary>
    /// Gets or sets the application configuration settings.
    /// </summary>
    [Inject] public IConfiguration Configuration { get; set; } = null!;
}
