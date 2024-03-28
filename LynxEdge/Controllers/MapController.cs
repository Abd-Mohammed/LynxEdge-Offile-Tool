using System.Collections.Frozen;
using System.Text.Json;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using SharedKernal.Models.VRP;
using SharedKernal.Persistence.Repositories;

namespace LynxEdge.Controllers;

public class MapController(ILocationRepository locationRepository)  : Controller
{
	private static FrozenDictionary<string, (decimal, decimal)> _locations; 

	public IActionResult Index()
	{
		_locations ??= locationRepository.GetAllLocations().ToFrozenDictionary(
			loc => loc.Address,
			loc => (loc.Lat, loc.Lng)
		);

		var vrpResponse = JsonSerializer.Deserialize<VRPResponse>(TempData["data"] as string ?? string.Empty)!;

		//var geoJson = GeoJsonBuilder.Build(vrpResponse, _locations);


		return View();
	}
}


