using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Channels
{
    public sealed class GetChannelDetails
    {
	    public sealed record Query(string Id) : Query<QueryResult<Channel>>;

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<Channel>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public QueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<QueryResult<Channel>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _unitOfWork.ChannelRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
