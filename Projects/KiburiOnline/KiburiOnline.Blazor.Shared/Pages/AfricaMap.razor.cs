using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KiburiOnline.Blazor.Shared.Pages
{
    /// <summary>
    /// Represents a component that displays an interactive map of Africa, allowing selection and navigation based on
    /// country.
    /// </summary>
    /// <remarks>This component is intended for use within Blazor applications. It integrates with JavaScript
    /// to initialize map functionality and uses navigation services to respond to user interactions. The selected
    /// country can be specified via the <see cref="Country"/> parameter.</remarks>
    public partial class AfricaMap
    {
        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>Use this property to perform JavaScript interop operations within Blazor components.
        /// The injected instance enables calling JavaScript methods and handling results asynchronously.</remarks>
        [Inject] public IJSRuntime JS { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> used to manage and perform navigation operations within the
        /// application.
        /// </summary>
        /// <remarks>Use this property to programmatically navigate to different URIs or to access
        /// information about the current navigation state. This property is typically injected by the framework in
        /// Blazor components.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the component.
        /// </summary>
        [Parameter] public string? Country { get; set; } = null;

        /// <summary>
        /// Handles logic to be executed after the component has rendered. Invokes initialization routines when the
        /// component is rendered for the first time.
        /// </summary>
        /// <remarks>Override this method to perform post-rendering initialization, such as interacting
        /// with JavaScript or updating the UI. Initialization code within this method will only run when <paramref
        /// name="firstRender"/> is <see langword="true"/>.</remarks>
        /// <param name="firstRender">Indicates whether this is the first time the component has been rendered. <see langword="true"/> if this is
        /// the initial render; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var dd = NavigationManager.Uri;

                await JS.InvokeVoidAsync("initAfricaMap");
            }
        }
    }
}
