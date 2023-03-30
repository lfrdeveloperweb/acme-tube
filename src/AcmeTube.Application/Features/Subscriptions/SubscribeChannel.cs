﻿using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Subscriptions;

public static class SubscribeChannel
{
    public sealed record Command(
        string ChannelId,
        OperationContext Context) : Command<CommandResult>(Context);

    public sealed class CommandHandler : CommandHandler<Command>
    {
        private readonly ISystemClock _systemClock;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ISystemClock systemClock) : base(loggerFactory, unitOfWork)
        {
            _systemClock = systemClock;
        }

        protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var channel = await UnitOfWork.ChannelRepository.GetByIdAsync(command.ChannelId, cancellationToken);
            var user = await UnitOfWork.UserRepository.GetByIdAsync(command.Context.Identity.Id, cancellationToken);

            //var subscription = new Subscription(channel, user, _systemClock.UtcNow);
            var subscription = new Subscription
            {
                ChannelId = channel.Id,
                MembershipId = user.Id,
                CreatedAt = _systemClock.UtcNow
            };

            await UnitOfWork.SubscriptionRepository.SubscribeAsync(subscription, cancellationToken);

            return CommandResult.NoContent();
        }
    }
}