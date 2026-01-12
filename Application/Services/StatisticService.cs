using WebApi.API.DTO;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services;

public class StatisticService
{
    private readonly IStatisticRepository _statisticRepository;

    public StatisticService(IStatisticRepository statisticRepository) =>
        _statisticRepository = statisticRepository;

    public async Task<List<ClientDto>> GetClientsByBirthDate(DateTime birthDate)
    {
        
        List<ClientDto> clients = await _statisticRepository.GetClientsByBirthDate(birthDate);

        return clients;
    }

    public async Task<List<ClientLastBuyDto>> GetLastBuyers(int dayRange)
    {
        DateTime cutOffDate = DateTime.Today.AddDays(-dayRange);
        
        List<ClientLastBuyDto> clientLastBuys = await _statisticRepository.GetBuyersAfterDate(cutOffDate);
        
        return clientLastBuys;
    }

    public async Task<List<CategoryBuysCountDto>> GetBuysPerCategory(int clientId)
    {
        List<CategoryBuysCountDto> clientBuysPerCategory =
            await _statisticRepository.GetBuysPerCategory(clientId);
        
        return clientBuysPerCategory;
    }
}