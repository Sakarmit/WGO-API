using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models.CommentModel;
using WGO_API.Models.MarkerModel;
using WGO_API.Models.ReportModel;
using WGO_API.Models.UserModel;

namespace WGO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _commentContext;
        private readonly UserManager<User> _userManager;

        public CommentsController(CommentContext context, UserManager<User> userManager)
        {
            _commentContext = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CommentDTO commentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            Comment comment = new Comment 
            { 
                Id = commentDTO.Id,
                MarkerId = commentDTO.MarkerId,
                UserId = user.Id,
                UserName = user.UserName ?? "UserName not found",
                DateTime = commentDTO.DateTime,
                Message = commentDTO.Message
            };

            _commentContext.Comments.Add(comment);
            await _commentContext.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutComment(CommentDTO commentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var comment = await _commentContext.Comments.FindAsync(commentDTO.Id);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if (comment.UserId != user.Id)
            {
                return Unauthorized("User unauthorized to make changes to this comment.");
            }

            comment.Message = commentDTO.Message;

            try
            {
                await _commentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(commentDTO.Id))
            {
                return NotFound("Comment not found");
            }

            return Ok("Comment Updated Successfully");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForMarker(int id)
        {
            if (_commentContext.Comments == null)
            {
                return StatusCode(500);
            }
            return await _commentContext.Comments
              .Where(x => x.MarkerId == id)
              .Select(x => CommentToDTO(x))
              .ToListAsync();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_commentContext.Comments == null)
            {
                return StatusCode(500);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var comment = await _commentContext.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if (comment.UserId != user.Id)
            {
                return Unauthorized("User unauthorized to make changes to this comment.");
            }

            _commentContext.Comments.Remove(comment);
            await _commentContext.SaveChangesAsync();

            return Ok("Comment deleted");
        }

        [Authorize]
        [HttpPut("{id}/Report")]
        public async Task<IActionResult> ReportComment(Report report)
        {
            var comment = await _commentContext.Comments.FindAsync(report.ItemId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            comment.ReportCount += 1;
            if (comment.ReportCount > 10)
            {
                using (var reportContext = new ReportContext())
                {
                    reportContext.Reports
                    .RemoveRange(reportContext.Reports
                    .Where(x => x.ItemId == report.ItemId && x.Type == report.Type));

                    await reportContext.SaveChangesAsync();
                }
                _commentContext.Comments.Remove(comment);
                await _commentContext.SaveChangesAsync();
                return Ok("Reported comment removed");
            }
            
            try
            {
                await _commentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(report.ItemId))
            {
                return NotFound("Comment not found");
            }
            return Ok("Comment reported");
        }

        private bool CommentExists(int id)
        {
            return (_commentContext.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Converts the Comment object to CommentDTO hiding private info
        private static CommentDTO CommentToDTO(Comment marker)
        => new CommentDTO
        {
            Id = marker.Id,
            MarkerId = marker.MarkerId,
            UserName = marker.UserName,
            DateTime = marker.DateTime,
            Message = marker.Message,  
        };
    }
}
