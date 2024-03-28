using System.Linq.Expressions;
using SharedKernal.Models.Profile;

namespace SharedKernal.Persistence.Repositories;
public interface IProfileRepository
{
	Task<Profile?> GetByIdAsync(int id, bool withTrack = false);
	Task AddAsync(Profile profile);
	Task AddVehiclesToProfile(IEnumerable<Vehicle> vehicles);
	void Update(Profile profile);
	void Delete(Profile profile);
	IQueryable<Profile> GetAll(bool withTrack = false);

}