using System.Collections.Generic;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Core.Queries
{
    public record PaginatedQueryResult<TData>(IEnumerable<TData> Data, PagingInfo PagingInfo, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
