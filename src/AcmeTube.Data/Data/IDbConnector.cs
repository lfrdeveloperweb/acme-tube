using System;
using System.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AcmeTube.Data.Data
{
    /// <summary>
    /// Responsible for handling a connection, processing database transactions
    /// and ensuring the consistency of the transaction.
    /// </summary>
    public interface IDbConnector : IDisposable, IHealthCheck
    {
        /// <summary>
        /// Database connection, through which all transactions will be committed to.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Database transaction associated with the connection.
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Begins a transaction in the <see cref="Connection"/> source.
        /// It's useful to guarantee the consistency of all operations made within
        /// the transaction, enabling rolling back all scripts ran in case of error.
        /// </summary>
        IDbTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted);
    }
}
