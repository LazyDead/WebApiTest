using Microsoft.AspNetCore.Mvc;
using WebApi.API.DTO;
using WebApi.Application.Services;

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
    public async Task<IActionResult> GetClientsByBirthDate([FromQuery(Name = "birthday")] DateTime birthDate)
    {
        List<ClientDto> clients = await _statisticService.GetClientsByBirthDate(birthDate);
        return Ok(clients);
    }

    [HttpGet("orders/recent")]
    public async Task<IActionResult> GetLastBuyers([FromQuery(Name = "days")] int dayRange)
    {
        if (dayRange <= 0)
            return BadRequest();
        List<ClientLastBuyDto> clientsLastBuy = await _statisticService.GetLastBuyers(dayRange);
        return Ok(clientsLastBuy);
    }

    [HttpGet("clients/{clientId}/categories")]
    public async Task<IActionResult> GetBuysPerCategory(int clientId)
    {
        if (clientId <= 0)
            return BadRequest();
        List<CategoryBuysCountDto> buysPerCategory = await _statisticService.GetBuysPerCategory(clientId);
        return Ok(buysPerCategory);
    }
}