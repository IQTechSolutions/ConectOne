using OneOf;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Provides options for configuring the appearance and behavior of a marker displayed on a Google Map.
/// </summary>
/// <remarks>Use this class to specify marker properties such as position, icon, label, animation, and interaction
/// settings when creating or updating a marker on a map. All properties are optional unless otherwise noted; required
/// properties must be set for the marker to function correctly. Some options may only be supported in specific versions
/// of the Google Maps API or may affect marker rendering and interactivity. For advanced scenarios, such as custom
/// icons or optimized rendering, refer to the documentation for each property.</remarks>
public class MarkerOptions : ListableEntityOptionsBase
{
    #region Constructors

    public MarkerOptions() { }
    public MarkerOptions(LatLngLiteral position, GoogleMap map, string? name)
    {
        Position = position;
        Map = map.InteropObject;
        Title = name;
    }

    #endregion


    /// <summary>
    /// The offset from the marker's position to the tip of an InfoWindow that has been opened with the marker as anchor.
    /// </summary>
    public Point AnchorPoint { get; set; }

    /// <summary>
    /// Which animation to play when marker is added to a map.
    /// </summary>
    public Animation? Animation { get; set; }

    /// <summary>
    /// If false, disables cross that appears beneath the marker when dragging. 
    /// This option is true by default.
    /// </summary>
    public bool? CrossOnDrag { get; set; }

    /// <summary>
    /// Mouse cursor to show on hover
    /// </summary>
    public string Cursor { get; set; }

    /// <summary>
    /// Icon for the foreground. 
    /// If a string is provided, it is treated as though it were an Icon with the string as url.
    /// </summary>
    //[JsonConverter(typeof(OneOfConverter))]
    public OneOf<string, Icon, Symbol>? Icon { get; set; }

    /// <summary>
    /// Adds a label to the marker. The label can either be a string, or a MarkerLabel object.
    /// </summary>
    //[JsonConverter(typeof(OneOfConverter))]
    public OneOf<string, MarkerLabel>? Label { get; set; }

    /// <summary>
    /// The marker's opacity between 0.0 and 1.0.
    /// </summary>
    public float? Opacity { get; set; }

    /// <summary>
    /// Optimization renders many markers as a single static element. 
    /// Optimized rendering is enabled by default. 
    /// Disable optimized rendering for animated GIFs or PNGs, or when each marker must be rendered as a separate DOM element (advanced usage only)
    /// </summary>
    public bool? Optimized { get; set; }

    /// <summary>
    /// Marker position. Required.
    /// </summary>
    public LatLngLiteral Position { get; set; }

    /// <summary>
    /// 2021-07 supported only in beta google maps version
    /// </summary>
    [JsonConverter(typeof(GoogleMapsComponents.Serialization.JsonStringEnumConverterEx<CollisionBehavior>))]
    public CollisionBehavior? CollisionBehavior { get; set; }

    /// <summary>
    /// Image map region definition used for drag/click.
    /// </summary>
    public MarkerShape Shape { get; set; }

    /// <summary>
    /// Rollover text
    /// </summary>
    public string? Title { get; set; }
}