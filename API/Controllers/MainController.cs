using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Mappers;
using WebApi.Application.Services;
using WebApi.Application.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class MainController : ControllerBase
{
    private readonly StatisticService _statisticService;
    
    public MainController(StatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    [HttpGet("clients")]
    public async Task<IActionResult> GetClientsByBirthDate([FromQuery(Name = "birthday")]DateTime birthDate)
    {
        List<Client> clients = await _statisticService.GetClientsByBirthDate(birthDate);
        return Ok(StatisticMapper.ConvertToDto(clients));
    }
    [HttpGet("orders/recent")]
    public async Task<IActionResult> GetLastBuyers([FromQuery(Name = "days")] int dayRange)
    {
        List<ClientLastBuy> clientsLastBuy = await _statisticService.GetLastBuys(dayRange);
        return Ok(StatisticMapper.ConvertToDto(clientsLastBuy));
    }
    [HttpGet("clients/{clientId}/categories")]
    public async Task<IActionResult> GetBuysPerCategory(int clientId)
    {
        List<PurchaseCountPerCategory> purchaseCountPerCategory = await _statisticService.GetBuysPerCategory(clientId);
        return Ok(StatisticMapper.ConvertToDto(purchaseCountPerCategory));
    }
}