using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Videos
{
    public sealed class GetVideoDetails
    {
        public sealed record Query(string Id) : Query<QueryResult<Video>>;

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<Video>>
        {
            private readonly IVideoRepository _videoRepository;

            public QueryHandler(IVideoRepository videoRepository) => _videoRepository = videoRepository;

            public async Task<QueryResult<Video>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _videoRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
