using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.DTOs;
using WebAPI.Models;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(AppDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Username == dto.UsernameOrEmail ||
                u.Email == dto.UsernameOrEmail);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = GenerateJwt(user);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role?.Name ?? "Unknown"
        };
    }

    private void DebugDecodedToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            Console.WriteLine("⚠️ Invalid JWT format.");
            return;
        }

        var jwt = handler.ReadJwtToken(token);

        Console.WriteLine("🔍 JWT Claims:");
        foreach (var claim in jwt.Claims)
        {
            Console.WriteLine($"  {claim.Type}: {claim.Value}");
        }

        Console.WriteLine("\n🔐 JWT Header:");
        foreach (var header in jwt.Header)
        {
            Console.WriteLine($"  {header.Key}: {header.Value}");
        }

        Console.WriteLine($"\n📅 Expires at: {jwt.ValidTo} UTC");
    }


    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );
        DebugDecodedToken(new JwtSecurityTokenHandler().WriteToken(token));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
