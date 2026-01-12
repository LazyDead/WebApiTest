using System.Data;
using Microsoft.Data.Sqlite;
using WebApi.Application.Models;
using WebApi.Application.Repository;
using WebApi.Infrastructure.SQLIte;


namespace WebApi.Infrastructure.Repository;

public class SQLiteStatisticRepository : IStatisticRepository
{
    private readonly SQLiteService _sqLiteService;

    public SQLiteStatisticRepository(SQLiteService sqLiteService) =>
        _sqLiteService = sqLiteService;

    public async Task<List<Client>> GetClientsByBirthDate(DateTime birthDate)
    {
        List<Client> clients = new List<Client>();
        string command = "SELECT Id,FullName " +
                         "FROM Clients " +
                         "WHERE " +
                         "strftime('%d', BirthDate) == @birthDay AND " +
                         "strftime('%m', BirthDate) == @birthMonth";
        var parameters = new[]
        {
            new SqliteParameter("@birthDay", birthDate.ToString("dd")),
            new SqliteParameter("@birthMonth", birthDate.ToString("MM"))
        };
        var dataTable = await _sqLiteService.GetDataTableAsync(command,parameters);
        if (dataTable.Rows.Count == 0)
            return clients;
        foreach (DataRow row in dataTable.Rows)
        {
            clients.Add(new()
            {
                Id = Convert.ToInt32(row["Id"]),
                FullName = row["FullName"].ToString() ?? ""
            });
        }

        return clients;
    }

    public async Task<List<Order>> GetOrdersInDayRange(int dayRange)
    {
        DateTime CutOffDate = DateTime.Today.AddDays(-dayRange);
        List<Order> orders = new List<Order>();
        string command = "SELECT Orders.Id,ClientId,Clients.FullName,PlaceDate " +
                         "FROM Orders  " +
                         "JOIN Clients ON Clients.Id = ClientId " +
                         "WHERE PlaceDate  >= @cutOffDate";
        var parameters = new[]
        {
            new SqliteParameter("@cutOffDate", CutOffDate.ToString("yyyy-MM-dd"))
        };
        var dataTable = await _sqLiteService.GetDataTableAsync(command,parameters);
        if (dataTable.Rows.Count == 0)
            return orders;
        foreach (DataRow row in dataTable.Rows)
        {
            orders.Add(new()
            {
                Id = Convert.ToInt32(row["Id"]),
                Client = new Client()
                {
                    Id = Convert.ToInt32(row["ClientId"]),
                    FullName = row["FullName"].ToString() ?? ""
                },
                PlaceDate = Convert.ToDateTime(row["PlaceDate"])
            });
        }

        return orders;
    }

    public async Task<List<PurchaseCountPerCategory>> GetPurchaseCountPerCategoryByClientId(int clientId)
    {
        List<PurchaseCountPerCategory> purchaseCountPerCategory = new();
        string command = "SELECT Category, SUM(Orders_Positions.Quantity) as 'Quantity'" +
                  "FROM Orders " +
                  "JOIN Orders_Positions ON Orders_Positions.OrderId = Orders.Id " +
                  "JOIN Products ON Products.Id = Orders_Positions.ProductId " +
                  "WHERE Orders.ClientId == @clientId " +
                  "GROUP BY Category";
        var parameters = new[]
        {
            new SqliteParameter("@clientId", clientId)
        };
        DataTable dataTable = await _sqLiteService.GetDataTableAsync(command, parameters);

        foreach (DataRow row in dataTable.Rows)
        {
            purchaseCountPerCategory.Add(new()
            {
                Category = row["Category"].ToString() ?? "",
                PurchaseCount = Convert.ToInt32(row["Quantity"])
            });
        }
        
        return purchaseCountPerCategory;
    }
}