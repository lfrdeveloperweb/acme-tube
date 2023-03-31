using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;

namespace AcmeTube.Application.Features.Videos
{
	public static class SearchVideosPaginated
	{
		public record Query(PagingParameters PagingParameters)
			: PaginatedQuery<PaginatedQueryResult<Video>, Video>(PagingParameters);

		public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<Video>>
		{
			private readonly IUnitOfWork _unitOfWork;

			public QueryHandler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<PaginatedQueryResult<Video>> Handle(Query query, CancellationToken cancellationToken)
			{
				var pagedResult = await _unitOfWork.VideoRepository.ListPaginatedByFilterAsync(new VideoFilter(), query.PagingParameters, cancellationToken);

				return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
			}
		}
	}
}