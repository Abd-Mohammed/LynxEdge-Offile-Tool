using Microsoft.EntityFrameworkCore;
using SharedKernal.Models.Profile;
using SharedKernal.Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class ProfileRepository(ApplicationDbContext db) : IProfileRepository
{
	public async Task<Profile?> GetByIdAsync(int id, bool withTrack = false)
	{
		if (withTrack)
		{
			return await db.Set<Profile>()
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		return await db.Set<Profile>().FirstOrDefaultAsync(p => p.Id == id);
	}

	public async Task AddAsync(Profile profile)
	{
		await db.Set<Profile>().AddAsync(profile);
	}

	public async Task AddVehiclesToProfile(IEnumerable<Vehicle> vehicles)
	{
		await db.Set<Vehicle>().AddRangeAsync(vehicles);
	}

	public void Update(Profile profile)
	{
		db.Set<Profile>().Update(profile);
	}

	public void Delete(Profile profile)
	{
		db.Set<Profile>().Remove(profile);
	}

	public IQueryable<Profile> GetAll(bool withTrack = false)
	{
		var result = db.Set<Profile>()
			.AsNoTracking()
			.Include(s => s.Vehicles)
			.AsSplitQuery();

		return result;
	}
}