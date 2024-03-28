using SharedKernal.Models.Locations;

namespace SharedKernal.Persistence.Repositories;

public interface ILocationRepository
{
	IQueryable<Location> GetAllLocations(); 
}