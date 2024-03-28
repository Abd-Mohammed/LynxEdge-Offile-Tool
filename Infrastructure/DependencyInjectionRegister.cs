using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Persistence;
using SharedKernal.Persistence.Repositories;

namespace Infrastructure;

public static class DependencyInjectionRegister
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
															IConfiguration configuration)
	{
		services.AddPersistence(configuration);

		return services;
	}

	public static IServiceCollection AddPersistence(this IServiceCollection services,
														 IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(opt =>
			opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IProfileRepository, ProfileRepository>(); 
		services.AddScoped<IUnitOfWork, UnitOfWork>(); 
		services.AddScoped<ILocationRepository, LocationRepository>(); 

		return services;
	}
}