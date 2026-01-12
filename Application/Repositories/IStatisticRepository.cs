using WebApi.API.DTO;

namespace WebApi.Application.Repositories;

public interface IStatisticRepository
{
    public Task<List<ClientDto>> GetClientsByBirthDate(DateTime birthDate);
    public Task<List<ClientLastBuyDto>>GetBuyersAfterDate(DateTime cutOffDate);
    public Task<List<CategoryBuysCountDto>> GetBuysPerCategory(int clientId);
}