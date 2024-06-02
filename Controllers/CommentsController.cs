using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models.CommentModel;
using WGO_API.Models.ReportModel;

namespace WGO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _context;

        public CommentsController(CommentContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForMarker(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            return await _context.Comments
              .Where(x => x.MarkerId == id)
              .Select(x => CommentToDTO(x))
              .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
          if (_context.Comments == null)
          {
              return Problem("Entity set 'CommentContext.Comments'  is null.");
          }
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/Report")]
        public async Task<ActionResult<bool>> ReportComment(Report report)
        {
            var comment = await _context.Comments.FindAsync(report.ItemId);

            if (comment == null)
            {
                return NotFound();
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
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(report.ItemId))
            {
                return NotFound();
            }
            return false;
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Converts the Comment object to CommentDTO hiding private info
        private static CommentDTO CommentToDTO(Comment marker)
        => new CommentDTO
        {
            Id = marker.Id,
            MarkerId = marker.MarkerId,
            UserId = marker.UserId,
            DateTime = marker.DateTime,
            Message = marker.Message,  
        };
    }
}
