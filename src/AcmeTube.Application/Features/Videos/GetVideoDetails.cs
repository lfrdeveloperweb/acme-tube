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
            private readonly IVideoRepository _videoRepository;
            private readonly IPublisher _publisher;

            public QueryHandler(IVideoRepository videoRepository, IPublisher publisher)
            {
	            _videoRepository = videoRepository;
	            _publisher = publisher;
            }

            public async Task<QueryResult<Video>> Handle(Query query, CancellationToken cancellationToken)
            {
	            if (await _videoRepository.GetByIdAsync(query.Id, cancellationToken) is not { } video)
		            return QueryResult.NotFound<Video>();

	            await _publisher.Publish(new VideoViewedEvent(query.Id, query.Context.Identity), cancellationToken);
                    
				return QueryResult.Ok(video);
            }
        }
    }
}
