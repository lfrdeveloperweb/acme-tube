using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Resources;
using AcmeTube.Infrastructure.Settings;
using AutoMapper;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Infrastructure.Services.Dropbox;

public sealed class DropboxService : ExternalService, IFileStorageService, IHealthCheck
{
	private readonly IMapper _mapper;
	private readonly DropboxClient _client;

	public DropboxService(ILoggerFactory loggerFactory, IMapper mapper, DropboxSettings settings) : base(loggerFactory)
	{
		_mapper = mapper;

		_client = new DropboxClient(settings.AccessToken, new DropboxClientConfig
		{
			MaxRetriesOnError = settings.MaxRetriesOnError
		});
	}

	/// <summary>
	/// Dropbox service name.
	/// </summary>
	protected override string ServiceName => "Dropbox";

	/// <inheritdoc />
	public async Task<ProcessResult<FileUploaderResult>> UploadFileAsync(string path, byte[] fileContent, CancellationToken cancellationToken)
	{
		try
		{
			var fileMetadata = await _client.Files.UploadAsync(
				path: path,
				mode: WriteMode.Overwrite.Instance,
				body: new MemoryStream(fileContent));

			//await _client.Sharing.GetSharedLinkFileAsync(new GetSharedLinkMetadataArg(fileMetadata.PathLower));

			var sharedLinkMetadata = await _client.Sharing.CreateSharedLinkWithSettingsAsync(path);

			return ProcessResult.Success(new FileUploaderResult(
				fileMetadata.Id,
				fileMetadata.PathLower,
				ContentType: "",
				sharedLinkMetadata.Url,
				fileMetadata.ClientModified
			));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to upload file '{FullName}'.", path);

			return ProcessResult.Failure<FileUploaderResult>(ReportCodeType.DropboxFailure);
		}
	}

	public async Task<ProcessResult<byte[]>> DownloadFileAsync(string path, CancellationToken cancellationToken)
	{
		try
		{
			using var response = await _client.Files.DownloadAsync(path);

			return ProcessResult.Success(await response.GetContentAsByteArrayAsync());
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to download from file '{FileId}'.", path);

			return ProcessResult.Failure<byte[]>(ReportCodeType.DropboxFailure);
		}
	}

	/// <summary>
	/// Delete a file by its identifier.
	/// </summary>
	/// <param name="path">Identifier of file</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Instance of <see cref="ProcessResult"/>.</returns>
	public async Task<ProcessResult> DeleteFileAsync(string path, CancellationToken cancellationToken)
	{
		try
		{
			var deleteResult = await _client.Files.DeleteV2Async(path);

			return ProcessResult.Success(_mapper.Map<FileUploaderResult>(deleteResult.Metadata.AsFile));
		}
		catch (ApiException<DeleteError> ex) when (ex.ErrorResponse.AsPathLookup.Value.IsNotFound)
		{
			return ProcessResult.Failure<FileUploaderResult>(ReportCodeType.ResourceNotFound);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to try delete file '{FileId}'.", path);

			return ProcessResult.Failure<FileUploaderResult>(ReportCodeType.DropboxFailure);
		}
	}

	/// <inheritdoc />
	async Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
	{
		try
		{
			var echoResult = await _client.Check.UserAsync(query: "check");

			return echoResult.Result.Equals("check")
				? HealthCheckResult.Healthy()
				: HealthCheckResult.Unhealthy();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex.Message, ex);

			return new(context.Registration.FailureStatus, exception: ex);
		}
	}
}