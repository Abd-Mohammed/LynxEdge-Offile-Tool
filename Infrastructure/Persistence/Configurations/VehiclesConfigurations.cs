using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernal.Models.Profile;

namespace Infrastructure.Persistence.Configurations;
public class VehiclesConfigurations: IEntityTypeConfiguration<Vehicle>
{
	public void Configure(EntityTypeBuilder<Vehicle> builder)
	{
		builder.ToTable("Vehicles");
		builder.HasKey(s => s.Id);
		builder.HasIndex(s => s.Id);
		builder.Property(v => v.Count);
		builder.Property(v => v.Capacity);
		builder.Property(v => v.Fuel).HasPrecision(6, 4);
		builder.Property(v => v.Label);

		builder.HasOne(v => v.Profile)
			.WithMany(s => s.Vehicles)
			.HasForeignKey(p => p.ProfileId);

		builder.Ignore(v => v.CreateDateTime);
		builder.Ignore(v => v.UpdatedDateTime);
	}
}
