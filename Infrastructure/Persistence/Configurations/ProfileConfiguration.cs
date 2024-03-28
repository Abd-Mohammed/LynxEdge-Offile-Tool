using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernal.Models.Profile;

namespace Infrastructure.Persistence.Configurations;

public sealed class ProfileConfiguration: IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder)
	{
		builder.ToTable("Profiles");

		builder.HasKey(x => x.Id);
		builder.HasIndex(x => x.Id).IsUnique();

		builder.Property(p => p.Name);
		builder.Property(p => p.EndDepot);
		builder.Property(p => p.StartDepot);
		builder.Property(p => p.SlackTime);
		builder.Property(p => p.OnOffLoadTime);
		builder.Property(p => p.Status);
		builder.Property(p => p.FileName);

		builder.Property(p => p.Summary).HasColumnType("NVARCHAR(MAX)");


		builder.Ignore(p => p.CreateDateTime);
		builder.Ignore(p => p.UpdatedDateTime);

		builder.HasMany(p => p.Vehicles)
			.WithOne(v => v.Profile)
			.HasForeignKey(v => v.ProfileId);

		builder.Metadata
			.FindNavigation(nameof(Profile.Vehicles))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
}