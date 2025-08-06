using Dapper;
using rtgg_data_console.Providers;

namespace rtgg_data_console;

public class Repository(ConnectionProvider connectionProvider)
{
    public async Task<T> GetAsync<T>(string query, object param) where T : new()
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            var data = await connection.QuerySingleOrDefaultAsync<T>(query, param);
            return data ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new();
        }
    }

    public async Task<bool> ExistsAsync(string query, object param)
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            var data = await connection.QueryFirstOrDefaultAsync(query, param);
            return data is null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<T?> InsertAsync<T>(string query, object param)
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            return await connection.QuerySingleAsync<T>(query, param);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
    }

    public async Task<bool> UpdateAsync(string query, object param)
    {
        using var connection = connectionProvider.GetConnection();
        try
        {
            var rowCount = await connection.ExecuteAsync(query, param);
            return rowCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
