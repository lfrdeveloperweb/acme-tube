﻿using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Accounts
{
    public sealed class GetUserDetails
    {
        public sealed record Query(string Id, OperationContext OperationContext) : Query<QueryResult<User>>(OperationContext);

        internal sealed class QueryHandler : IQueryHandler<Query, QueryResult<User>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public QueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

            public async Task<QueryResult<User>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _unitOfWork.UserRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
