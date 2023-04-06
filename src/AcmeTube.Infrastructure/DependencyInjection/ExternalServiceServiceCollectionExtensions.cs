using AcmeTube.Application.Services;
using AcmeTube.Infrastructure.Services.Dummies;
using AcmeTube.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AcmeTube.Infrastructure.DependencyInjection;

public static class ExternalServiceServiceCollectionExtensions
{
	/// <summary>
	/// This method configures dependencies from external services.
	/// </summary>
	public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
	{
		var dropboxSettings = configuration.GetSection("externalServices:dropboxSettings").Get<DropboxSettings>();
		services.AddSingleton(dropboxSettings);
			
		services.RemoveAll<IFileStorageService>();

		services.AddScoped<IFileStorageService, LocalFileStorageService>();

		//services.AddScoped<IFileStorageService>(provider => new DropboxService(
		//	provider.GetRequiredService<ILoggerFactory>(),
		//	new MapperConfiguration(cfg => cfg.AddProfile<DropboxProfile>()).CreateMapper(),
		//	dropboxSettings));

		return services;
	}
}