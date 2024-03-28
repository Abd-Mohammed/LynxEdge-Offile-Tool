
using System.Security.AccessControl;

namespace SharedKernal.Models.CsvModel;

public class CsvModel
{
	public string BusType { get; set; }
	public string BusNumber { get; set; }
	public string TripStartTime { get; set; }
	public string TripEndTime { get; set; }
	public decimal TripDistance { get; set; }
	public string Duration { get; set; }
	public string Address { get; set; }
	public string StopAddress { get; set; }
	public string DepartureTime { get; set; }
	public string ArrivalTime { get; set; }
	public int pickupCount { get; set; }
	public int DropOffCount { get; set; }
}