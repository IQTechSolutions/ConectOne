using OneOf;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// Represents configuration options for creating or describing a feature, including its geometry and unique identifier.
/// </summary>
public class FeatureOptions
{
    /// <summary>
    /// The feature geometry. 
    /// If none is specified when a feature is constructed, the feature's geometry will be null. 
    /// If a LatLng object or LatLngLiteral is given, this will be converted to a Data.Point geometry.
    /// </summary>
    public OneOf<Geometry, LatLngLiteral>? Geometry { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier, which can be either an integer or a string value.
    /// </summary>
    /// <remarks>Use an integer value for numeric identifiers or a string value for alphanumeric or custom
    /// identifiers. The type of the identifier may depend on the source or context in which it is used.</remarks>
    public OneOf<int, string> Id { get; set; }
}
