using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Videos
{
	public static class CloneVideo
	{
		public record Command(string VideoId, OperationContext Context) : Command<CommandResult<Video>>(Context);

		public sealed class CommandHandler : CommandHandler<Command, CommandResult<Video>>
		{
			private readonly ISender _dispatcher;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ISender dispatcher) : base(loggerFactory, unitOfWork)
			{
				_dispatcher = dispatcher;
			}

			protected override async Task<CommandResult<Video>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var todo = await UnitOfWork.VideoRepository.GetByIdAsync(command.VideoId, cancellationToken);
				if (todo is null)
				{
					return CommandResult.NotFound<CommandResult<Video>>();
				}

				var createTodoCommand = new CreateVideo.Command(
					todo.Title,
					todo.Description,
					todo.Channel.Id,
					todo.DueDate,
					todo.Priority,
					todo.Labels,
					command.OperationContext,
					BypassValidation: true);

				return await _dispatcher.Send(createTodoCommand, cancellationToken);
			}
		}
	}
}