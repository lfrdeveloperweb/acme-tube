using AcmeTube.Application.Services;
using AcmeTube.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeTube.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
	/// <summary>
	/// This method configures dependencies from external services.
	/// </summary>
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		var mediaSettings = configuration.GetSection(nameof(MediaSettings)).Get<MediaSettings>();

		services.AddSingleton(mediaSettings);
		
		services.AddScoped<AccountAppService>();
		services.AddScoped<UserAppService>();
		services.AddScoped<ChannelAppService>();
		services.AddScoped<SubscriptionAppService>();
		services.AddScoped<VideoAppService>();

		return services;
	}
}