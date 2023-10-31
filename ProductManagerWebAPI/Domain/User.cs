using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Domain;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string UserName { get; set; }

    [MaxLength(50)]
    public required string Password { get; set; }
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
