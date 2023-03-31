using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Videos
{
	public static class CreateVideoComment
	{
		public sealed record Command(
			string VideoId,
			string Description) : Command<CommandResult<VideoComment>>;

		public sealed class CommandHandler : CommandHandler<Command, CommandResult<VideoComment>>
		{
			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ICommandValidator<Command> validator,
				IMapper mapper) : base(loggerFactory, unitOfWork, validator, mapper: mapper) { }

			protected override async Task<CommandResult<VideoComment>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var comment = Mapper.Map<VideoComment>(command);
				comment.Video = await UnitOfWork.VideoRepository.GetByIdAsync(command.VideoId, cancellationToken);
				
				await UnitOfWork.VideoRepository.CreateCommentAsync(comment, cancellationToken);

				return CommandResult.Created(comment);
			}
		}

		/// <summary>
		/// Validator to validate request information about <see cref="VideoComment"/>.
		/// </summary>
		public sealed class CommandValidator : CommandValidator<Command>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly ISystemClock _dateTimeProvider;

			public CommandValidator(IUnitOfWork unitOfWork, ISystemClock dateTimeProvider)
			{
				_unitOfWork = unitOfWork;
				_dateTimeProvider = dateTimeProvider;

				SetupValidation();
			}

			private void SetupValidation()
			{
				Transform(it => it.Description, it => it.Trim())
				    .NotNullOrEmpty();
			}
		}
	}
}