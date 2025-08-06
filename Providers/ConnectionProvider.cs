using System.Data;
using Npgsql;

namespace rtgg_data_console.Providers;

public class ConnectionProvider
{
    private readonly string _connectionString;

    public ConnectionProvider()
    {
        _connectionString = Environment.GetEnvironmentVariable("FeDb_ConnectionString") ?? "";
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
