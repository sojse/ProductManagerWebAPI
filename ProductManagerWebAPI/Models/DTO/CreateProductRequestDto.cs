using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Models.DTO;

public class CreateProductRequestDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string StockKeepingUnit { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageURL { get; set; }

    [Required]
    public decimal Price { get; set; }
}
