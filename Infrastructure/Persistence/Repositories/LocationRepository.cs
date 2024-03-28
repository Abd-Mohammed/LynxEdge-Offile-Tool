
using Microsoft.EntityFrameworkCore;
using SharedKernal.Models.Locations;
using SharedKernal.Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories;

public class LocationRepository(ApplicationDbContext db): ILocationRepository
{
	public IQueryable<Location> GetAllLocations()
	{
		var locations = db.Set<Location>().AsNoTracking();
		
		return locations;
	}
}