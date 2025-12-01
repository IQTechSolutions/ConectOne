using ConectOne.Blazor.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor.Interfaces;
using MudBlazor.Utilities;
using Color = MudBlazor.Color;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Menus
{
    /// <summary>
    /// Represents a selectable item within a bottom navigation menu, supporting icons, navigation links, and
    /// customizable appearance.
    /// </summary>
    /// <remarks>Use this class to define individual items in a bottom navigation menu. Each item can display
    /// an icon, respond to navigation events, and be styled according to the application's theme. The item supports
    /// enabling or disabling navigation, specifying a target URL, and customizing the icon's color. This class is
    /// typically used as a child component within a bottom navigation menu container.</remarks>
    public partial class BottomNavMenuItem : MudBaseSelectItem
    {
        /// <summary>
        /// Gets the CSS class string for the navigation item, including any additional classes specified by the
        /// component.
        /// </summary>
        protected string Classname => new CssBuilder("mud-nav-item").AddClass(Class).Build();

        /// <summary>
        /// Gets the CSS class string for the navigation link, including any state-dependent classes.
        /// </summary>
        /// <remarks>The returned class string includes the base class for navigation links and appends a
        /// disabled class if the link is disabled. This value is typically used to style the link element appropriately
        /// based on its state.</remarks>
        protected string LinkClassname => new CssBuilder("mud-nav-link").AddClass($"mud-nav-link-disabled", Disabled).Build();

        /// <summary>
        /// Gets the CSS class name used for the navigation link icon, based on the current icon color.
        /// </summary>
        protected string IconClassname => new CssBuilder("mud-nav-link-icon").AddClass($"mud-nav-link-icon-default", IconColor == Color.Default).Build();

        /// <summary>
        /// Gets a collection of attribute key-value pairs for the current instance.
        /// </summary>
        /// <remarks>Returns null if the instance is disabled. Otherwise, the collection contains an entry
        /// for the "href" attribute with its associated value.</remarks>
        private Dictionary<string, object> Attributes
        {
            get => Disabled ? null : new Dictionary<string, object>()
            {
                { "href", Href }
            };
        }

        /// <summary>
        /// Icon to use if set.
        /// </summary>
        [Parameter] public string Icon { get; set; }

        /// <summary>
        /// The color of the icon. It supports the theme colors, default value uses the themes drawer icon color.
        /// </summary>
        [Parameter] public Color IconColor { get; set; } = Color.Error;

        /// <summary>
        /// Gets or sets a value that determines how the current URL is compared to the link's href to determine if the
        /// link is active.
        /// </summary>
        /// <remarks>Use this property to specify whether the link should be considered active when the
        /// current URL matches the href exactly or when it matches the prefix. The default value is <see
        /// cref="NavLinkMatch.Prefix"/>.</remarks>
        [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

        /// <summary>
        /// Gets or sets the name of the browsing context in which to open the linked document.
        /// </summary>
        /// <remarks>This property corresponds to the HTML 'target' attribute. Common values include
        /// "_blank" to open the link in a new tab or window, "_self" to open in the same frame, and custom frame names.
        /// If not set, the default browser behavior is used.</remarks>
        [Parameter] public string Target { get; set; }

        /// <summary>
        /// Gets or sets the title to display for the component.
        /// </summary>
        [Parameter] public string Title { get; set; }

        /// <summary>
        /// Gets or sets the receiver for navigation events cascaded to this component.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework to provide the component with
        /// notifications about navigation events, such as location changes. It is intended for use in components that
        /// need to respond to navigation actions within the application.</remarks>
        [CascadingParameter] INavigationEventReceiver NavigationEventReceiver { get; set; }

        /// <summary>
        /// Gets or sets the parent <see cref="BottomNavMenu"/> component
        /// in the cascading parameter hierarchy.
        /// </summary>
        /// <remarks>This property is typically set automatically by the Blazor framework when the
        /// component is used within a <see cref="BottomNavMenu"/>. It
        /// enables child components to access shared state or functionality provided by the parent menu.</remarks>
        [CascadingParameter] BottomNavMenu Parent { get; set; }

        /// <summary>
        /// Handles a navigation event by invoking the associated navigation event receiver if navigation is enabled.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task completes when the navigation event has been
        /// handled, or immediately if navigation is disabled or no event receiver is set.</returns>
        private Task HandleNavigation()
        {
            if (!Disabled && NavigationEventReceiver != null)
            {
                return NavigationEventReceiver.OnNavigation();
            }
            return Task.CompletedTask;
        }
    }
}
