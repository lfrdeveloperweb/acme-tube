using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Channels
{
    public sealed class GetChannelDetails
    {
        public sealed record Query(string Id, OperationContext OperationContext) : Query<QueryResult<Channel>>(OperationContext);

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
