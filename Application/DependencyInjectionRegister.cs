using Microsoft.Extensions.DependencyInjection;
using Refit;
using SharedKernal.Persistence.IOptimizationApi;

namespace Application;

public static class DependencyInjectionRegister
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

		services.AddRefitClient<IOptimizationApi>()
			.ConfigureHttpClient(cfg =>
			{
				cfg.BaseAddress = 
					new Uri("https://beta-lynxedgeapis-optimization.azurewebsites.net/api");
			});


		return services;
	}
}