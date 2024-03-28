using MediatR;

namespace Application.Location.Queries;

public record GetAllLocationsQuery(): IRequest<IQueryable<SharedKernal.Models.Locations.Location>>; 
