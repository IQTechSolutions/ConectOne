using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Menus
{
    /// <summary>
    /// Represents a bottom navigation menu component that displays navigation items and optional menu title content
    /// within a MudBlazor application.
    /// </summary>
    /// <remarks>Use this component to create a bottom navigation bar with customizable content. The menu can
    /// optionally display a title, depending on the value of the ShowMenuTitle property. Child content typically
    /// consists of navigation items or other interactive elements.</remarks>
    public partial class BottomNavMenu : MudComponentBase
    {
        /// <summary>
        /// Gets or sets the content to be rendered inside the component.
        /// </summary>
        /// <remarks>Use this parameter to specify child elements or markup that should appear within the
        /// component's output. Typically set implicitly by placing content between the component's opening and closing
        /// tags in Razor markup.</remarks>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the menu title is displayed.
        /// </summary>
        [Parameter] public bool ShowMenuTitle { get; set; } = true;
    }
}
