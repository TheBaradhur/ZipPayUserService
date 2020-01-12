using System.Data;
using System.Data.SqlClient;

namespace Dal
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        public string ConnectionString { get; }

        public ConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}