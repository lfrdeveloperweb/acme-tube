using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Repositories
{
	public interface IChannelRepository
	{
		/// <summary>
		/// Retrieves an <see cref="Channel"/> by its identifier.
		/// </summary>
		Task<Channel> GetByIdAsync(string id, CancellationToken cancellationToken);

		Task<PaginatedResult<Channel>> ListPaginatedByFilterAsync(ChannelFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken);

		Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

		Task CreateAsync(Channel channel, CancellationToken cancellationToken);

		Task UpdateAsync(Channel channel, CancellationToken cancellationToken);

		Task DeleteAsync(Channel channel, CancellationToken cancellationToken);
	}
}