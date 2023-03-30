using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Channel = AcmeTube.Domain.Models.Channel;

namespace AcmeTube.Data.Repositories;

public sealed class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
	public SubscriptionRepository(MainContext context) : base(context) { }

	public async Task<Subscription> GetAsync(string channelId, string membershipId, CancellationToken cancellationToken) =>
		await DbSet.FindAsync(new object[] { channelId, membershipId }, cancellationToken);

	public async Task<PaginatedResult<User>> ListSubscribersPaginatedByFilterAsync(string channelId, PagingParameters pagingParameters, CancellationToken cancellationToken)
	{
		var queryable = DbSetAsNoTracking
			.Where(it => it.Channel.Id == channelId)
			.Select(it => it.User);

		return await ListPaginatedAsync(queryable, pagingParameters, cancellationToken);
	}

	public async Task<PaginatedResult<Channel>> ListChannelsPaginatedByFilterAsync(string userId, PagingParameters pagingParameters, CancellationToken cancellationToken)
	{
		var queryable = DbSetAsNoTracking
			.Where(it => it.User.Id == userId)
			.Select(it => it.Channel);

		return await ListPaginatedAsync(queryable, pagingParameters, cancellationToken);
	}

	public async Task SubscribeAsync(Subscription subscription, CancellationToken cancellationToken) =>
		await CreateAsync(subscription, cancellationToken);

	public Task<int> UnsubscribeAsync(Subscription subscription, CancellationToken cancellationToken) =>
		DbSet.Where(it => it.ChannelId == subscription.ChannelId && it.MembershipId == subscription.MembershipId).ExecuteDeleteAsync(cancellationToken);
}