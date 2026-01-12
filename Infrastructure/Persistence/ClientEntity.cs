namespace WebApi.Infrastructure.Persistence;

public class ClientEntity
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateTime SignInDate { get; set; }
    public List<OrderEntity> Orders { get; set; } = new();
}