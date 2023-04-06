using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Videos;

public static class DownloadVideo
{
	public sealed record Command(string Id) : Command<CommandResult<FileMetadata>>;

	public sealed class CommandHandler : CommandHandler<Command, CommandResult<FileMetadata>>
	{
		private readonly IFileStorageService _fileStorageService;
		
		public CommandHandler(
			ILoggerFactory loggerFactory,
			IUnitOfWork unitOfWork,
			IFileStorageService fileStorageService) : base(loggerFactory, unitOfWork)
		{
			_fileStorageService = fileStorageService;
		}

		protected override bool IgnoreGlobalTransaction => true;

		protected override async Task<CommandResult<FileMetadata>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
		{
			if (await UnitOfWork.VideoRepository.GetByIdAsync(command.Id, cancellationToken) is not { } video)
				return CommandResult.NotFound<CommandResult<FileMetadata>>();
			
			var fileDownloadResult = await _fileStorageService.DownloadFileAsync(video.VideoFilePath, cancellationToken);
			if (!fileDownloadResult.Succeeded)
			{
				return CommandResult.UnprocessableEntity<CommandResult<FileMetadata>>(fileDownloadResult.Reports.ToArray());
			}

			return CommandResult.Ok(new FileMetadata(
				Path.GetFileNameWithoutExtension(video.VideoFilePath),
				ContentType: video.VideoContentType,
				Size: fileDownloadResult.Data.Length,
				Content: fileDownloadResult.Data
			));
		}
	}
}