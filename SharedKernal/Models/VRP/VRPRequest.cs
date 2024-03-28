using SharedKernal.DTO;

namespace SharedKernal.Models.VRP;

public class VRPRequest
{
	public string Id => Guid.NewGuid().ToString();
	public Configuration Configuration { get; private set; }
	public VehicleRequest[] Vehicles { get; private set; }
	public Passenger[] Passengers { get; private set; }

 
    private VRPRequest(VehicleRequest[] vehicles, Passenger[] passengers)
	{
		Vehicles = vehicles;
		Passengers = passengers;
		Configuration = new Configuration
		{
			SolverConfiguration = new SolverConfiguration
			{
				ConstructionHeuristic = new ConstructionHeuristic(),
				DistanceMatrix = new DistanceMatrix(),
				Termination = new Termination()
			},
		};
	}


	public static VRPRequest Build(VehicleRequest[] vehicles, Passenger[] passengers)
	{
		return new VRPRequest(vehicles, passengers);
	}

}

public class Configuration
{
	public string Type => "VRP";
	public string TimeZoneId => "Jordan Standard Time";
	public SolverConfiguration SolverConfiguration { get; set; }
}

public class SolverConfiguration
{
	public bool EnablePartitioning => true;
	public Termination Termination { get; set; }
	public ConstructionHeuristic ConstructionHeuristic { get; set; }
	public DistanceMatrix DistanceMatrix { get; set; }
}

public class Termination
{
	public int SecondsSpentLimit => 900;
}

public class ConstructionHeuristic
{
	public string Type => "FIRST_FIT_DECREASING";
}

public class DistanceMatrix
{
	public string Type => "FILE";
	public string Path => "dubai_distance_matrix.csv";
}

public class VehicleRequest
{
	public string Name { get; set; }
	public StartDepot StartDepot { get; set; }
	public EndDepot EndDepot { get; set; }
	public decimal? FuelCost { get; set; }
	public int WeightCapacity { get; set; }
	public int VolumeCapacity { get; set; }
}

public class StartDepot
{
	public LocationDto Location { get; set; }
	public DateTime Time { get; } = new(DateOnly.FromDateTime(DateTime.Now), new TimeOnly());
}


public class EndDepot
{
	public LocationDto Location { get; set; }
	public DateTime Time { get; set; } = new(DateOnly.FromDateTime(DateTime.Now),
												new TimeOnly(23, 15, 0));
}


public class Passenger
{
	public int Demand { get; set; }
	public string TrackingNo { get; set; }
	public PickupLocation PickupLocation { get; set; }
	public DropOffLocation DropOffLocation { get; set; }
	public int PickupDuration { get; set; }
	public int DeliveryDuration { get; set; }
	public DateTime? PickupReadyTime { get; set; }
	public DateTime? PickupDueTime { get; set; }
	public DateTime? DropOffReadyTime { get; set; }
	public DateTime? DropOffDueTime { get; set; }

	public int WeightCapacity { get; set; }
	public int VolumeCapacity { get; set; }
}

public class PickupLocation
{
	public string Address { get; set; }
	public decimal Lat { get; set; }
	public decimal Lng { get; set; }
}

public class DropOffLocation
{
	public string Address { get; set; }
	public decimal Lat { get; set; }
	public decimal Lng { get; set; }
}
