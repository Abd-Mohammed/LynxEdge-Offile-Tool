
using MediatR;
using SharedKernal.Persistence.Repositories;

namespace Application.Profile.Queries.GetAllProfiles;

public sealed class GetAllProfilesQueryHandler(IProfileRepository profileRepository)
	:IRequestHandler<GetAllProfilesQuery, IQueryable<SharedKernal.Models.Profile.Profile>>
{
	public async Task<IQueryable<SharedKernal.Models.Profile.Profile>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;

		var result = profileRepository.GetAll();
		return result;
	}
}