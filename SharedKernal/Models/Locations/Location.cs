using SharedKernal.Common;

namespace SharedKernal.Models.Locations;
public sealed class Location: Entity<int?>
{
	public string Address { get; private set; }
	public decimal Lat { get; private set; }
	public decimal Lng { get; private set; }

	private Location(int? id, decimal lat, decimal lng, string address) : base(id)
	{
		Address = address; 
		Lat = lat;
		Lng = lng;

	}

	public static Location Create(string address, decimal lat, decimal lng)
	{
		return new Location(null!, lat, lng, address) ;
	}

#pragma warning disable CS8618
	private Location() : base(null!) { }
#pragma warning restore CS8618
}