﻿namespace AcmeTube.Data
{
    /*

    /// <summary>
    /// Unit of work of repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Flag to identify if Dispose has already been called.
        /// </summary>
        private bool _disposed;

        private IUserRepository _userRepository;
        private IProjectRepository _projectRepository;
        private ITodoRepository _todoRepository;

        public UnitOfWork(IDbConnector dbConnector)
        {
            DbConnector = dbConnector;
        }

        /// <summary>
        /// Instance of <see cref="IDbConnector"/>.
        /// </summary>
        public IDbConnector DbConnector { get; }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(DbConnector);

        public IProjectRepository ProjectRepository => _projectRepository ??= new ProjectRepository(DbConnector);

        public ITodoRepository TodoRepository => _todoRepository ??= new TodoRepository(DbConnector);

        /// <summary>
        /// Initiates a transaction under the connection held an instance of <see cref="UnitOfWork"/>.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level. Default: <see cref="IsolationLevel.ReadUncommitted"/>.</param>
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted) => 
            DbConnector.BeginTransaction(isolationLevel);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (DbConnector?.Transaction?.Connection?.State == ConnectionState.Open)
            {
                DbConnector.Transaction.Commit();
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (DbConnector?.Transaction?.Connection?.State == ConnectionState.Open)
            {
                DbConnector.Transaction.Rollback();
            }
        }

        /// <summary>
        /// Dispose the resources used along the lifecycle of this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of the Dispose pattern.
        /// </summary>
        /// <param name="disposing">Flag to indicate a disposing running in this instance.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                DbConnector.Transaction?.Dispose();
                DbConnector.Connection?.Dispose();
            }

            // Unmanaged objects were disposed.
            _disposed = true;
        }
    }

    */
}
