using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Domain
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public  int Id { get; set; }

        [MaxLength(25)]
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
