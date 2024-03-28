using MediatR;
using SharedKernal.Persistence.Repositories;

namespace Application.Location.Queries;

public class GetAllLocationsQueryHandler(ILocationRepository locationRepository): 
	IRequestHandler<GetAllLocationsQuery, IQueryable<SharedKernal.Models.Locations.Location>>
{
	public async Task<IQueryable<SharedKernal.Models.Locations.Location>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
		var locations = locationRepository.GetAllLocations();

		return locations;
	}
}