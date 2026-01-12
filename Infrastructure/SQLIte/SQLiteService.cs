using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace WebApi.Infrastructure.SQLIte;

public class SqLiteService
{
    private readonly string _connectionString;

    public SqLiteService(string connectionString) => _connectionString = connectionString;

    public async Task<DataTable> GetDataTableAsync(string query, IEnumerable<SqliteParameter>? parameters = null)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                SqliteCommand command = new(query, connection);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                
                await connection.OpenAsync();

                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return new DataTable();
        }
    }
}