using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProductManagerWebAPI.Domain;

[Index(nameof(StockKeepingUnit), IsUnique = true)]
public class Product
{

    private string imageUrl;

    public int Id { get; set; }

    [MaxLength(25)]
    public required string Name { get; set; }

    [Column(TypeName = "nchar(6)")]
    public required string StockKeepingUnit { get; set; }

    [MaxLength(200)]
    public required string Description { get; set; }

    [MaxLength(100)]
    public required string ImageURL
    {
        get => imageUrl;
        set
        {
            // regex that matches https or ftp, doesnt contain white spaces or special characters and ends 
            // with a dot and some of the following endings.
            string imageURLPattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*\.(jpg|jpeg|png|gif|bmp)$";

            Regex regex = new Regex(imageURLPattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(value))
            {
                imageUrl = value;
            }
            else
            {
                throw new ArgumentException("Invalid URL");
            }
        }
    }

    public required decimal Price { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
}

