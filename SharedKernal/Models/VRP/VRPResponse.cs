

namespace SharedKernal.Models.VRP;


public class VRPResponse
{
	public Route[] Routes { get; set; }
}

public class Route
{
	public string VehicleType { get; set; }
	public string VehicleNumber { get; set; }
	public int Distance { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }
	public Stop[] Stops { get; set; }
	public Activity[] Activities { get; set; }
}

public class Stop
{
	public string Location { get; set; }
	public string ArrivalTime { get; set; }
	public string DepartureTime { get; set; }
	public int PickupCount { get; set; }
	public int DropOffCount { get; set; }
}

public class Activity
{
	public int Type { get; set; }
	public string Location { get; set; }
	public string ArrTime { get; set; }
	public string EndTime { get; set; }
	public int Distance { get; set; }
	public int ServiceDuration { get; set; }
	public int Demand { get; set; }
	public Passenger Passenger { get; set; }
}

