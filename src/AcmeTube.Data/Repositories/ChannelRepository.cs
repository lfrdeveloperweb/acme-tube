using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;

namespace AcmeTube.Data.Repositories;

public sealed class ChannelRepository : Repository, IChannelRepository
{
    public ChannelRepository(MainContext context) : base(context) { }

    public Task<Channel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<PaginatedResult<Channel>> ListPaginatedByFilterAsync(ChannelFilter filter, PagingParameters pagingParameters,
        CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task CreateAsync(Channel channel, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(Channel todo, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}