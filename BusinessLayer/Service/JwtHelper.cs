using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// Helper class for generating JWT tokens.
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtHelper"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration containing JWT settings.</param>
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token for the authenticated user.
    /// </summary>
    /// <param name="username">The username of the authenticated user.</param>
    /// <returns>A JWT token string.</returns>
    public string GenerateToken(string username)
    {
        // Retrieve JWT settings from configuration
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username) // Adding username as a claim
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpiryInMinutes"])), // Token expiration time
            Issuer = jwtSettings["Issuer"], // Token issuer
            Audience = jwtSettings["Audience"], // Token audience
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Signing credentials using HMAC-SHA256
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}