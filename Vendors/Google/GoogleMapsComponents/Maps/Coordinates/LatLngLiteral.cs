using System.Diagnostics;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

[DebuggerDisplay("{Lat}, {Lng}")]
public class LatLngLiteral
{
    #region Constructors

    public LatLngLiteral()
    {
    }
    public LatLngLiteral(decimal lat, decimal lng) : this(Convert.ToDouble(lat), Convert.ToDouble(lng))
    {
    }
    public LatLngLiteral(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }    

    #endregion

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
    
}