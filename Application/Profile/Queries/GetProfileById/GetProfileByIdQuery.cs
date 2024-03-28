using MediatR;

namespace Application.Profile.Queries.GetProfileById;

public record GetProfileByIdQuery(int Id) : IRequest<string>; 