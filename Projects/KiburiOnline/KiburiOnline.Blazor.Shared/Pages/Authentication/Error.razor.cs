using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents an error encountered during the application's execution.
    /// </summary>
    /// <remarks>This class provides information about the current request context, including a unique request
    /// identifier that can be used for tracing and debugging purposes.</remarks>
    public partial class Error
    {
        /// <summary>
        /// Gets or sets the current <see cref="HttpContext"/> for the component.
        /// </summary>
        [CascadingParameter] private HttpContext? HttpContext { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the current request.
        /// </summary>
        private string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be displayed.
        /// </summary>
        private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Initializes the component and sets the <see cref="RequestId"/> property.
        /// </summary>
        /// <remarks>This method assigns a unique identifier to the <see cref="RequestId"/> property, 
        /// using the current activity's ID if available, or the HTTP context's trace identifier as a
        /// fallback.</remarks>
        protected override void OnInitialized() => RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}
