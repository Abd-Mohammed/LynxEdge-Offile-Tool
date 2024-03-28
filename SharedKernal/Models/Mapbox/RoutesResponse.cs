
using System.Text.Json.Serialization;

namespace SharedKernal.Models.Mapbox;

public class RoutesResponse
{
	[JsonPropertyName("routes")]
	public Route[]? Routes { get; set; }

	[JsonPropertyName("waypoints")]
	public Waypoint[]? Waypoints { get; set; }

	[JsonPropertyName("code")]
	public string? Code { get; set; }

	[JsonPropertyName("uuid")]
	public string? Uuid { get; set; }
}

public class Route
{
	[JsonPropertyName("weight_name")]
	public string? WeightName { get; set; }

	[JsonPropertyName("weight")]
	public decimal? Weight { get; set; }

	[JsonPropertyName("duration")]
	public decimal? Duration { get; set; }

	[JsonPropertyName("distance")]
	public decimal? Distance { get; set; }

	[JsonPropertyName("legs")]
	public Leg[]? Legs { get; set; }

	[JsonPropertyName("geometry")]
	public Geometry? Geometry { get; set; }
}

public class Geometry
{
	[JsonPropertyName("coordinates")]
	public decimal[][]? Coordinates { get; set; }

	[JsonPropertyName("type")]
	public string? Type { get; set; }
}

public class Leg
{
	[JsonPropertyName("via_waypoints")]
	public object[]? ViaWaypoints { get; set; }

	[JsonPropertyName("admins")]
	public Admin[]? Admins { get; set; }

	[JsonPropertyName("weight")]
	public decimal? Weight { get; set; }

	[JsonPropertyName("duration")]
	public decimal? Duration { get; set; }

	[JsonPropertyName("steps")]
	public Step[]? Steps { get; set; }

	[JsonPropertyName("distance")]
	public decimal? Distance { get; set; }

	[JsonPropertyName("summary")]
	public string? Summary { get; set; }
}

public class Admin
{
	[JsonPropertyName("iso_3166_1_alpha3")]
	public string? Iso3166_1_Alpha3 { get; set; }

	[JsonPropertyName("iso_3166_1")]
	public string? Iso3166_1 { get; set; }
}

public class Step
{
	[JsonPropertyName("intersections")]
	public Intersection[]? Intersections { get; set; }

	[JsonPropertyName("maneuver")]
	public Maneuver? Maneuver { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("duration")]
	public decimal? Duration { get; set; }

	[JsonPropertyName("distance")]
	public decimal? Distance { get; set; }

	[JsonPropertyName("driving_side")]
	public string? DrivingSide { get; set; }

	[JsonPropertyName("weight")]
	public decimal? Weight { get; set; }

	[JsonPropertyName("mode")]
	public string? Mode { get; set; }

	[JsonPropertyName("geometry")]
	public Geometry? Geometry { get; set; }

	[JsonPropertyName("ref")]
	public string? Ref { get; set; }

	[JsonPropertyName("destinations")]
	public string? Destinations { get; set; }

	[JsonPropertyName("exits")]
	public string? Exits { get; set; }
}

public class Intersection
{
	[JsonPropertyName("entry")]
	public bool[]? Entry { get; set; }

	[JsonPropertyName("bearings")]
	public long[]? Bearings { get; set; }

	[JsonPropertyName("duration")]
	public decimal? Duration { get; set; }

	[JsonPropertyName("mapbox_streets_v8")]
	public MapboxStreetsV8? MapboxStreetsV8 { get; set; }

	[JsonPropertyName("is_urban")]
	public bool? IsUrban { get; set; }

	[JsonPropertyName("admin_index")]
	public long? AdminIndex { get; set; }

	[JsonPropertyName("out")]
	public long? Out { get; set; }

	[JsonPropertyName("weight")]
	public decimal? Weight { get; set; }

	[JsonPropertyName("geometry_index")]
	public long? GeometryIndex { get; set; }

	[JsonPropertyName("location")]
	public decimal[]? Location { get; set; }

	[JsonPropertyName("in")]
	public long? In { get; set; }

	[JsonPropertyName("turn_weight")]
	public decimal? TurnWeight { get; set; }

	[JsonPropertyName("turn_duration")]
	public decimal? TurnDuration { get; set; }

	[JsonPropertyName("traffic_signal")]
	public bool? TrafficSignal { get; set; }

	[JsonPropertyName("lanes")]
	public Lane[]? Lanes { get; set; }

	[JsonPropertyName("classes")]
	public string[]? Classes { get; set; }

	[JsonPropertyName("toll_collection")]
	public TollCollection? TollCollection { get; set; }
}

public class Lane
{
	[JsonPropertyName("indications")]
	public string[]? Indications { get; set; }

	[JsonPropertyName("valid")]
	public bool? Valid { get; set; }

	[JsonPropertyName("active")]
	public bool? Active { get; set; }

	[JsonPropertyName("valid_indication")]
	public string? ValidIndication { get; set; }
}

public class MapboxStreetsV8
{
	[JsonPropertyName("class")]
	public string? Class { get; set; }
}

public class TollCollection
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("type")]
	public string? Type { get; set; }
}

public class Maneuver
{
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	[JsonPropertyName("instruction")]
	public string? Instruction { get; set; }

	[JsonPropertyName("bearing_after")]
	public long? BearingAfter { get; set; }

	[JsonPropertyName("bearing_before")]
	public long? BearingBefore { get; set; }

	[JsonPropertyName("location")]
	public decimal[]? Location { get; set; }

	[JsonPropertyName("modifier")]
	public string? Modifier { get; set; }
}

public class Waypoint
{
	[JsonPropertyName("distance")]
	public decimal? Distance { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("location")]
	public decimal[]? Location { get; set; }
}