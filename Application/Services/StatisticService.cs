using WebApi.Application.Models;
using WebApi.Application.Repository;

namespace WebApi.Application.Services;

public class StatisticService
{
    private readonly IStatisticRepository _statisticRepository;

    public StatisticService(IStatisticRepository statisticRepository) =>
        _statisticRepository = statisticRepository;

    public async Task<List<Client>> GetClientsByBirthDate(DateTime birthDate)
    {
        List<Client> clients = await _statisticRepository.GetClientsByBirthDate(birthDate);
        return clients;
    }

    public async Task<List<ClientLastBuy>> GetLastBuys(int dayRange)
    {
        List<Order> lastOrders = await _statisticRepository.GetOrdersInDayRange(dayRange);
        List<ClientLastBuy> clientLastBuys = new();

        for (int i = 0; i < lastOrders.Count; i++)
        {
            if (clientLastBuys.FindIndex(o => o.Client.Id == lastOrders[i].Client.Id) == -1)
            {
                clientLastBuys.Add(new()
                {
                    Client = lastOrders[i].Client,
                    LastBuyDate = lastOrders[i].PlaceDate
                });
                continue;
            }

            int index = clientLastBuys.FindIndex(o =>
                o.Client.Id == lastOrders[i].Client.Id && o.LastBuyDate < lastOrders[i].PlaceDate);
            if (index > -1)
                lastOrders[index].PlaceDate = lastOrders[i].PlaceDate;
        }

        return clientLastBuys;
    }

    public async Task<List<PurchaseCountPerCategory>> GetBuysPerCategory(int clientId)
    {
        List<PurchaseCountPerCategory> clientBuysPerCategory =
            await _statisticRepository.GetPurchaseCountPerCategoryByClientId(clientId);
        return clientBuysPerCategory;
    }
}