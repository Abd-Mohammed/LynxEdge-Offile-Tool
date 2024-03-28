namespace SharedKernal.Models.ZoneFullDay;

public class ZoneFullDay
{
	public string PickupLocation { get; set; }
	public string DropOffLocation { get; set; }
	public string Demand { get; set; }
	public string? PickupTime { get; set; }
	public string? DropOffTime { get; set; }
}