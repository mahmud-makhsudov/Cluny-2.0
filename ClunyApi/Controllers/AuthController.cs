using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.Security.Claims;
using System.Text;

namespace ClunyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] Credential credential)
        {
            if (string.IsNullOrWhiteSpace(credential?.EmailAddress))
            {
                ModelState.AddModelError("Invalid", "Email is required.");
                var pd = new ProblemDetails { Title = "Invalid request", Status = StatusCodes.Status400BadRequest };
                return BadRequest(pd);
            }

            if (string.IsNullOrWhiteSpace(credential?.Password))
            {
                ModelState.AddModelError("Invalid", "Password is required.");
                var pd = new ProblemDetails { Title = "Invalid request", Status = StatusCodes.Status400BadRequest };
                return BadRequest(pd);
            }

            var user = await userManager.FindByEmailAsync(credential.EmailAddress);
            if (user != null)
            {
                var passwordValid = await userManager.CheckPasswordAsync(user, credential.Password);
                if (!passwordValid)
                {
                    ModelState.AddModelError("Unauthorized", "Invalid email or password.");
                    var problemDetails = new ProblemDetails
                    {
                        Title = "Unauthorized",
                        Status = StatusCodes.Status401Unauthorized
                    };
                    return Unauthorized(problemDetails);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email ?? user.UserName ?? string.Empty)
                };

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt,
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint.");
            var problemDetailsNoUser = new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized
            };
            return Unauthorized(problemDetailsNoUser);
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"] ?? string.Empty)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = expiresAt,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
