using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly HelloGreetingContext _context;

    public UserController(HelloGreetingContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registers a new user by hashing their password and storing their credentials in the database.
    /// </summary>
    /// <param name="user">The user details including email and password.</param>
    /// <returns>A success or failure message.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        if (await _context.Greetings.AnyAsync(u => u.Email == user.Email))
            return BadRequest("User already exists");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(user.Password, salt);

        var newUser = new HelloGreetingEntity { Email = user.Email, PasswordHash = hashedPassword, Salt = salt };
        _context.Greetings.Add(newUser);
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
        var existingUser = await _context.Greetings.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser == null)
            return Unauthorized("User not found");

        string hashedPassword = HashPassword(user.Password, existingUser.Salt);
        if (existingUser.PasswordHash != hashedPassword)
            return Unauthorized("Invalid credentials");

        return Ok("Login successful");
    }

    /// <summary>
    /// Generates a reset token for password recovery.
    /// </summary>
    /// <param name="model">The email address of the user requesting a password reset.</param>
    /// <returns>A reset token if the email exists in the system.</returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        var user = await _context.Greetings.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
            return BadRequest("User not found");

        string resetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.Email));
        return Ok(new { Message = "Reset token generated", Token = resetToken });
    }

    /// <summary>
    /// Resets the user's password using the provided reset token.
    /// </summary>
    /// <param name="model">The reset token and the new password.</param>
    /// <returns>A success or failure message.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        string email = Encoding.UTF8.GetString(Convert.FromBase64String(model.Token));
        var user = await _context.Greetings.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return BadRequest("Invalid token");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(model.NewPassword, salt);

        user.PasswordHash = hashedPassword;
        user.Salt = salt;
        await _context.SaveChangesAsync();

        return Ok("Password reset successfully");
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