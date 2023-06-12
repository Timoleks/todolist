using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using TodolistApi.Infrastructure.IdentityModels;

namespace TodolistApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username == "username" && password == "password")
            {
                var claims = new Claim[]
                {
                    new(JwtRegisteredClaimNames.Sub, "f8f7d5e9-21df-4b01-82c7-23decd1a57a8"),
                    new(ClaimTypes.Role, "ADMIN"),
                    new("MainCharacter", "Blight"),
                    new("Name", "Yevhenii"),
                    new(JwtRegisteredClaimNames.Email, "example@example.com")
                };

                var secret = "SomeSecresadfsdafasdfasdfasdfasddfasdgasdfgadgsdafsadfasdfasdt";
                var issuer = "KanyaFieldsIdentityProvider";
                var audience = "KanyaFieldsUsers";

                var secretBytes = Encoding.UTF8.GetBytes(secret);
                var key = new SymmetricSecurityKey(secretBytes);
                var algorithm = SecurityAlgorithms.HmacSha256;

                var signingCredentials = new SigningCredentials(key, algorithm);

                var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials);

                var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenJson);
            }

            return Unauthorized();
        }


        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new User(username);
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded is false)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddClaimAsync(user, new Claim("MainCharacter", "Blight"));
            await _userManager.AddToRoleAsync(user, "ADMIN");
            return Ok();
        }
    }
}

