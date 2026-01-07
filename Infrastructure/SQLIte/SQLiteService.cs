using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace WebApi.Infrastructure.SQLIte;

public class SQLiteService
{
    private readonly string _connectionString;

    public SQLiteService(string connectionString) => _connectionString = connectionString;
    public async Task<DataTable> GetDataTableAsync(string query)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            SqliteCommand command = new(query, connection);
            await connection.OpenAsync();

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
        }
    }
}