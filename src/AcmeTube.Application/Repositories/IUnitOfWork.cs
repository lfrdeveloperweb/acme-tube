using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Repositories
{
    /// <summary>
    /// Unit of work of repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository to handle information about <see cref="User"/>
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Repository to handle information about <see cref="Channel"/>
        /// </summary>
        IChannelRepository ChannelRepository { get; }

        /// <summary>
        /// Repository to handle information about <see cref="Video"/>
        /// </summary>
        IVideoRepository VideoRepository { get; }

        /// <summary>
        /// Initiates a transaction under the connection held an instance of UnitOfWork.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level. Default: <see cref="IsolationLevel.ReadCommitted"/>.</param>
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task CommitTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
