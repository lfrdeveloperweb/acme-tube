using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcmeTube.Data.Repositories;

public sealed class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
	public SubscriptionRepository(MainContext context) : base(context) { }

	public async Task<PaginatedResult<User>> ListSubscribersPaginatedByFilterAsync(string channelId, PagingParameters pagingParameters, CancellationToken cancellationToken)
	{
		var queryable = DbSetAsNoTracking
			.Where(it => it.ChannelId == channelId)
			.Select(it => it.User);

		return await ListPaginatedAsync(queryable, pagingParameters, cancellationToken);

	}

	public async Task SubscribeAsync(Subscription subscription, CancellationToken cancellationToken)
	{
		await DbSet.AddAsync(subscription, cancellationToken);
	}

	public Task<int> UnsubscribeAsync(string channelId, string membershipId, CancellationToken cancellationToken)
	{
		return DbSet
			.Where(it => it.ChannelId == channelId && it.MembershipId == membershipId)
			.ExecuteDeleteAsync(cancellationToken);
	}
}