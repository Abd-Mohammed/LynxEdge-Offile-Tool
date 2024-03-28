
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernal.Models.Locations;

namespace Infrastructure.Persistence.Configurations;

public sealed class LocationConfiguration: IEntityTypeConfiguration<Location>
{
	public void Configure(EntityTypeBuilder<Location> builder)
	{
		builder.ToTable("Locations");
		builder.HasKey(l => l.Id); 
		builder.HasIndex(l => l.Id);

		builder.Property(l => l.Address); 
		builder.Property(l => l.Lng).HasPrecision(18, 15); 
		builder.Property(l => l.Lat).HasPrecision(18, 15); 

		builder.Ignore(l => l.CreateDateTime);
		builder.Ignore(l => l.UpdatedDateTime);

	}
}