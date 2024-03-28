using System.ComponentModel.DataAnnotations;
using SharedKernal.Common;

namespace SharedKernal.Models.Profile;
public class Vehicle: Entity<int?>
{
	[Required]
	public string? Label { get; private set; }
    [Required]
    public int? Count { get; private set; }
    [Required]
    public int? Capacity { get; private set; }
    [Required]
    public decimal? Fuel { get; private set; }

	public int? ProfileId { get; private set; }
	public virtual Profile Profile { get; private set; }

	private Vehicle(int? id,
					string label,
					int? count, 
					int? capacity,
					decimal? fuel,
					int? profileId): base(id)
	{
		Label = label;
		Count = count;
		Capacity = capacity;
		Fuel = fuel;
		ProfileId = profileId;
	}


	public static Vehicle Create(string label,
								 int? count,
								 int? capacity,
								 decimal? fuel,
								 int? profileId)
	{
		return new Vehicle(null!, label, count, capacity, fuel, profileId);
	}



#pragma warning disable CS8618
	private Vehicle() : base(null!) { }
#pragma warning restore CS8618

}
