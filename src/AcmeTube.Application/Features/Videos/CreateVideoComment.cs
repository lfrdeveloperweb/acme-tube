using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Videos
{
	public static class CreateVideoComment
	{
		public sealed record Command(
			string VideoId,
			string Description,
			OperationContext Context) : Command<CommandResult<VideoComment>>(Context);

		public sealed class CommandHandler : CommandHandler<Command, CommandResult<VideoComment>>
		{
			private readonly ISystemClock _systemClock;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ICommandValidator<Command> validator,
				IMapper mapper,
				ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
			{
				_systemClock = systemClock;
			}

			protected override async Task<CommandResult<VideoComment>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var comment = Mapper.Map<VideoComment>(command);

				comment.VideoId = command.VideoId;
				comment.CreatedBy = Membership.From(command.OperationContext.Identity);
				comment.CreatedAt = _systemClock.UtcNow;

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
				//Transform(it => it.Title, it => it.Trim())
				//    .NotNullOrEmpty();

				//Transform(it => it.Description, it => it.Trim())
				//    .NotNullOrEmpty();

				//RuleFor(command => command.Level)
				//    .NotNullOrEmpty()
				//    .Must(level => Enum.IsDefined(typeof(CourseLevel), level));
				//.WithMessageFromErrorCode(ReportCodeType.InvalidCourseLevel);

				//RuleFor(request => request.Name)
				//    .NotNullOrEmpty();

				RuleFor(request => request)
					.CustomAsync(CanCreate);
			}

			/// <summary>
			/// Validate if can create TodoComment.
			/// </summary>
			private Task CanCreate(Command command, ValidationContext<Command> validationContext, CancellationToken cancellationToken)
			{
				//if (!RequestContext.Membership.Roles.Contains(Common.Models.Security.Role.Manager) && !RequestContext.Membership.IsSuperAdmin)
				//{
				//    validationContext.AddFailure("User", ReportCodeType.OnlyManagerIsAllowedToDoThisOperation);
				//    return;
				//}

				return Task.CompletedTask;
			}
		}
	}
}