namespace WebApi.API.DTO;

public class ClientLastBuyDto
{
    public required ClientDto Client { get; set; }
    public required string LastBuyDate { get; set; }
}