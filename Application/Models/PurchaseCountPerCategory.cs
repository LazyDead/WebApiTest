namespace WebApi.Application.Models;

public class PurchaseCountPerCategory
{
    public required string Category { get; set; }
    public int PurchaseCount { get; set; }
}