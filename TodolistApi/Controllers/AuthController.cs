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
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
            {
                _logger.LogInformation("User does not exists");
                return BadRequest("user does not exists");
            }

            if (await _userManager.CheckPasswordAsync(user, password) is false)
            {
                _logger.LogInformation("Password incorrect");
                return BadRequest("Password incorrect");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            //var claims2 = roles.Select(role => new Claim(ClaimTypes.Role, role));
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

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

