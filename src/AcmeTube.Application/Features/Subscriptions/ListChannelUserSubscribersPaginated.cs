using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;

namespace AcmeTube.Application.Features.Subscriptions;

public static class ListChannelUserSubscribersPaginated
{
	public record Query(
		string ChannelId,
		PagingParameters PagingParameters,
		OperationContext OperationContext) : PaginatedQuery<PaginatedQueryResult<User>, User>(PagingParameters, OperationContext);

	public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<User>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public QueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PaginatedQueryResult<User>> Handle(Query query, CancellationToken cancellationToken)
		{
			var pagedResult = await _unitOfWork.SubscriptionRepository.ListSubscribersPaginatedByFilterAsync(query.ChannelId, query.PagingParameters, cancellationToken);

			return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
		}
	}
}