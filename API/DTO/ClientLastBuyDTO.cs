namespace WebApi.API.DTO;

public class LastBuysDto
{
    public required List<ClientLastBuyDto> LastBuys {get; set;}
}
public class ClientLastBuyDto
{
    public required ClientDto Client { get; set; }
    public required string LastBuyDate { get; set; }
}