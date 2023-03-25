using System.Data;

namespace AcmeTube.Data.Data
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}