using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WGO_API.Models.UserModel;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WGO_API.Models.UpdateValue;

namespace WGO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> PostUser(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(userDTO.Email))
            {
                return BadRequest("User email must be provided");
            }

            var userExists = await _userManager.FindByEmailAsync(userDTO.Email);
            if (userExists != null)
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                UserName = userDTO.UserName ?? userDTO.Email,
                Email = userDTO.Email
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User created successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserDTO userDTO)
        {
            User? user = null;
            if (!string.IsNullOrEmpty(userDTO.Email))
            {
                user = await _userManager.FindByEmailAsync(userDTO.Email);
            }
            else if (!string.IsNullOrEmpty(userDTO.UserName))
            {
                user = await _userManager.FindByNameAsync(userDTO.UserName);
            }

            if (user == null)
            {
                return Unauthorized("Invalid username or email.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password.");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("Missing Jwt:Key")));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMonths(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [Authorize]
        [HttpPut("change_password")]
        public async Task<IActionResult> ChangePassword(UpdatePassword passwordChange)
        {
            if (string.IsNullOrEmpty(passwordChange.oldPassword))
            {
                return BadRequest("Old password must be provided.");
            }

            if (string.IsNullOrEmpty(passwordChange.newPassword))
            {
                return BadRequest("New password must be provided.");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, passwordChange.oldPassword, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password.");
            }

            await _userManager.ChangePasswordAsync(user, passwordChange.oldPassword, passwordChange.newPassword);

            return Ok("Password successfully updated.");
        }

        [Authorize]
        [HttpPut("change_username")]
        public async Task<IActionResult> ChangeUsername([FromQuery] String newUsername)
        {
            if (string.IsNullOrEmpty(newUsername))
            {
                return BadRequest("New username must be provided.");
            }
            if (newUsername.Length <= 4)
            {

            }
            
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            user.UserName = newUsername;
            await _userManager.UpdateNormalizedUserNameAsync(user);

            return Ok("Username successfully updated.");
        }

        [Authorize]
        [HttpPut("change_email")]
        public async Task<IActionResult> ChangeEmail([FromQuery] String newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                return BadRequest($"New Email must be provided.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = newEmail;
            await _userManager.UpdateNormalizedEmailAsync(user);

            return Ok("Email successfully updated.");
        }
    }
}
