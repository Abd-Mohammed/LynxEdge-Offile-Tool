
using SharedKernal.Models.Mapbox;

namespace SharedKernal.Models.VRP;

public class VrpOptimizationResponse
{
	public VRPResponse VrpResponse { get; set; }
	public string GeoJson { get; set; }
}