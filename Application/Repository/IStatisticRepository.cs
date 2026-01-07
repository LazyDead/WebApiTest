using WebApi.Application.Models;

namespace WebApi.Application.Repository;

public interface IStatisticRepository
{
    public Task<List<Client>> GetClientsByBirthDate(DateOnly birthDate);
    public Task<List<Order>> GetOrdersInDayRange(int dayRange);
    public Task<List<PurchaseCountPerCategory>> GetPurchaseCountPerCategoryByClientId(int clientId);
}