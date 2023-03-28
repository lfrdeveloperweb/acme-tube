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
            bool BypassValidation = false) : Command<CommandResult>(Context, BypassValidation);

        public sealed class CommandHandler : CommandHandler<Command>
        {
            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                ICommandValidator<Command> validator,
                IMapper mapper) : base(loggerFactory, unitOfWork, validator, mapper: mapper) { }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                if (await UnitOfWork.VideoRepository.GetByIdAsync(command.Id, cancellationToken)  is not { } video)
                {
                    return CommandResult.NotFound();
                }

                Mapper.Map(command, video);

                await UnitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);

                return CommandResult.NoContent();
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