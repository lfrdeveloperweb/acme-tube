using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Channels
{
	public static class CreateChannel
	{
		public sealed record Command(
			string Name,
			string Description,
			string CountryName,
			ICollection<string> Tags,
			IDictionary<string, string> Links,
			OperationContext Context) : Command<CommandResult<Channel>>(Context);

		public sealed class CommandHandler : CommandHandler<Command, CommandResult<Channel>>
		{
			private readonly IKeyGenerator _keyGenerator;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ICommandValidator<Command> validator,
				IMapper mapper,
				IKeyGenerator keyGenerator) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
			{
				_keyGenerator = keyGenerator;
			}

			protected override async Task<CommandResult<Channel>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var channel = Mapper.Map<Channel>(command);

				channel.Id = _keyGenerator.Generate();

				await UnitOfWork.ChannelRepository.CreateAsync(channel, cancellationToken);

				return CommandResult.Created(channel);
			}
		}

		/// <summary>
		/// Validator to validate request information about <see cref="Channel"/>.
		/// </summary>
		public sealed class CommandValidator : CommandValidator<Command>
		{
			private readonly IUnitOfWork _unitOfWork;

			public CommandValidator(IUnitOfWork unitOfWork, ISystemClock systemClock)
			{
				_unitOfWork = unitOfWork;

				SetupValidation();
			}

			private void SetupValidation()
			{
				Transform(command => command.Name, name => name?.Trim())
					.NotNullOrEmpty();

				Transform(command => command.Description, description => description?.Trim())
					.NotNullOrEmpty();
			}

			/// <summary>
			/// Validate if can create Channel.
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