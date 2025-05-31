using Bookealo.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookealoWebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email == "andresberros@gmail.com" && request.Password == "Hot34*jl")
        {
            var user = GetUserByEmail(request.Email);
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token, user });
            }
        }

        return Unauthorized("Invalid username or password");
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        foreach (var permission in user.Permissions)
        {
            claims.Add(new Claim("permission", permission.ToString()));
        }

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User GetUserByEmail(string email)
    {
        if (email.Equals("andresberros@gmail.com", StringComparison.InvariantCultureIgnoreCase))
        {
            return new User()
            {
                ID = 4,
                Name = "Charlie",
                Email = "andresberros@gmail.com",
                Permissions = [Permission.BookealoAdmin]
            };
        }

        return new User();
    }
}
