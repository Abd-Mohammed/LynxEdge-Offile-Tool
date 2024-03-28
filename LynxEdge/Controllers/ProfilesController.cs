using Application.Location.Queries;
using Application.Profile.Commands.CreateProfile;
using Application.Profile.Common;
using Application.Profile.Queries.GetAllProfiles;
using Application.Profile.Queries.GetProfileById;
using CsvHelper;
using LynxEdge.Managers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using SharedKernal.Data.Enums;
using SharedKernal.Models.Locations;
using SharedKernal.Models.Profile;
using SharedKernal.Models.VRP;
using System.Globalization;
using SharedKernal.Models.CsvModel;

namespace LynxEdge.Controllers;

public class ProfilesController : Controller
{

	private static List<Profile> _profiles = [];
	private static List<Location> _locations = [];
	private static int _dataStatus = (int)VariableStatus.InProgress;
	private static VrpOptimizationResponse _response = new();
	private static int _responseStatus = (int)VariableStatus.InProgress;


	private readonly ISender _sender;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public ProfilesController(ISender sender, IWebHostEnvironment webHostEnvironment)
	{
		_sender = sender;
		_webHostEnvironment = webHostEnvironment;
		_locations = [.. sender.Send(new GetAllLocationsQuery()).GetAwaiter().GetResult()];
	}

	public async Task<IActionResult> Index()
	{
		if (_dataStatus == (int)VariableStatus.InProgress)
		{
			var result = await _sender.Send(new GetAllProfilesQuery());
			_profiles = [.. result];
			_dataStatus = (int)VariableStatus.Processed;
		}

		return View(_profiles);
	}

	public IActionResult Create()
	{

		var profile = Profile.Create(string.Empty,
			string.Empty,
			default!,
			default!,
			default!,
			default!,
			string.Empty,
			string.Empty,
			string.Empty,
			[

				Vehicle.Create(string.Empty, null!,null!, null!, null!)
			]);

		return PartialView("_PopUp", profile);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProfile(IFormCollection form)
	{
		#region Peparing Data

		var id = _profiles.Count + 1;

		var vehiclesLabels = (form["vehicleLabel"]).ToArray();
		var vehiclesCount = form["VehicleCount"].ToArray();
		var vehiclesCapacity = form["vehicleCapacity"].ToArray();
		var vehiclesFuel = form["vehicleFuel"].ToArray();

		var profileName = form["ProfileName"].ToString();

		var startDepot = form["StartDepot"].ToString();
		var endDepot = form["EndDepot"].ToString();
		var onOffLoadTime = int.Parse(form["OnOffLoadTime"].ToString());
		var slackTime = int.Parse(form["SlackTime"].ToString());

		var csvFile = form.Files.FirstOrDefault();


		var vehicles = new Vehicle[vehiclesLabels.Length];

		for (var index = 0; index < vehicles.Length; index += 1)
		{
			vehicles[index] = Vehicle.Create(
				vehiclesLabels[index]!,
				Convert.ToInt32(vehiclesCount[index]),
				Convert.ToInt32(vehiclesCapacity[index]),
				Convert.ToDecimal(vehiclesFuel[index]),
				id
			);
		}


		#endregion

		#region Preparing Optimization Request

		var command = new CreateProfileCommand(new CreateProfileRequest(
			profileName,
			startDepot,
			endDepot,
			onOffLoadTime,
			slackTime,
			csvFile,
			vehicles));

		#endregion

	
		_response = await _sender.Send(command);

		_dataStatus = (int)VariableStatus.InProgress;
		_responseStatus = (int)VariableStatus.InProgress;


		//CreateCsvFile(csvFile.FileName, _response.VrpResponse);

		//var script = $@"
  //      var downloadLink = document.createElement('a');
  //      downloadLink.href = '/Profiles/DownloadCsv?id=#{id}';
  //      downloadLink.download = '{csvFileName}';
  //      document.body.appendChild(downloadLink);
  //      downloadLink.click();
  //      window.location.href = '/Profiles/Map'; ";

		//return Content($"<script>{script}</script>", "text/html");

		return RedirectToAction("Map");
	}

	[HttpGet]
	public IActionResult Map()
	{
		return View();
	}


	[HttpGet]
	public async Task<FileContentResult> DownloadCsv([FromQuery] int id)
	{
	   var fileName = await _sender.Send(new GetProfileByIdQuery(id));

		const string fileMimeType = "text/csv";

		var fileBytes = FileManager.Download(fileName, _webHostEnvironment.WebRootPath);

		return File(fileBytes, fileMimeType, fileName); 
	}

	[HttpGet]
	public string GetGeoJson()

	{
		if (_responseStatus == (int)VariableStatus.Processed)
		{
			_response.GeoJson = null!;
		}
		_responseStatus = (int)VariableStatus.Processed;

        return _response.GeoJson;
	}


	[HttpGet]
	public IActionResult AutoComplete(string prefix = "")
	{

		var results = _locations
			.Where(loc => loc.Address.StartsWith(prefix))
			.Select(l => new
			{
				Name = l.Address,
				Value = l.Address
			});

		return Json(results);
	}

	[HttpGet("/Backdoor/UAT/SeedsData")]
	public IActionResult SeedsData()
	{

		List<Location> locations = [];

		using (var parser = new TextFieldParser(@"d:\csv\AUH New Locations - Updated.csv"))
		{
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			parser.HasFieldsEnclosedInQuotes = true;

			var distanceMatrix2d = new List<string[]>();
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				if (fields.Any(f => !string.IsNullOrWhiteSpace(f)))
				{
					distanceMatrix2d.Add(fields);
				}
			}

			for (int i = 1; i < distanceMatrix2d.Count; i++)
			{

				var s = distanceMatrix2d[i][0];
				var ss = distanceMatrix2d[i][1].Split(',').Select(s => s.Trim()).ToArray();
				var lat = Convert.ToDecimal(ss[0]);
				var lng = Convert.ToDecimal(ss[1]);
				locations.Add(Location.Create(
					s.Trim().ToLower(),
					lat, lng
				));
			}

			// Now distanceMatrix2d contains the parsed CSV data without the last empty row
		}


		return Ok();
	}


	private  void CreateCsvFile(string fileName, VRPResponse routes)
	{

		var records = new List<CsvModel>();


		foreach (var route in routes.Routes)
		{

			records.Add(new CsvModel
			{
				BusType = route.VehicleType,
				BusNumber = route.VehicleNumber,
				TripStartTime = route.StartTime,
				TripEndTime = route.EndTime,
				TripDistance = route.Distance,
				DepartureTime = route.StartTime,
				StopAddress = route.Stops.FirstOrDefault()!.Location
			});


			// handling the rest of stop address 

			records.AddRange(route.Stops.Skip(1)
			.Select(stop => new CsvModel
			{
				StopAddress = stop.Location,
				DepartureTime = stop.DepartureTime,
				pickupCount = stop.PickupCount,
				DropOffCount = stop.DropOffCount,
				ArrivalTime = stop.ArrivalTime
			}));

		}

		var filePath = FileManager.GetFilePath(fileName, _webHostEnvironment.WebRootPath);

		using var writer = new StreamWriter(filePath);

		using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

		csv.WriteRecords(records);

	}
}