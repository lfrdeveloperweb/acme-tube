using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Channels
{
	public static class CreateChannel
	{
		public sealed record Command(
			string Title,
			string Color,
			OperationContext Context) : Command<CommandResult<Channel>>(Context);

		public sealed class CommandHandler : CommandHandler<Command, CommandResult<Channel>>
		{
			private readonly IKeyGenerator _keyGenerator;
			private readonly ISystemClock _systemClock;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ICommandValidator<Command> validator,
				IMapper mapper,
				IKeyGenerator keyGenerator,
				ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
			{
				_keyGenerator = keyGenerator;
				_systemClock = systemClock;
			}

			protected override async Task<CommandResult<Channel>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var project = Mapper.Map<Channel>(command);

				project.Id = _keyGenerator.Generate();
				project.CreatedBy = Membership.From(command.OperationContext.Identity);
				project.CreatedAt = _systemClock.UtcNow;

				await UnitOfWork.ChannelRepository.CreateAsync(project, cancellationToken);

				return CommandResult.Created(project);
			}
		}

		/// <summary>
		/// Validator to validate request information about <see cref="Channel"/>.
		/// </summary>
		public sealed class CommandValidator : CommandValidator<Command>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly ISystemClock _systemClock;

			public CommandValidator(IUnitOfWork unitOfWork, ISystemClock systemClock)
			{
				_unitOfWork = unitOfWork;
				_systemClock = systemClock;

				SetupValidation();
			}

			private void SetupValidation()
			{
				Transform(it => it.Title, it => it.Trim())
					.NotNullOrEmpty();

				RuleFor(request => request)
					.CustomAsync(CanCreate);
			}

			/// <summary>
			/// Validate if can create Project.
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