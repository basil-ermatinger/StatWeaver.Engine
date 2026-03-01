using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StatWeaver.Engine.Infrastructure.DbContexts;

namespace StatWeaver.Engine.Infrastructure.ServiceRegistration;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		string? connectionString = configuration.GetConnectionString("StatWeaverDbConnectionString");
		services.AddDbContext<StatWeaverDbContext>(options => options.UseSqlServer(connectionString));
		return services;
	}
}
