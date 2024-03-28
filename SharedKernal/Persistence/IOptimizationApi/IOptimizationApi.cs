using SharedKernal.Models.VRP;
namespace SharedKernal.Persistence.IOptimizationApi;
using Refit; 

public interface IOptimizationApi
{
	[Post("/optimization/vrp")]
	Task<VRPResponse> GetRoutes([Body] VRPRequest request); 
}