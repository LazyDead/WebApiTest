namespace WebApi.Application.Models;

public class Order
{
    public int Id {get; set;}
    public required Client Client {get; set;}
    public DateTime PlaceDate {get; set;}
}