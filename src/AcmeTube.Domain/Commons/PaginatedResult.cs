using System.Collections.Generic;

namespace AcmeTube.Domain.Commons
{
    public abstract record PaginatedResult(long TotalRecords);

    /// <summary>
    /// Represents a paged result of an operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record PaginatedResult<T>(IEnumerable<T> Results, long TotalRecords) : PaginatedResult(TotalRecords)
    {
	    /// <summary>
	    /// Create instance of PaginatedResult.
	    /// </summary>
	    public static PaginatedResult<T> Create(IEnumerable<T> data, int totalRecords) => new(data, totalRecords);
	}
}
