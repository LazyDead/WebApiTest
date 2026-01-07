using System.Data;
using WebApi.Application.Models;
using WebApi.Application.Repository;
using WebApi.Infrastructure.SQLIte;


namespace WebApi.Infrastructure.Repository;

public class SQLiteStatisticRepository : IStatisticRepository
{
    private readonly SQLiteService _sqLiteService;

    public SQLiteStatisticRepository(SQLiteService sqLiteService) =>
        _sqLiteService = sqLiteService;

    public async Task<List<Client>> GetClientsByBirthDate(DateOnly birthDate)
    {
        List<Client> clients = new List<Client>();
        string command = $"SELECT Id,FullName FROM Clients WHERE BirthDate = '{birthDate.ToString("yyyy-MM-dd")}';";
        var dataTable = await _sqLiteService.GetDataTableAsync(command);
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
                         $"WHERE PlaceDate  >= '{CutOffDate.ToString("yyyy-MM-dd")}'";
        var dataTable = await _sqLiteService.GetDataTableAsync(command);
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
        string command = $"SELECT Id FROM Orders Where ClientId = {clientId} ";
        var dataTable = await _sqLiteService.GetDataTableAsync(command);
        if (dataTable.Rows.Count == 0)
            return purchaseCountPerCategory;
        string ordersString = "";
        foreach (DataRow row in dataTable.Rows)
            ordersString += $"{row["Id"]},";

        command = "SELECT Products.Category, Quantity " +
                  "FROM Orders_Positions " +
                  "JOIN Products ON Products.Id = Orders_Positions.ProductId " +
                  $"WHERE OrderId IN({ordersString.TrimEnd(',')}) ";
        dataTable = await _sqLiteService.GetDataTableAsync(command);

        foreach (DataRow row in dataTable.Rows)
        {
            int index = purchaseCountPerCategory.FindIndex(o => o.Category == row["Category"].ToString());
            if (index == -1)
            {
                purchaseCountPerCategory.Add(new()
                {
                    Category = row["Category"].ToString() ?? "",
                    PurchaseCount = Convert.ToInt32(row["Quantity"])
                });
            }
            else
                purchaseCountPerCategory[index].PurchaseCount += Convert.ToInt32(row["Quantity"]);
        }

        return purchaseCountPerCategory;
    }
}