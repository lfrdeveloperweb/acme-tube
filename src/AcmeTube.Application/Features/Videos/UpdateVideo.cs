using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;
using AutoMapper;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Videos
{
    public static class UpdateVideo
    {
        public sealed record Command(
            string Id,
            string Title,
            string Description,
            string ChannelId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels,
            OperationContext Context,
            bool BypassValidation = false) : Command<CommandResult<Video>>(Context, BypassValidation);

        public sealed class CommandHandler : CommandHandler<Command, CommandResult<Video>>
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

            protected override async Task<CommandResult<Video>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var todo = await UnitOfWork.VideoRepository.GetByIdAsync(command.Id, cancellationToken);
                if (todo is null)
                {
                    return CommandResult.NotFound<CommandResult<Video>>();
                }

                Mapper.Map(command, todo);

                todo.UpdatedBy = Membership.From(command.OperationContext.Identity);
                todo.UpdatedAt = _systemClock.UtcNow;

                await UnitOfWork.VideoRepository.CreateAsync(todo, cancellationToken);

                return CommandResult.Created(todo);
            }
        }

        /// <summary>
        /// Validator to validate request information about <see cref="Video"/>.
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
                RuleFor(request => request.Title)
                    .NotNullOrEmpty();
            }
        }
    }
}