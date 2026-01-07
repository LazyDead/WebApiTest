namespace WebApi.API.DTO;

public class ClientsDto
{
    public required List<ClientDto> Clients {get; set;}
}
public class ClientDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
}