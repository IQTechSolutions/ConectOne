using Microsoft.JSInterop;
using OneOf;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Represents a marker on a Google Map, allowing customization of its appearance, position, and interactivity.
/// </summary>
/// <remarks>Use the Marker class to add, configure, and control markers displayed on a Google Map instance.
/// Markers can display custom icons, labels, and respond to user interactions such as clicks or drags. Most properties
/// and methods correspond to the Google Maps JavaScript API Marker object. Methods are asynchronous and interact with
/// the underlying JavaScript map instance; ensure the associated map and JavaScript runtime are available before using
/// Marker methods.</remarks>
public class Marker : ListableEntityBase<MarkerOptions>
{
    /// <summary>
    /// Asynchronously creates a new Google Maps marker instance using the specified JavaScript runtime and options.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime to use for creating the marker. Cannot be null.</param>
    /// <param name="opts">The options to configure the marker, or null to use default settings.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created marker instance.</returns>
    public static async Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Marker", opts);
        var obj = new Marker(jsObjectRef);
        return obj;
    }

    /// <summary>
    /// Initializes a new instance of the Marker class with the specified JavaScript object reference.
    /// </summary>
    /// <param name="jsObjectRef">A reference to the underlying JavaScript object that represents the marker. Cannot be null.</param>
    internal Marker(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Asynchronously retrieves the current animation state, if available.
    /// </summary>
    /// <returns>A nullable <see cref="Animation"/> value representing the current animation state, or <see langword="null"/> if
    /// no animation is set or the value cannot be determined.</returns>
    public async Task<Animation?> GetAnimation()
    {
        var animation = await _jsObjectRef.InvokeAsync<object?>("getAnimation");

        return Helper.ToNullableEnum<Animation>(animation?.ToString());
    }

    /// <summary>
    /// Asynchronously determines whether the associated element is currently clickable.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the element is
    /// clickable; otherwise, <see langword="false"/>.</returns>
    public Task<bool> GetClickable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getClickable");
    }

    /// <summary>
    /// Retrieves the current cursor value as a string from the underlying JavaScript object asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current cursor value as a
    /// string.</returns>
    public Task<string> GetCursor()
    {
        return _jsObjectRef.InvokeAsync<string>(
            "getCursor");
    }

    /// <summary>
    /// Asynchronously determines whether the associated element is currently draggable.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
    /// element is draggable; otherwise, <see langword="false"/>.</returns>
    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getDraggable");
    }

    /// <summary>
    /// Asynchronously retrieves the icon associated with the current object.
    /// </summary>
    /// <remarks>The returned value type depends on the icon's representation in the underlying system.
    /// Callers should handle all possible result types when processing the returned value.</remarks>
    /// <returns>A <see cref="OneOf{T0, T1, T2}"/> containing either a string representing the icon's name, an <see cref="Icon"/>
    /// object, or a <see cref="Symbol"/> object, depending on the icon's format.</returns>
    public async Task<OneOf<string, Icon, Symbol>> GetIcon()
    {
        var result = await _jsObjectRef.InvokeAsync<string, Icon, Symbol>(
            "getIcon");

        return result;
    }

    /// <summary>
    /// Asynchronously retrieves the label associated with the marker.
    /// </summary>
    /// <remarks>The returned value may be a simple string or a more complex <see cref="MarkerLabel"/> object,
    /// depending on how the label was set. Await the returned task to obtain the label value.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains either a string representing the
    /// label text or a <see cref="MarkerLabel"/> object with detailed label information.</returns>
    public Task<OneOf<string, MarkerLabel>> GetLabel()
    {
        return _jsObjectRef.InvokeAsync<string, MarkerLabel>("getLabel");
    }

    /// <summary>
    /// Asynchronously retrieves the label text associated with the current marker.
    /// </summary>
    /// <returns>A string containing the label text. The returned value is never null.</returns>
    public async Task<string> GetLabelText()
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        return markerLabel.IsT0 ? markerLabel.AsT0 : markerLabel.AsT1.Text;
    }

    /// <summary>
    /// Asynchronously retrieves the marker label as a MarkerLabel instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a MarkerLabel representing the label
    /// for the marker. If the label is not already a MarkerLabel, a new MarkerLabel is created with the label text.</returns>
    public async Task<MarkerLabel> GetLabelMarkerLabel()
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        return markerLabel.IsT1 ?
            markerLabel.AsT1 :
            new MarkerLabel { Text = markerLabel.AsT0 };
    }

    /// <summary>
    /// Asynchronously retrieves the current geographic position represented by latitude and longitude coordinates.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="LatLngLiteral"/> object
    /// with the current latitude and longitude values.</returns>
    public Task<LatLngLiteral> GetPosition()
    {
        return _jsObjectRef.InvokeAsync<LatLngLiteral>(
            "getPosition");
    }

    /// <summary>
    /// Asynchronously retrieves the shape definition for the marker.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="MarkerShape"/> object
    /// describing the marker's clickable region.</returns>
    public Task<MarkerShape> GetShape()
    {
        return _jsObjectRef.InvokeAsync<MarkerShape>(
            "getShape");
    }

    /// <summary>
    /// Asynchronously retrieves the title of the associated JavaScript object.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the title as a string, or an empty
    /// string if the object has no title.</returns>
    public Task<string> GetTitle()
    {
        return _jsObjectRef.InvokeAsync<string>(
            "getTitle");
    }

    /// <summary>
    /// Asynchronously gets a value indicating whether the associated element is currently visible.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
    /// element is visible; otherwise, <see langword="false"/>.</returns>
    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getVisible");
    }

    /// <summary>
    /// Asynchronously retrieves the z-index value of the associated JavaScript object.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the z-index value as an integer.</returns>
    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>(
            "getZIndex");
    }

    /// <summary>
    /// Start an animation. 
    /// Any ongoing animation will be cancelled. 
    /// Currently supported animations are: BOUNCE, DROP. 
    /// Passing in null will cause any animation to stop.
    /// </summary>
    /// <param name="animation"></param>
    public Task SetAnimation(Animation? animation)
    {
        int? animationCode;

        switch (animation)
        {
            case null: animationCode = null; break;
            case Animation.Bounce: animationCode = 1; break;
            case Animation.Drop: animationCode = 2; break;
            default: animationCode = 0; break;
        }

        return _jsObjectRef.InvokeAsync(
            "setAnimation",
            animationCode);
    }

    /// <summary>
    /// Sets whether the associated element is clickable.
    /// </summary>
    /// <param name="flag">A value indicating whether the element should be clickable. Set to <see langword="true"/> to make the element
    /// clickable; otherwise, <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetClickable(bool flag)
    {
        return _jsObjectRef.InvokeAsync(
            "setClickable",
            flag);
    }

    /// <summary>
    /// Sets the cursor style for the associated JavaScript object.
    /// </summary>
    /// <param name="cursor">The CSS cursor value to apply. This can be any valid CSS cursor string, such as "pointer", "default", or a
    /// custom URL. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetCursor(string cursor)
    {
        return _jsObjectRef.InvokeAsync(
            "setCursor",
            cursor);
    }

    /// <summary>
    /// Sets whether the associated element is draggable by the user.
    /// </summary>
    /// <param name="flag">A value indicating whether the element should be draggable. Set to <see langword="true"/> to enable dragging;
    /// otherwise, <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetDraggable(bool flag)
    {
        return _jsObjectRef.InvokeAsync(
            "setDraggable",
            flag);
    }

    /// <summary>
    /// Sets the icon for the associated object asynchronously.
    /// </summary>
    /// <param name="icon">The URL or identifier of the icon to set. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetIcon(string icon)
    {
        return _jsObjectRef.InvokeAsync(
            "setIcon",
            icon);
    }

    /// <summary>
    /// Sets the icon for the associated object asynchronously.
    /// </summary>
    /// <param name="icon">The icon to set. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetIcon(Icon icon)
    {
        return _jsObjectRef.InvokeAsync(
            "setIcon",
            icon);
    }

    /// <summary>
    /// Sets the label for the marker using the specified value.
    /// </summary>
    /// <param name="label">The label to assign to the marker. Can be either a string or a MarkerLabel object. If a string is provided, it
    /// will be used as the label text; if a MarkerLabel is provided, its properties will define the label's appearance
    /// and content.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetLabel(OneOf<string, MarkerLabel> label)
    {
        return _jsObjectRef.InvokeAsync(
            "setLabel",
            label);
    }

    /// <summary>
    /// Sets the text of the label associated with the marker asynchronously.
    /// </summary>
    /// <param name="labelText">The new text to assign to the marker's label. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetLabelText(string labelText)
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        if (markerLabel.IsT1)
        {
            MarkerLabel label = markerLabel.AsT1;
            label.Text = labelText;
            await SetLabel(label);
        }
        else
            await SetLabel(labelText);
    }

    /// <summary>
    /// Sets the opacity level of the associated element asynchronously.
    /// </summary>
    /// <param name="opacity">The opacity value to set. Must be between 0.0 (fully transparent) and 1.0 (fully opaque).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetOpacity(float opacity)
    {
        return _jsObjectRef.InvokeAsync(
            "setOpacity",
            opacity);
    }

    /// <summary>
    /// Sets the marker's configuration options asynchronously.
    /// </summary>
    /// <param name="options">The options to apply to the marker. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetOptions(MarkerOptions options)
    {
        return _jsObjectRef.InvokeAsync(
            "setOptions",
            options);
    }

    /// <summary>
    /// Asynchronously sets the position of the object on the map to the specified latitude and longitude.
    /// </summary>
    /// <param name="latLng">The geographic coordinates to set as the new position. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetPosition(LatLngLiteral latLng)
    {
        return _jsObjectRef.InvokeAsync(
            "setPosition",
            latLng);
    }

    public Task SetShape(MarkerShape shape)
    {
        return _jsObjectRef.InvokeAsync(
            "setShape",
            shape);
    }

    /// <summary>
    /// Asynchronously sets the title of the associated JavaScript object.
    /// </summary>
    /// <param name="title">The new title to set. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetTitle(string title)
    {
        return _jsObjectRef.InvokeAsync(
            "setTitle",
            title);
    }

    /// <summary>
    /// Sets the visibility state of the associated UI element asynchronously.
    /// </summary>
    /// <param name="visible">A value indicating whether the element should be visible. Set to <see langword="true"/> to make the element
    /// visible; otherwise, <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }

    /// <summary>
    /// Sets the z-index value for the associated JavaScript object asynchronously.
    /// </summary>
    /// <param name="zIndex">The z-index to assign. Higher values bring the object to the front. Typically, this should be a non-negative
    /// integer.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetZIndex(int zIndex)
    {
        return _jsObjectRef.InvokeAsync(
            "setZIndex",
            zIndex);
    }
}