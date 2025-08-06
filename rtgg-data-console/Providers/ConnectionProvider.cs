using System.Data;
using Npgsql;

namespace rtgg_data_console.Providers;

public class ConnectionProvider
{
    private readonly string _connectionString;

    public ConnectionProvider()
    {
        _connectionString = Environment.GetEnvironmentVariable("FeDb_ConnectionString") ?? "";
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ArgumentException($"{nameof(_connectionString)} is null or empty. Cannot proceed");
        }
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
