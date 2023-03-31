using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Accounts
{
    public sealed class GetUserDetails
    {
        public sealed record Query : Query<QueryResult<User>>;

        internal sealed class QueryHandler : IQueryHandler<Query, QueryResult<User>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public QueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

            public async Task<QueryResult<User>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _unitOfWork.UserRepository.GetByIdAsync(query.Context.Identity.Id, cancellationToken));
        }
    }
}
