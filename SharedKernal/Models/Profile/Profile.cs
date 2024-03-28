using System.ComponentModel.DataAnnotations;
using SharedKernal.Common;

namespace SharedKernal.Models.Profile;
public class Profile : Entity<int?>
{

	private readonly List<Vehicle> _vehicles = [];

	public string Name { get; private set; }
    public string StartDepot { get; private set; }
    public string EndDepot { get; private set; }
    public int OnOffLoadTime { get; private set; }
    public int SlackTime { get; private set; }
	public int NumberOfVehicles { get; private set; }
	public string Summary { get; private set; }
	public string Status { get; private set; }
	public string FileName { get; private set; }

	public IReadOnlyList<Vehicle> Vehicles => _vehicles.AsReadOnly();

	private Profile(int? id,
					string name,
					string startDepot,
					string endDepot,
					int onOffLoadTime,
					int slackTime,
					int numberOfVehicles,
					string summary,
					string status,
					string fileName,
					List<Vehicle> vehicles) : base(id)
	{
		Name = name;
		StartDepot = startDepot;
		EndDepot = endDepot;
		OnOffLoadTime = onOffLoadTime;
		SlackTime = slackTime;
		NumberOfVehicles = numberOfVehicles;
		_vehicles = vehicles;
		Summary = summary;
		Status = status;
		FileName = fileName;
	}

	public static Profile Create(string name,
								 string startDepot,
								 string endDepot,
								 int onOffLoadTime,
								 int slackTime,
								 int numberOfVehicles,
								 string summary,
								 string status,
								 string fileName,
								 List<Vehicle> vehicles = null!)
	{
		return new Profile(null!,
						   name,
						   startDepot,
						   endDepot,
						   onOffLoadTime,
						   slackTime,
						   numberOfVehicles,
						   summary,
						   status,
						   fileName,
						   vehicles);
	}
#pragma warning disable CS8618
	private Profile(): base(null!)
	{
	}
#pragma warning restore CS8618
}
