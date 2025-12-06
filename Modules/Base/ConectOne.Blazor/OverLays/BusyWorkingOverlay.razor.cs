using Microsoft.AspNetCore.Components;

namespace ConectOne.Blazor.OverLays
{
    /// <summary>
    /// Represents an overlay component that displays a busy or working indicator, typically used to block user
    /// interaction while a background operation is in progress.
    /// </summary>
    /// <remarks>Use this component to inform users that a process is ongoing and to prevent further
    /// interaction until the operation completes. The overlay can display custom text and captions, and supports both
    /// light and dark backgrounds for visual integration with different UI themes.</remarks>
    public partial class BusyWorkingOverlay
    {
        /// <summary>
        /// Gets or sets a value indicating whether the component is visible.
        /// </summary>
        [Parameter, EditorRequired] public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the text displayed to indicate that the component is busy.
        /// </summary>
        [Parameter] public string? BusyText { get; set; }

        /// <summary>
        /// Gets or sets the caption text displayed by the component.
        /// </summary>
        [Parameter] public string? CaptionText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component should use a dark background theme.
        /// </summary>
        [Parameter] public bool DarkBackground { get; set; }
    }
}
