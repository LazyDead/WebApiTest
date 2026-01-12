namespace WebApi.Infrastructure.Persistence;

public class OrderEntity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;
    public DateTime PlaceDate { get; set; }
    public decimal TotalValue { get; set; }
    public List<OrderPositionEntity> Positions { get; set; } = new();
}