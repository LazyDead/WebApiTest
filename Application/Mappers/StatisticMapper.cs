using WebApi.API.DTO;
using WebApi.Application.Models;

namespace WebApi.Application.Mappers;

public static class StatisticMapper
{
    public static ClientsDto ConvertToDto(List<Client> clients)
        => new ClientsDto(){Clients = clients.Select(ConvertToDto).ToList()};

    public static ClientDto ConvertToDto(Client client)
    {
        return new ClientDto()
        {
            Id = client.Id,
            FullName = client.FullName
        };
    }

    public static PurchaseCountPerCategoryDto ConvertToDto(List<PurchaseCountPerCategory> purchaseCountPerCategory)
    {
        PurchaseCountPerCategoryDto result = new(){Categories = new()};
        purchaseCountPerCategory.ForEach(c =>
        {
            result.Categories.Add(new()
            {
                Name = c.Category,
                PurchaseCount = c.PurchaseCount
            });
        });
        return result;
    }

    public static LastBuysDto ConvertToDto(List<ClientLastBuy> clientLastBuys)
    {
        LastBuysDto result = new(){LastBuys = new()};
        clientLastBuys.ForEach(c =>
        {
            result.LastBuys.Add(new()
            {
                Client = ConvertToDto(c.Client),
                LastBuyDate = c.LastBuyDate.ToString("yyyy-MM-dd HH:mm:ss"),
            });
        });
        return result;
    }
}