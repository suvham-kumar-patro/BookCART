using BookCARTWebApi.Data;
using BookCARTWebApi.DTOs;
using BookCARTWebApi.Models;
using BookCARTWebApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookCARTWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;
        private readonly IEmailRepo _emailRepo;
        private readonly AppDbContext _context;

        public AuthController(IUserRepository userRepo, IConfiguration config, IEmailRepo emailRepo, AppDbContext context)
        {
            _userRepo = userRepo;
            _config = config;
            _hasher = new PasswordHasher<User>();
            _emailRepo = emailRepo;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _userRepo.UserExistsAsync(dto.Username))
                return BadRequest("Username already exists.");

            var confirmationToken = Guid.NewGuid();

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = "User",
                EmailConfirmed = false,
                ConfirmationToken = confirmationToken
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _userRepo.AddUserAsync(user);

            // Generate confirmation link
            var confirmLink = Url.Action(nameof(ConfirmEmail), "Auth",
                new { userId = user.Id, token = confirmationToken }, Request.Scheme);

            await _emailRepo.SendEmailAsync(dto.Email, "Confirm your email",
                $"<p>Click <a href='{confirmLink}'>here</a> to confirm your email.</p>");

            return Ok(new { message = "User registered successfully. Please confirm your email." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (!user.EmailConfirmed)
                return Unauthorized("Email not confirmed.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Invalid username or password.");

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(int userId, Guid token)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            if (user.EmailConfirmed)
                return BadRequest("Email already confirmed.");

            if (user.ConfirmationToken != token)
                return BadRequest("Invalid confirmation token.");

            user.EmailConfirmed = true;
            user.ConfirmationToken = Guid.Empty;

            await _context.SaveChangesAsync();

            return Ok("Email confirmed successfully.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var user = await _userRepo.GetByEmailAsync(request.Email);

            if (user != null)
            {
                user.PasswordResetToken = Guid.NewGuid();
                user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

                await _context.SaveChangesAsync();

                var resetLink = $"http://localhost:4200/reset-password?userId={user.Id}&token={user.PasswordResetToken}";

                await _emailRepo.SendEmailAsync(user.Email, "Password Reset Request",
                    $"<p>Click <a href='{resetLink}'>here</a> to reset your password. This link will expire in 15 minutes.</p>");
            }

            return Ok(new { message = "If an account with that email exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found.");

            if (user.PasswordResetToken != request.Token || user.ResetTokenExpires < DateTime.UtcNow)
                return BadRequest("Invalid or expired token.");

            user.PasswordHash = _hasher.HashPassword(user, request.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password has been reset successfully." });
        }
    }

}
