using Application.Profile.Common;
using MediatR;
using SharedKernal.Models.VRP;

namespace Application.Profile.Commands.CreateProfile;

public record CreateProfileCommand(CreateProfileRequest Request)
	: IRequest<VrpOptimizationResponse>;
