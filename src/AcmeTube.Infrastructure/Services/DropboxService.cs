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

namespace AcmeTube.Infrastructure.Services
{
	public sealed class DropboxService : ExternalService, IFileUploaderService, IHealthCheck
	{
		private readonly ILogger<DropboxService> _logger;
		private readonly IMapper _mapper;
		private readonly DropboxClient _storageClient;

		public DropboxService(ILogger<DropboxService> logger, IMapper mapper, DropboxSettings settings) : base(logger)
		{
			_logger = logger;
			_mapper = mapper;

			_storageClient = new DropboxClient(settings.AccessToken, new DropboxClientConfig
			{
				MaxRetriesOnError = settings.MaxRetriesOnError
			});
		}

		/// <summary>
		/// Dropbox service name.
		/// </summary>
		protected override string ServiceName => "Dropbox";

		public async Task<ProcessResult<Attachment>> UploadFileAsync(string fullName, byte[] fileContent)
		{
			try
			{
				var fileMetadata = await _storageClient.Files.UploadAsync(
					path: fullName,
					mode: WriteMode.Overwrite.Instance,
					body: new MemoryStream(fileContent));

				
				var sharedLinkMetadata = await _storageClient.Sharing.CreateSharedLinkWithSettingsAsync(fullName);

				//var attachment = _mapper.Map<Attachment>(fileMetadata);
				//attachment.Url = sharedLinkMetadata.Url;

				return ProcessResult.Success(new Attachment(
					fileMetadata.Id,
					fileMetadata.PathLower,
					ContentType: "",
					sharedLinkMetadata.Url,
					fileMetadata.ClientModified
				));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to upload file '{FullName}'.", fullName);

				return ProcessResult.Failure<Attachment>(ReportCodeType.DropboxFailure);
			}
		}

		public async Task<ProcessResult<byte[]>> DownloadFileAsync(string fileId)
		{
			try
			{
				using var response = await _storageClient.Files.DownloadAsync(fileId);

				return ProcessResult.Success(await response.GetContentAsByteArrayAsync());
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to download from file '{FileId}'.", fileId);

				return ProcessResult.Failure<byte[]>(ReportCodeType.DropboxFailure);
			}
		}

		/// <summary>
		/// Delete a file by its identifier.
		/// </summary>
		/// <param name="fileId">Identifier of file</param>
		/// <returns>Instance of <see cref="ProcessResult"/>.</returns>
		public async Task<ProcessResult<Attachment>> DeleteFileAsync(string fileId)
		{
			try
			{
				var deleteResult = await _storageClient.Files.DeleteV2Async(fileId);

				// if(deleteResult.Metadata.As)

				return ProcessResult.Success(_mapper.Map<Attachment>(deleteResult.Metadata.AsFile));
			}
			catch (ApiException<DeleteError> ex) when (ex.ErrorResponse.AsPathLookup.Value.IsNotFound)
			{
				return ProcessResult.Failure<Attachment>(ReportCodeType.ResourceNotFound);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to try delete file '{FileId}'.", fileId);

				return ProcessResult.Failure<Attachment>(ReportCodeType.DropboxFailure);
			}
		}

		/// <inheritdoc />
		async Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
		{
			try
			{
				var echoResult = await _storageClient.Check.UserAsync(query: "check");

				return echoResult.Result.Equals("check")
					? HealthCheckResult.Healthy()
					: HealthCheckResult.Unhealthy();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);

				return new (context.Registration.FailureStatus, exception: ex);
			}
		}
	}
}
