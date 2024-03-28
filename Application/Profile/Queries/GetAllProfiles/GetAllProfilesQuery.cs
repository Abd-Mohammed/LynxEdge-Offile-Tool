
using MediatR;

namespace Application.Profile.Queries.GetAllProfiles;
public record GetAllProfilesQuery(): IRequest<IQueryable<SharedKernal.Models.Profile.Profile>>;