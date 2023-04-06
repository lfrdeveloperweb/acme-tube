using Microsoft.Extensions.Logging;

namespace AcmeTube.Infrastructure.Services;

/// <summary>
/// The base ExternalService, it is meant to be inherited from by your custom ExternalService implementation.
/// </summary>
public abstract class ExternalService
{
	protected ExternalService(ILoggerFactory loggerFactory)
	{
		Logger = loggerFactory.CreateLogger(GetType());
	}

	/// <summary>
	/// Instance of <see cref="ILogger"/>.
	/// </summary>
	protected ILogger Logger { get; }

	/// <summary>
	/// Service name of external service.
	/// </summary>
	protected abstract string ServiceName { get; }
}