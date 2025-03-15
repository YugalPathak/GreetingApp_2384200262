using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using RepositoryLayer.Entity;
using System;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly HelloGreetingContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="context">The database context for user-related operations.</param>
    /// <param name="jwtHelper">Helper class for JWT token generation.</param>

    public UserController(HelloGreetingContext context, JwtHelper jwtHelper, EmailService emailService, IConfiguration configuration)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _emailService = emailService;
        _configuration = configuration;
    }

    /// <summary>
    /// Authenticates the user and generates a JWT token.
    /// </summary>
    /// <param name="model">The login credentials (username & password).</param>
    /// <returns>Returns a JWT token if authentication is successful; otherwise, returns Unauthorized.</returns>
    [HttpPost("login/Jwt")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Username == "admin" && model.Password == "password123") // Replace with actual user authentication logic
        {
            var token = _jwtHelper.GenerateResetToken(model.Username);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }

    /// <summary>
    /// Retrieves protected data. This endpoint is accessible only to authenticated users.
    /// </summary>
    /// <returns>Returns a success message if the user is authorized.</returns>
    [Authorize]
    [HttpGet("secure-data")]
    public IActionResult SecureData()
    {
        return Ok("You are authenticated! This is protected data.");
    }

    /// <summary>
    /// Registers a new user by hashing their password and storing their credentials in the database.
    /// </summary>
    /// <param name="user">The user details including email and password.</param>
    /// <returns>A success or failure message.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] HelloGreetingEntity user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return BadRequest("User already exists");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(user.Password, salt);

        var newUser = new HelloGreetingEntity { Email = user.Email, Password = hashedPassword, Salt = salt };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    /// <summary>
    /// Authenticates a user by verifying their email and hashed password.
    /// </summary>
    /// <param name="user">The user login details including email and password.</param>
    /// <returns>A success message if credentials are valid, otherwise an error message.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserModel user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser == null)
            return Unauthorized("User not found");

        string hashedPassword = HashPassword(user.Password, existingUser.Salt);
        if (existingUser.Password != hashedPassword)
            return Unauthorized("Invalid credentials");

        return Ok("Login successful");
    }

    /// <summary>
    /// Handles forgot password functionality.
    /// Generates a password reset token and sends it to the user's email.
    /// </summary>
    /// <param name="model">Contains the user's email address.</param>
    /// <returns>Returns a message indicating whether the reset token was sent successfully.</returns>

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            return BadRequest("User with this email does not exist.");
        }

        var resetToken = _jwtHelper.GenerateResetToken(user.Email);

        var emailBody = $"Your password reset token is: {resetToken}\n\nUse this token in the reset password API.";

        await _emailService.SendEmailAsync(user.Email, "Password Reset Request", emailBody);

        // Return the reset token in the response (useful for testing)
        return Ok(new { message = "Password reset token sent to your email.", token = resetToken });
    }

    /// <summary>
    /// Handles password reset functionality.
    /// Validates the reset token, updates the user's password, and saves it securely.
    /// </summary>
    /// <param name="model">Contains the reset token and the new password.</param>
    /// <returns>Returns a message indicating whether the password reset was successful.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            var principal = tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null) return Unauthorized("Invalid token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim.Value);
            if (user == null) return BadRequest("User not found.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword); // Hash new password
            await _context.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }
        catch
        {
            return BadRequest("Invalid or expired token.");
        }
    }


    /// <summary>
    /// Generates a random cryptographic salt.
    /// </summary>
    /// <returns>A base64-encoded salt string.</returns>
    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    /// Hashes a password using SHA256 and a salt.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <param name="salt">The salt to append before hashing.</param>
    /// <returns>A base64-encoded hashed password.</returns>
    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(sha256.ComputeHash(saltedPassword));
        }
    }
}