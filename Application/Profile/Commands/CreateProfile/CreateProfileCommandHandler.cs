using System.Collections.Frozen;
using CsvHelper;
using System.Text.Json;
using Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Refit;
using SharedKernal.Data.Enums;
using SharedKernal.DTO;
using SharedKernal.Models.VRP;
using SharedKernal.Persistence;
using SharedKernal.Persistence.IOptimizationApi;
using SharedKernal.Persistence.IRoutingBuilder;
using SharedKernal.Persistence.Repositories;
using System.Globalization;
using SharedKernal.Models.ZoneFullDay;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using SharedKernal.Models.Profile;


namespace Application.Profile.Commands.CreateProfile;

public sealed class CreateProfileCommandHandler(
	IUnitOfWork unitOfWork,
	ILocationRepository locationRepository,
	IOptimizationApi optimizationApi,
	IProfileRepository profileRepository)

	: IRequestHandler<CreateProfileCommand, VrpOptimizationResponse>
{
	private static FrozenDictionary<string, (decimal Lat, decimal Lng)>? _locations;

	public async Task<VrpOptimizationResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
	{
		_locations ??= locationRepository.GetAllLocations().ToFrozenDictionary(
			loc => loc.Address.ToLower(),
			loc => (loc.Lat, loc.Lng)
		);


		var vehiclesRequest = ConvertVehicleToVehicleRequest(
			request.Request.Vehicles,
			request.Request.StartDepot,
			request.Request.EndDepot).ToArray();


		List<Passenger> passengers = [];

		GetDataFromCsv(request.Request.CsvFile, ref passengers, request.Request.SlackTime, request.Request.OnOffLoadTime);


		var vrpRequest = VRPRequest.Build(vehiclesRequest, [.. passengers]);

		var jsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		var summary = JsonSerializer.Serialize(vrpRequest, jsonSerializerOptions);

		Console.WriteLine(summary);
		Console.WriteLine(passengers.Count);


		var routingBuilderClient = RestService.For<IRoutingBuilder>("https://api.mapbox.com");

		HashSet<KeyValuePair<decimal, decimal>> coordinates = [];

		var response =  await optimizationApi.GetRoutes(vrpRequest);

		foreach (var route in response.Routes)
		{
			Console.WriteLine("Vechicle Number: " + route.VehicleNumber + " :\n");

			foreach (var stop in route.Stops.Take(route.Stops.Length - 1))
			{
				
				var coordinate = new KeyValuePair<decimal, decimal>(
					_locations[stop.Location.ToLower()].Lng,
					_locations[stop.Location.ToLower()].Lat);
				
				coordinates.Add(coordinate);
			}
		}

		foreach (var coordinate in coordinates)
		{
			Console.WriteLine($"[{coordinate.Key},{coordinate.Value}]");
		}

		var res = await routingBuilderClient
			.GetRoutesFromTo(BuildCoordinateQuery(coordinates));

		var routes = string.Empty;

		if (res.Code == "Ok")
        {
            var stops = coordinates.Select(pair => new [] { pair.Key, pair.Value }).ToArray();

            routes = GeoJsonBuilder.Build(res.Routes!.FirstOrDefault()!.Geometry?.Coordinates!, stops);
        }

		var profile = SharedKernal.Models.Profile.Profile.Create(
			request.Request.ProfileName,
			request.Request.StartDepot,
			request.Request.EndDepot,
			request.Request.OnOffLoadTime,
			request.Request.SlackTime,
			vrpRequest.Vehicles.Length,
			summary,
			nameof(ProfileStatus.Success),
			request.Request.CsvFile.FileName,
			[.. request.Request.Vehicles]
		);

		await profileRepository.AddAsync(profile);
		await unitOfWork.Commit();

		return new VrpOptimizationResponse
		{
			VrpResponse = response,
			GeoJson = routes
		};
	}

	public static string BuildCoordinateQuery(HashSet<KeyValuePair<decimal, decimal>> coordinates)
	{
		var coordinatePairs = coordinates.Select(c => $"{c.Key},{c.Value}");
		var coordinatesAsQuery = string.Join(";", coordinatePairs);

		return coordinatesAsQuery;
	}


	public static void GetDataFromCsv(
		IFormFile file,
		ref List<Passenger> passengers,
		int waitingTime,
		int onOffLoadDuration)
	{
		// HACK: Handling extra spaces between words
		Regex regex = new("[ ]{2,}", RegexOptions.None);

		try
		{
			var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ",",
				HasHeaderRecord = false,
				TrimOptions = TrimOptions.Trim,
				MissingFieldFound = null,
				IgnoreBlankLines = true,
			};

			using var reader = new StreamReader(file.OpenReadStream());
			using var csv = new CsvReader(reader, csvConfiguration);

			var records = csv.GetRecords<ZoneFullDay>().Skip(1).ToList();

			foreach (var record in records)
			{
				DateTime? pickupReadyTime = null;
				DateTime? dropOffDueTime = null;

				var pickupAddress = regex.Replace(record.PickupLocation, " ").ToLower().Trim();
				var dropOffAddress = regex.Replace(record.DropOffLocation, " ").ToLower().Trim();

				if (!string.IsNullOrEmpty(record.PickupTime))
				{
					var date = record.PickupTime.Split(':');

					pickupReadyTime = new DateTime(DateOnly.FromDateTime(DateTime.Now),
						new TimeOnly(Convert.ToInt32(date[0]), Convert.ToInt32(date[1])));
				}

				if (!string.IsNullOrEmpty(record.DropOffTime))
				{
					var date = record.DropOffTime.Split(':');

					dropOffDueTime = new DateTime(DateOnly.FromDateTime(DateTime.Now),
						new TimeOnly(Convert.ToInt32(date[0]), Convert.ToInt32(date[1])));
				}

				passengers.Add(new Passenger
				{
					Demand = Convert.ToInt32(record.Demand),
					TrackingNo = Guid.NewGuid().ToString(),

					PickupLocation = new PickupLocation
					{
						Address = record.PickupLocation,
						Lat = _locations![pickupAddress].Lat,
						Lng = _locations![pickupAddress].Lng
					},

					DropOffLocation = new DropOffLocation()
					{
						Address = record.DropOffLocation,
						Lat = _locations![dropOffAddress].Lat,
						Lng = _locations![dropOffAddress].Lng
					},

					DeliveryDuration = waitingTime,
					PickupDuration = onOffLoadDuration,

					DropOffReadyTime = dropOffDueTime,
					PickupReadyTime = pickupReadyTime,

					PickupDueTime = pickupReadyTime?.AddMinutes(waitingTime),
					DropOffDueTime = dropOffDueTime?.AddMinutes(waitingTime),

					VolumeCapacity = 0,
					WeightCapacity = 0
				});
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	public static IEnumerable<VehicleRequest> ConvertVehicleToVehicleRequest(
		Vehicle[] vehicles,
		string startDepot,
		string endDepot)
	{
		List<VehicleRequest> vehiclesList = [];

		startDepot = startDepot.ToLower();
		endDepot = endDepot.ToLower();

		foreach (var vehicle in vehicles)
		{
			for (var i = 0; i < vehicle.Count; i += 1)
			{
				vehiclesList.Add(new VehicleRequest
				{
					StartDepot = new StartDepot
					{
						Location = new LocationDto(startDepot,
							_locations![startDepot].Lng,
							_locations![startDepot].Lat)
					},
					EndDepot = new EndDepot
					{
						Location = new LocationDto(endDepot,
							_locations![endDepot].Lng,
							_locations![endDepot].Lat)
					},
					VolumeCapacity = (int)vehicle.Capacity!,
					WeightCapacity = (int)vehicle.Capacity,
					FuelCost = vehicle.Fuel,
					Name = $"{vehicle.Label!}-{i}"
				});
			}
		}

		return vehiclesList;
	}
}