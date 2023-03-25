using System.Collections.Generic;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Core.Queries
{
    public record ListQueryResult<TData>(IEnumerable<TData> Data, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
