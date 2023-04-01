using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Subscriptions;

public static class ListUserChannelSubscribersPaginated
{
	public record Query(
		PagingParameters PagingParameters) : PaginatedQuery<PaginatedQueryResult<Channel>, Channel>(PagingParameters);

	public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<Channel>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public QueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PaginatedQueryResult<Channel>> Handle(Query query, CancellationToken cancellationToken)
		{
			var pagedResult = await _unitOfWork.SubscriptionRepository.ListChannelsPaginatedByFilterAsync(query.GetMembershipId(), query.PagingParameters, cancellationToken);

			return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
		}
	}
}