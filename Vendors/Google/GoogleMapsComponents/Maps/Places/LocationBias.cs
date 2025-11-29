using OneOf;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// A LocationBias represents a soft boundary or hint to use when searching for Places.
/// Results may come from outside the specified area.
/// To use the current user's IP address as a bias, the string "IP_BIAS" can be specified.
/// Note: if using a Circle the center and radius must be defined.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class LocationBias : OneOfBase<LatLngLiteral, LatLngBounds, LatLngBoundsLiteral, Circle, string>
{
    /// <summary>
    /// Initializes a new instance of the LocationBias class using the specified bias value.
    /// </summary>
    /// <remarks>Use this constructor to specify a location bias when searching or filtering results. The bias
    /// influences the prioritization of results but does not restrict them strictly to the specified area.</remarks>
    /// <param name="value">The location bias to apply. Can be a latitude/longitude literal, a bounding box, a circle, or a string
    /// representing a place or area. The type determines how the bias is interpreted.</param>
    public LocationBias(OneOf<LatLngLiteral, LatLngBounds, LatLngBoundsLiteral, Circle, string> value) : base(value) { }
}