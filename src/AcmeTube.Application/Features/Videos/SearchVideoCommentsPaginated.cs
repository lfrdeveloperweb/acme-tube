using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;

namespace AcmeTube.Application.Features.Videos
{
	public static class SearchVideoCommentsPaginated
	{
		public record Query(string VideoId, PagingParameters PagingParameters, OperationContext OperationContext)
			: PaginatedQuery<PaginatedQueryResult<VideoComment>, VideoComment>(PagingParameters, OperationContext);

		public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<VideoComment>>
		{
			private readonly IUnitOfWork _unitOfWork;

			public QueryHandler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<PaginatedQueryResult<VideoComment>> Handle(Query query, CancellationToken cancellationToken)
			{
				var filter = new VideoCommentFilter
				{
					VideoId = query.VideoId
				};

				var pagedResult = await _unitOfWork.VideoRepository.ListCommentsPaginatedByFilterAsync(filter, query.PagingParameters, cancellationToken);

				return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
			}
		}
	}
}