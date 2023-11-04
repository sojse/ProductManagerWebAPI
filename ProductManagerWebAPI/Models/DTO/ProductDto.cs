namespace ProductManagerWebAPI.Models.DTO;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string StockKeepingUnit { get; set; }

    public string Description { get; set; }

    public string ImageURL { get; set; }

    public decimal Price { get; set; }
}