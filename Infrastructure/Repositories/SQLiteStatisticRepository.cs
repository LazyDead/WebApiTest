using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using WebApi.API.DTO;
using WebApi.Application.Repositories;

namespace WebApi.Infrastructure.Repositories;

public class SqLiteStatisticRepository : IStatisticRepository
{
    private readonly string _connectionString;

    public SqLiteStatisticRepository(string connectionString) => _connectionString = connectionString;

    public async Task<List<ClientDto>> GetClientsByBirthDate(DateTime birthDate)
    {
        List<ClientDto> clients = new();

        const string sql = "SELECT Id,FullName " +
                           "FROM Clients " +
                           "WHERE strftime('%m-%d', BirthDate) = @monthDay";
        var parameters = new[]
        {
            new SqliteParameter("@monthDay", birthDate.ToString("MM-dd"))
        };

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using SqliteCommand command = new(sql, connection);
        command.Parameters.AddRange(parameters);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            clients.Add(new()
            {
                Id = reader.GetInt32("Id"),
                FullName = reader.GetString("FullName")
            });
        }

        return clients;
    }

    public async Task<List<ClientLastBuyDto>> GetBuyersAfterDate(DateTime cutOffDate)
    {
        List<ClientLastBuyDto> lastBuys = new();

        string sql = "SELECT Orders.Id,ClientId,Clients.FullName,MAX(PlaceDate) as 'LastPlaceDate' " +
                     "FROM Orders  " +
                     "JOIN Clients ON Clients.Id = ClientId " +
                     "WHERE PlaceDate  >= @cutOffDate " +
                     "GROUP BY Clients.Id ";
        var parameters = new[]
        {
            new SqliteParameter("@cutOffDate", cutOffDate.ToString("yyyy-MM-dd"))
        };

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lastBuys.Add(new()
            {
                Client = new ClientDto()
                {
                    Id = reader.GetInt32("ClientId"),
                    FullName = reader.GetString("FullName")
                },
                LastBuyDate = reader.GetDateTime("LastPlaceDate").ToString("dd-MM-yyyy HH:mm:ss")
            });
        }

        return lastBuys;
    }

    public async Task<List<CategoryBuysCountDto>> GetBuysPerCategory(int clientId)
    {
        List<CategoryBuysCountDto> categories = new();

        string sql = "SELECT Category, SUM(Orders_Positions.Quantity) as 'Quantity'" +
                     "FROM Orders " +
                     "JOIN Orders_Positions ON Orders_Positions.OrderId = Orders.Id " +
                     "JOIN Products ON Products.Id = Orders_Positions.ProductId " +
                     "WHERE Orders.ClientId == @clientId " +
                     "GROUP BY Category";
        var parameters = new[]
        {
            new SqliteParameter("@clientId", clientId)
        };

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new()
            {
                Name = reader.GetString("Category"),
                PurchaseCount = reader.GetInt32("Quantity")
            });
        }

        return categories;
    }
}