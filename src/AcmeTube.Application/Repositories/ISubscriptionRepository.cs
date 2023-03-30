using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Repositories;

public interface ISubscriptionRepository
{
	Task<Subscription> GetAsync(string channelId, string membershipId, CancellationToken cancellationToken);

	Task<PaginatedResult<User>> ListSubscribersPaginatedByFilterAsync(string channelId, PagingParameters pagingParameters, CancellationToken cancellationToken);

	Task<PaginatedResult<Channel>> ListChannelsPaginatedByFilterAsync(string userId, PagingParameters pagingParameters, CancellationToken cancellationToken);

	Task SubscribeAsync(Subscription subscription, CancellationToken cancellationToken);

	Task<int> UnsubscribeAsync(Subscription subscription, CancellationToken cancellationToken);
}