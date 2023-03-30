using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Data.Repositories;

public sealed class ChannelRepository : Repository<Channel>, IChannelRepository
{
    public ChannelRepository(MainContext context) : base(context) { }

    public async Task<PaginatedResult<Channel>> ListPaginatedByFilterAsync(ChannelFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
    {
	    var count = await DbSetAsNoTracking.CountAsync(cancellationToken: cancellationToken);
	    var result = await DbSetAsNoTracking
		    .Skip(pagingParameters.Offset)
		    .Take(pagingParameters.RecordsPerPage)
		    .ToListAsync(cancellationToken);
        
	    return new PaginatedResult<Channel>(result, count);

    }

   
    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(Channel channel, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}