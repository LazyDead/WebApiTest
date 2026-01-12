namespace WebApi.Infrastructure.Persistence;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Article {get; set;}
    public decimal Price { get; set; }
}