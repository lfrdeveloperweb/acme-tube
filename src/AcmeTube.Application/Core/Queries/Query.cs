using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using MediatR;

namespace AcmeTube.Application.Core.Queries
{
    public interface IQuery<out TQueryResult> : IRequest<TQueryResult>
        where TQueryResult : IQueryResult { }

    public record Query<TQueryResult>(bool BypassValidation = false) : IQuery<TQueryResult>, IOperationContextSetup
		where TQueryResult : IQueryResult
    {
	    public string GetMembershipId() => Context?.Identity?.Id;

		public OperationContext Context { get; private set; }

	    void IOperationContextSetup.Setup(OperationContext context) => Context = context;
	}

    public record PaginatedQuery<TQueryResult, TData>(PagingParameters PagingParameters) : Query<TQueryResult>
        where TQueryResult : PaginatedQueryResult<TData>;
}