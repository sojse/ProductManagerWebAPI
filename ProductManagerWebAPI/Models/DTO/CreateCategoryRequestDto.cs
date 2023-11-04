using ProductManagerWebAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Models.DTO
{
    public class CreateCategoryRequestDto
    {

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
