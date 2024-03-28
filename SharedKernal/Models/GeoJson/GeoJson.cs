
using System.Text.Json.Serialization;

namespace SharedKernal.Models.GeoJson;

public class GeoJson
{
	public string Type { get; set; }
	public List<Feature> Features { get; set; }
}

public class Feature
{
	public string Type { get; set; }
	public Geometry Geometry { get; set; }
    public Properties Properties { get; set; }
}

public class Geometry
{
	public string Type { get; set; }
	public object Coordinates { get; set; }
}

public class Properties
{
	public string? Id { get; set; } = null!; 
	[JsonPropertyName("marker-color")]
	public string? MarkerColor { get; set; } = null!;
	[JsonPropertyName("marker-size")]
	public string? MarkerSize { get; set; } = null!;
    [JsonPropertyName("marker-symbol")]
    public string? MarkerSymbol { get; set; } = null!; 
}


