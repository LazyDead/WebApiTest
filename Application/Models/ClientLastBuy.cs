namespace WebApi.Application.Models;

public class ClientLastBuy
{
    public required Client Client {get; set;}
    public DateTime LastBuyDate {get; set;}
}