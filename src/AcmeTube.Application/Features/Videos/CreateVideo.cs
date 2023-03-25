using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
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
    public static class CreateVideo
    {
        public sealed record Command(
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

            protected override async Task<CommandResult<Video>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var video = Mapper.Map<Video>(command);

                video.Id = _keyGenerator.Generate();
				video.CreatedBy = Membership.From(command.OperationContext.Identity);
				video.CreatedAt = _systemClock.UtcNow;

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
            /// Validate if can create Todo.
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