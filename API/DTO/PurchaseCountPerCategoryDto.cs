namespace WebApi.API.DTO;

public class PurchaseCountPerCategoryDto
{
    public required List<CategoryDto> Categories {get; set;}
}

public class CategoryDto
{
    public required string Name { get; set; }
    public int PurchaseCount { get; set; }
}