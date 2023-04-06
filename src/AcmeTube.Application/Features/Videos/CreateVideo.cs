using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Features.Commons;
using AcmeTube.Application.Repositories;
using AcmeTube.Application.Settings;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Resources;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Services;
using System.Linq;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Features.Videos;

public static class CreateVideo
{
	public sealed record Command(
		string Title,
		string Description,
		string ChannelId,
		ICollection<string> Tags,
		bool? IsPublic,
		FileMetadata File,
		bool BypassValidation = false) : Command<CommandResult<Video>>(BypassValidation);

	public sealed class CommandHandler : CommandHandler<Command, CommandResult<Video>>
	{
		private readonly IMediaService _mediaService;
		private readonly IFileStorageService _fileStorageService;
		private readonly IKeyGenerator _keyGenerator;
		private readonly MediaSettings _mediaSettings;

		public CommandHandler(
			ILoggerFactory loggerFactory,
			IUnitOfWork unitOfWork,
			ICommandValidator<Command> validator,
			IMapper mapper,
			IMediaService mediaService,
			IFileStorageService fileStorageService,
			IKeyGenerator keyGenerator,
			MediaSettings mediaSettings) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
		{
			_mediaService = mediaService;
			_fileStorageService = fileStorageService;
			_keyGenerator = keyGenerator;
			_mediaSettings = mediaSettings;
		}

		protected override async Task<CommandResult<Video>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
		{
			var video = Mapper.Map<Video>(command);
			video.Id = _keyGenerator.Generate();
			video.VideoFilePath = string.Format(_mediaSettings.ChannelVideoPathTemplate + "/media{2}", video.Channel.Id, video.Id, command.File.Extension);
			video.ThumbnailFilePath = string.Format(_mediaSettings.ChannelVideoPathTemplate + "/thumbnail.gif", video.Channel.Id, video.Id);

			(video.Duration, var thumbnail) = await _mediaService.GetVideoMetadata(command.File.Content);

			var fileUploadedResult = await _fileStorageService.UploadFileAsync(video.VideoFilePath, command.File.Content, cancellationToken);
			if (!fileUploadedResult.Succeeded)
			{
				return CommandResult.UnprocessableEntity<CommandResult<Video>>(fileUploadedResult.Reports.ToArray());
			}

			var thumbnailUploadResult = await _fileStorageService.UploadFileAsync(video.ThumbnailFilePath, thumbnail, cancellationToken);
			if (!thumbnailUploadResult.Succeeded)
			{
				return CommandResult.UnprocessableEntity<CommandResult<Video>>(thumbnailUploadResult.Reports.ToArray());
			}

			video.VideoExternalId = fileUploadedResult.Data.Id;
			video.VideoUrl = fileUploadedResult.Data.Url;
			video.ThumbnailExternalId = fileUploadedResult.Data.Id;
			video.ThumbnailUrl = thumbnailUploadResult.Data.Url;

			await UnitOfWork.VideoRepository.CreateAsync(video, cancellationToken);

			return CommandResult.Created(video);
		}
	}

	/// <summary>
	/// Validator to validate request information about <see cref="Video"/>.
	/// </summary>
	public sealed class CommandValidator : CommandValidator<Command>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly MediaSettings _settings;

		public CommandValidator(IUnitOfWork unitOfWork, MediaSettings settings)
		{
			_unitOfWork = unitOfWork;
			_settings = settings;

			SetupValidation();
		}

		private void SetupValidation()
		{
			Transform(command => command.Title, it => it.Trim())
				.NotNullOrEmpty();

			Transform(command => command.Description, it => it.Trim())
				.NotNullOrEmpty();

			RuleFor(command => command.ChannelId)
				.NotNullOrEmpty()
				.MustAsync(_unitOfWork.ChannelRepository.ExistsAsync)
				.WithMessageFromErrorCode(ReportCodeType.InvalidChannel);

			RuleForEach(command => command.Tags)
				.NotNullOrEmpty();

			RuleFor(command => command.IsPublic)
				.NotNullOrEmpty();

			RuleFor(request => request.File)
				.NotNullOrEmpty()
				.SetValidator(new FileMetadataValidator(_settings.SupportedContentTypes, _settings.MaximumSizeInMegabytes));
		}
	}
}