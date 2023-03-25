using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Videos
{
	public static class DeleteVideoComment
	{
		public record Command(string Id, string VideoId, OperationContext Context) : Command<CommandResult>(Context);

		public sealed class CommandHandler : CommandHandler<Command>
		{
			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

			protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				if (await UnitOfWork.VideoRepository.GetCommentByIdAsync(command.Id, cancellationToken) is { } comment &&
				    comment.VideoId == command.VideoId)
				{
					await UnitOfWork.VideoRepository.DeleteCommentAsync(comment, cancellationToken);

					return CommandResult.Ok();
				}

				return CommandResult.NotFound();
			}
		}
	}
}
