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
    [HttpGet("GetClientsByBirthDate")]
    public IActionResult GetClientsByBirthDate([FromQuery(Name = "BirthDate")] DateOnly birthDate)
    {
        List<Client> clients;
        if(!_statisticService.TryGetClientsByBirthDate(birthDate,out clients))
            return NotFound();
        return Ok(StatisticMapper.ConvertToDto(clients));
    }
    [HttpGet("GetLastBuyers")]
    public IActionResult GetLastBuyers([FromQuery(Name = "DayRange")] int dayRange)
    {
        List<ClientLastBuy> clientsLastBuy;
        if(!_statisticService.TryGetLastBuys(dayRange,out clientsLastBuy))
            return NotFound();
        return Ok(StatisticMapper.ConvertToDto(clientsLastBuy));
    }
    [HttpGet("GetBuysPerCategory")]
    public IActionResult GetBuysPerCategory([FromQuery(Name = "ClientID")] int clientID)
    {
        List<PurchaseCountPerCategory> purchaseCountPerCategory;
        if(!_statisticService.TryGetBuysPerCategory(clientID,out purchaseCountPerCategory))
            return NotFound();
        return Ok(StatisticMapper.ConvertToDto(purchaseCountPerCategory));
    }
}