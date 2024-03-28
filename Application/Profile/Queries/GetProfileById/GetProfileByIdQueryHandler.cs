
using MediatR;
using SharedKernal.Persistence.Repositories;

namespace Application.Profile.Queries.GetProfileById;

public class GetProfileByIdQueryHandler(IProfileRepository profileRepository): IRequestHandler<GetProfileByIdQuery, string>
{
	public async Task<string> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
	{
		var profile = await profileRepository.GetByIdAsync(request.Id, true);

		return profile!.FileName; 
	}
}