using ConectOne.Domain.Extensions;
using Microsoft.AspNetCore.Components;

namespace ConectOne.Blazor.Components
{
    /// <summary>
    /// Represents a customizable tile button component with support for images, titles, subtitles, and click events.
    /// </summary>
    /// <remarks>The <see cref="TileButton"/> component is designed for use in user interfaces where a
    /// visually distinct, interactive tile is needed. It supports various customization options, including setting a
    /// title, subtitle, image, and dimensions. The component also allows for handling click events and displaying a
    /// badge count.</remarks>
    public partial class TileButton
    {
        /// <summary>
        /// Gets or sets the callback to be invoked when a tile is clicked.
        /// </summary>
        /// <remarks>Use this property to specify the action to perform when a tile is clicked.  The
        /// callback can be used to handle user interactions, such as navigating to a new page  or updating the
        /// application state.</remarks>
        [Parameter] public EventCallback TileClicked { get; set; }

        /// <summary>
        /// Gets or sets the title to be displayed. This value is required.
        /// </summary>
        [Parameter, EditorRequired] public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the height of the component.
        /// </summary>
        [Parameter] public string Height { get; set; } = "130px";

        /// <summary>
        /// Gets or sets the URL of the image to be displayed.
        /// </summary>
        [Parameter] public string Image { get; set; } = "/images/static/icons2/communication.svg";

        /// <summary>
        /// Gets or sets a value indicating whether an image should be displayed.
        /// </summary>
        [Parameter] public bool DisplayImage { get; set; } = true;

        /// <summary>
        /// Gets or sets the subtitle text to be displayed.
        /// </summary>
        [Parameter] public string? SubTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image should be displayed in its full size.
        /// </summary>
        [Parameter] public bool FullImage { get; set; } = false;

        /// <summary>
        /// Gets or sets the width of the image, specified as a CSS length value.
        /// </summary>
        [Parameter] public string? ImageWidth { get; set; } = "50px";

        /// <summary>
        /// Gets or sets the height of the image, specified as a CSS length value.
        /// </summary>
        [Parameter] public string? ImageHeight { get; set; } = "50px";

        /// <summary>
        /// Gets or sets the elevation level of the component.
        /// </summary>
        /// <remarks>Elevation typically affects the visual appearance of the component, such as its
        /// shadow or depth. Higher values may indicate greater prominence or layering.</remarks>
        [Parameter] public int Elevation { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number displayed on the badge.
        /// </summary>
        /// <remarks>This property is typically used to indicate a quantity or count, such as
        /// notifications or items. Setting this property to a negative value may result in undefined
        /// behavior.</remarks>
        [Parameter] public int BadgeCount { get; set; }

        /// <summary>
        /// Gets the inline CSS style string representing the height and width of an image.
        /// </summary>
        private string GetImageStyle => $"height: {NormalizeLength(ImageHeight)}; width: {NormalizeLength(ImageWidth)}";

        /// <summary>
        /// Gets the CSS class for the card container based on the presence of a subtitle.
        /// </summary>
        private string CardContainerClass => string.IsNullOrEmpty(SubTitle) ? "rounded-15 second-card2" : "rounded-lg second-card2";

        /// <summary>
        /// Gets the CSS class string for the card content based on the presence of a subtitle.
        /// </summary>
        private string CardContentClass => string.IsNullOrEmpty(SubTitle) ? "d-flex flex-column mb-1 mt-1 px-1 py-5" : "d-flex flex-column px-3 py-2";

        /// <summary>
        /// Gets a value indicating whether the current image is represented as a font icon.
        /// </summary>
        private bool HasFontIcon => IconValueHelper.IsFontIcon(Image);

        /// <summary>
        /// Gets the CSS class name for the font icon associated with the current image.
        /// </summary>
        private string FontIconClass => IconValueHelper.GetFontIconClass(Image);

        /// <summary>
        /// Gets the CSS style string used to display the icon preview with the current image dimensions and font size.
        /// </summary>
        private string IconPreviewStyle => $"width: {NormalizeLength(ImageWidth)}; height: {NormalizeLength(ImageHeight)}; font-size: calc({NormalizeLength(ImageHeight)} * 0.7);";

        /// <summary>
        /// Gets the inline CSS style string that specifies the width of the badge container based on the image width.
        /// </summary>
        private string BadgeContainerStyle => $"width: {NormalizeLength(ImageWidth)};";

        /// <summary>
        /// Gets the CSS style string used to display a full-size image icon with centered content.
        /// </summary>
        private string FullImageIconStyle => "height: 100%; width: 100%; display:flex; align-items:center; justify-content:center; font-size: clamp(2.5rem, 6vw, 4.5rem);";

        /// <summary>
        /// Normalizes a CSS length value, returning a default if the input is null or whitespace.
        /// </summary>
        /// <param name="value">The CSS length value to normalize. Can be null or contain leading/trailing whitespace or a trailing
        /// semicolon.</param>
        /// <returns>A trimmed CSS length string without a trailing semicolon. Returns "50px" if <paramref name="value"/> is null
        /// or consists only of whitespace.</returns>
        private static string NormalizeLength(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "50px";
            }

            return value.Trim().TrimEnd(';');
        }
    }
}
