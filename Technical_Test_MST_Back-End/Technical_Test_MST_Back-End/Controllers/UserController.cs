using Microsoft.AspNetCore.Mvc;
using System.Text;
using Technical_Test_MST_Back_End.Models;
using Technical_Test_MST_Back_End.Models.Request;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Technical_Test_MST_Back_End.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Technical_Test_MST_Back_End.Services;
using Technical_Test_MST_Back_End.Helper;

namespace Technical_Test_MST_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbContextClass _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public UserController(DbContextClass context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest dataUser)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dataUser.Username))
                return BadRequest("Username already exists.");
            var user = new Users
            {
                Id = Guid.NewGuid(),
                Username = dataUser.Username,
                Password = HashPassword(dataUser.Password),
                Email = dataUser.Email,
                Fullname = dataUser.Fullname,
                Token = string.Empty,
                PasswordResetToken = string.Empty,
                CreatedDate = DateTime.Now
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // Generate a random key and IV. These should be securely stored and managed.
            byte[] key = new byte[32]; // AES-256 key
            byte[] iv = new byte[16];  // AES block size
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }

            EncryptionHelper encryptionHelper = new EncryptionHelper(key, iv);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null || user.Password != HashPassword(loginRequest.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);
            user.Token = token;
            user.ExpiredToken = DateTime.Now.AddDays(1);
            user.ModifiedDate = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            string dataFullnameToEncrypt = $"{user.Fullname}";
            string dataEmailToEncrypt = $"{user.Email}";
            string fullname = encryptionHelper.Encrypt(dataFullnameToEncrypt);
            string email = encryptionHelper.Encrypt(dataEmailToEncrypt);

            return Ok(new { token, fullname, email });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email && u.Username == request.Username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.PasswordResetToken = GeneratePasswordResetToken();
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);

            await _context.SaveChangesAsync();

            _emailService.SendResetTokenEmail(user.Email, user.PasswordResetToken);

            return Ok("Password reset token sent to email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PasswordResetToken == request.Token && u.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            user.Password = HashPassword(request.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            return Ok("Password has been reset.");
        }

        private string GeneratePasswordResetToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteToken = new byte[20];
                rng.GetBytes(byteToken);
                return Convert.ToBase64String(byteToken);
            }
        }

        private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
