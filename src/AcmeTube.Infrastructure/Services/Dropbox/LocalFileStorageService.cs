using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Resources;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Infrastructure.Services.Dropbox;

public sealed class LocalFileStorageService : ExternalService, IFileStorageService
{
	private static readonly string RootStoragePath;

	private readonly IFileSystem _fileSystem;
	private readonly ISystemClock _systemClock;

	static LocalFileStorageService()
	{
		RootStoragePath = Path.Combine(Path.GetTempPath(), "acme-tube");
	}

	public LocalFileStorageService(
		ILoggerFactory loggerFactory,
		IFileSystem fileSystem,
		ISystemClock systemClock) : base(loggerFactory)
	{
		_fileSystem = fileSystem;
		_systemClock = systemClock;
	}

	/// <summary>
	/// Dropbox service name.
	/// </summary>
	protected override string ServiceName => "Local storage";

	/// <inheritdoc />
	public async Task<ProcessResult<FileUploaderResult>> UploadFileAsync(string path, byte[] fileContent, CancellationToken cancellationToken)
	{
		try
		{
			var fileInfo = _fileSystem.FileInfo.FromFileName(Path.Combine(RootStoragePath, path.TrimStart('/')));
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}

			if (!fileInfo.Directory.Exists)
			{
				_fileSystem.Directory.CreateDirectory(fileInfo.Directory.FullName);
			}

			await using var file = _fileSystem.File.Create(fileInfo.FullName);
			await file.WriteAsync(fileContent, cancellationToken);

			return ProcessResult.Success(new FileUploaderResult(
				Path.GetFileNameWithoutExtension(fileInfo.FullName),
				path,
				ContentType: "",
				fileInfo.FullName,
				_systemClock.UtcNow.DateTime
			));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to upload file '{FullName}'.", path);

			return ProcessResult.Failure<FileUploaderResult>(ReportCodeType.DropboxFailure);
		}
	}

	/// <inheritdoc />
	public async Task<ProcessResult<byte[]>> DownloadFileAsync(string path, CancellationToken cancellationToken)
	{
		try
		{
			return ProcessResult.Success(await _fileSystem.File.ReadAllBytesAsync(path, cancellationToken));
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to download from file '{FileId}'.", path);

			return ProcessResult.Failure<byte[]>(ReportCodeType.DropboxFailure);
		}
	}

	/// <inheritdoc />
	public Task<ProcessResult> DeleteFileAsync(string path, CancellationToken cancellationToken)
	{
		try
		{
			_fileSystem.File.Delete(path);

			return Task.FromResult(ProcessResult.Success());
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to try delete file '{FileId}'.", path);

			return Task.FromResult(ProcessResult.Failure(ReportCodeType.DropboxFailure));
		}
	}

}