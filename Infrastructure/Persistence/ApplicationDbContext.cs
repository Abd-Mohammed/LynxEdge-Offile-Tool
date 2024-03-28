
using Microsoft.EntityFrameworkCore;
using SharedKernal.Models.Locations;
using SharedKernal.Models.Profile;

namespace Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions options) 
	: DbContext(options)
{
	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<Vehicle> Vehicles { get; set; } = null!;
	public DbSet<Location> Locations { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}

}