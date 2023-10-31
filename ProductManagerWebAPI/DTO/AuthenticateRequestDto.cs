using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.DTO;

public class AuthenticateRequestDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
