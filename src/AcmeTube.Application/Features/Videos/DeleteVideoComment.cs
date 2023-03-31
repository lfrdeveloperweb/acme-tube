using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Videos
{
	public static class DeleteVideoComment
	{
		public record Command(string Id, string VideoId) : Command<CommandResult>;

		public sealed class CommandHandler : CommandHandler<Command>
		{
			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

			protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				if (await UnitOfWork.VideoRepository.GetCommentByIdAsync(command.Id, cancellationToken) is not { } comment ||
				    comment.Video.Id != command.VideoId)
				{
					return CommandResult.NotFound();
				}
				
				await UnitOfWork.VideoRepository.DeleteCommentAsync(comment, cancellationToken);

				return CommandResult.NoContent();
			}
		}
	}
}
