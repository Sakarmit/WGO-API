using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models.CommentModel;
using WGO_API.Models.MarkerModel;
using WGO_API.Models.UserModel;

namespace WGO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return UserToDTO(user);
        }

        [HttpGet("{id}/Verified")]
        public async Task<ActionResult<bool>> GetVerified(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user.EmailConfirmed;
        }
        
        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(int id, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != oldPassword)
            {
                return Unauthorized();
            }
            user.Password = newPassword;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserDTO userDTO)
        {
            var user = new User
            {
                Id  = userDTO.Id,
                Email = userDTO.Email,
                Password = userDTO.Password,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                UserToDTO(user));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private UserDTO UserToDTO(User user)
        => new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password
        };
    }
}
