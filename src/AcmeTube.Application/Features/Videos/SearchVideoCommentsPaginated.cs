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
		public sealed record Query(string VideoId, PagingParameters PagingParameters)
			: PaginatedQuery<PaginatedQueryResult<VideoComment>, VideoComment>(PagingParameters);

		public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<VideoComment>>
		{
			private readonly IUnitOfWork _unitOfWork;

			public QueryHandler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<PaginatedQueryResult<VideoComment>> Handle(Query query, CancellationToken cancellationToken)
			{
				var pagedResult = await _unitOfWork.VideoRepository.ListCommentsPaginatedByFilterAsync(query.VideoId, query.PagingParameters, cancellationToken);

				return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
			}
		}
	}
}