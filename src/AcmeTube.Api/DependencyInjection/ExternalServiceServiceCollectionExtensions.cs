using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeTube.Api.DependencyInjection;

public static class ExternalServiceServiceCollectionExtensions
{
	/// <summary>
	/// This method configures dependencies from external services.
	/// </summary>
	public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
	{
			
		return services;
	}
}