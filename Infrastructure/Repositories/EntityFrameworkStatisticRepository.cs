using WebApi.API.DTO;
using WebApi.Application.Repositories;
using WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Repositories;

public class EntityFrameworkStatisticRepository : IStatisticRepository
{
    private readonly AppDbContext _db;
    
    public EntityFrameworkStatisticRepository(AppDbContext db) => _db = db;

    public async Task<List<ClientDto>> GetClientsByBirthDate(DateTime birthDate)
    {
        return await _db.Clients
            .AsNoTracking()
            .Where(c =>
                c.BirthDate.Day == birthDate.Day &&
                c.BirthDate.Month == birthDate.Month)
            .Select(c => new ClientDto
            {
                Id = c.Id,
                FullName = c.FullName
            })
            .ToListAsync();
    }

    public async Task<List<ClientLastBuyDto>> GetBuyersAfterDate(DateTime cutOffDate)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.PlaceDate >= cutOffDate)
            .GroupBy(o => o.Client)
            .Select(g => new ClientLastBuyDto()
            {
                Client = new()
                {
                    Id = g.Key.Id,
                    FullName = g.Key.FullName
                },
                LastBuyDate = g.Max(x => x.PlaceDate).ToString("dd-MM-yyyy HH:mm:ss")
            }).ToListAsync();
    }

    public async Task<List<CategoryBuysCountDto>> GetBuysPerCategory(int clientId)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.ClientId == clientId)
            .SelectMany(o => o.Positions)
            .Select(p => new
            {
                Category = p.Product.Category,
                Quantity = p.Quantity
            })
            .GroupBy(x => x.Category)
            .Select(g => new CategoryBuysCountDto
            {
                Name = g.Key,
                PurchaseCount = g.Sum(x => x.Quantity)
            }).ToListAsync();
    }
}