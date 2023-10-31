using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagerWebAPI.Data;
using ProductManagerWebAPI.Domain;
using ProductManagerWebAPI.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductManagerWebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IConfiguration config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        this.context = context;
        this.config = config;
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenDto> Authenticate(AuthenticateRequestDto request)
    {
        var user = context.Users
            .Include(user => user.Roles)
            .FirstOrDefault(user =>
            user.UserName == request.UserName
            && user.Password == request.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var tokenDto = new TokenDto
        {
            Token = GenerateToken(user)
        };

        return tokenDto;
    }

    private string GenerateToken(User user)
    {
        var signingKey = Convert.FromBase64String(config["Jwt:SigningSecret"]);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.FullName)
        };

        context.Entry(user)
            .Collection(user => user.Roles)
            .Load();

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature),

            Subject = new ClaimsIdentity(claims)
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(jwtSecurityToken);
    }

}
