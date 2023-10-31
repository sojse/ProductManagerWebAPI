using ProductManagerWebAPI.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductManagerWebAPI.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
