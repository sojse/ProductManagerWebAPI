using ProductManagerWebAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.DTO
{
    public class CreateCategoryDto
    {

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
