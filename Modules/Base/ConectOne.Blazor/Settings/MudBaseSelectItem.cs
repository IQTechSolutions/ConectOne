using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace ConectOne.Blazor.Settings
{
    public abstract class MudBaseSelectItem : MudComponentBase
    {
        /// <summary>
        /// If true, the input element will be disabled.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>
        /// If true, disables ripple effect.
        /// </summary>
        [Parameter] public bool DisableRipple { get; set; }

        /// <summary>
        /// Link to a URL when clicked.
        /// </summary>
        [Parameter] public string Href { get; set; }

        /// <summary>
        /// Child content of component.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Command parameter.
        /// </summary>
        [Parameter] public object CommandParameter { get; set; }

        /// <summary>
        /// Command executed when the user clicks on an element.
        /// </summary>
        [Parameter] public ICommand Command { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager used for URI navigation and manipulation within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public Microsoft.AspNetCore.Components.NavigationManager UriHelper { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the component is clicked.
        /// </summary>
        /// <remarks>Use this property to specify a handler for click events on the component. The
        /// callback receives the mouse event data as a parameter. If not set, clicking the component has no
        /// effect.</remarks>
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Handles a mouse click event by navigating to the specified URL or invoking the associated click event and
        /// command.
        /// </summary>
        /// <remarks>If a navigation URL is specified, the method initiates navigation to that URL.
        /// Otherwise, it invokes the click event callback and executes the associated command if it can be
        /// executed.</remarks>
        /// <param name="ev">The mouse event data associated with the click action.</param>
        protected void OnClickHandler(MouseEventArgs ev)
        {
            if (Href != null)
            {
                UriHelper.NavigateTo(Href);
            }
            else
            {
                OnClick.InvokeAsync(ev);
                if (Command?.CanExecute(CommandParameter) ?? false)
                {
                    Command.Execute(CommandParameter);
                }
            }
        }
    }
}
