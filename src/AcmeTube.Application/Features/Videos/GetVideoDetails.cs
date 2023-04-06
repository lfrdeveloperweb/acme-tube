using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Events;

namespace AcmeTube.Application.Features.Videos
{
    public sealed class GetVideoDetails
    {
        public sealed record Query(string Id) : Query<QueryResult<Video>>;

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<Video>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPublisher _publisher;

            public QueryHandler(IUnitOfWork unitOfWork, IPublisher publisher)
            {
	            _unitOfWork = unitOfWork;
	            _publisher = publisher;
            }

            public async Task<QueryResult<Video>> Handle(Query query, CancellationToken cancellationToken)
            {
	            if (await _unitOfWork.VideoRepository.GetByIdAsync(query.Id, cancellationToken) is not { } video)
		            return QueryResult.NotFound<Video>();

	            await _publisher.Publish(new VideoViewedEvent(query.Id, query.Context.Identity), cancellationToken);
                    
				return QueryResult.Ok(video);
            }
        }
    }
}
