using System.Data;
using AcmeTube.Data.Data;
using Npgsql;

namespace AcmeTube.Data.Factories
{
    public class AcmeDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public AcmeDatabaseConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        //public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
