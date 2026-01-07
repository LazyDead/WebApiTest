using WebApi.Application.Models;
using WebApi.Application.Repository;

namespace WebApi.Application.Services;

public class StatisticService
{
    private readonly IStatisticRepository _statisticRepository;

    public StatisticService(IStatisticRepository statisticRepository) =>
        _statisticRepository = statisticRepository;

    public bool TryGetClientsByBirthDate(DateOnly birthDate, out List<Client> clients)
    {
        clients = _statisticRepository.GetClientsByBirthDate(birthDate).Result;
        return clients.Count > 0;
    }

    public bool TryGetLastBuys(int dayRange, out List<ClientLastBuy> clientLastBuys)
    {
        List<Order> lastOrders = _statisticRepository.GetOrdersInDayRange(dayRange).Result;
        clientLastBuys = new();
        if (lastOrders.Count == 0)
        {
            clientLastBuys = new();
            return false;
        }

        for(int i = 0; i < lastOrders.Count; i++)
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

            int index = clientLastBuys.FindIndex(o => o.Client.Id == lastOrders[i].Client.Id && o.LastBuyDate < lastOrders[i].PlaceDate);
            if (index > -1)
                lastOrders[index].PlaceDate = lastOrders[i].PlaceDate;
        }

        return true;
    }

    public bool TryGetBuysPerCategory(int clientId, out List<PurchaseCountPerCategory> clientBuysPerCategory)
    {
        clientBuysPerCategory =
            _statisticRepository.GetPurchaseCountPerCategoryByClientId(clientId).Result;
        if (clientBuysPerCategory.Count == 0)
            return false;
        return true;
    }
}