using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Resources;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Videos
{
    public static class CreateVideo
    {
        public sealed record Command(
            string Title,
            string Description,
            string ChannelId,
            ICollection<string> Tags,
            bool? IsPublic,
            byte[] FileContent,
            bool BypassValidation = false) : Command<CommandResult<Video>>(BypassValidation);

        public sealed class CommandHandler : CommandHandler<Command, CommandResult<Video>>
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

            protected override async Task<CommandResult<Video>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var video = Mapper.Map<Video>(command);

                video.Id = _keyGenerator.Generate();

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

            public CommandValidator(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                SetupValidation();
            }

            private void SetupValidation()
            {
                Transform(command => command.Title, it => it.Trim())
                    .NotNullOrEmpty();

                Transform(command => command.Description, it => it.Trim())
                    .NotNullOrEmpty();

                RuleFor(command => command.ChannelId)
                    .NotNullOrEmpty()
                    .MustAsync((channelId, cancellationToken) => _unitOfWork.ChannelRepository.ExistsAsync(channelId, cancellationToken))
                    .WithMessageFromErrorCode(ReportCodeType.InvalidChannel);

                RuleForEach(command => command.Tags)
	                .NotNullOrEmpty();

                RuleFor(command => command.IsPublic)
	                .NotNullOrEmpty();
			}
        }
    }
}