using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models.MarkerModel;
using WGO_API.Models.ReportModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WGO_API.Models.UserModel;

namespace WGO_API.Controllers
{
    [Route("api/Markers")]
    [ApiController]
    public class MarkersController : ControllerBase
    {
        private readonly MarkerContext _userContext;
        private readonly UserManager<User> _userManager;

        public MarkersController(MarkerContext context, UserManager<User> userManager)
        {
            _userContext = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MarkerDTO>> PostMarker(MarkerDTO markerDTO)
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

            Marker marker = new Marker
            {
                Id = markerDTO.Id,
                Title = markerDTO.Title,
                Summary = markerDTO.Summary,
                UserId = user.Id,
                UserName = user.UserName ?? "UserName Not Found",
                latitude = markerDTO.latitude,
                longitude = markerDTO.longitude,
                DateTime = markerDTO.DateTime,
                EndTime = markerDTO.EndTime,
            };

            _userContext.Markers.Add(marker);
            await _userContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMarker),
                new { id = marker.Id },
                MarkerToDTO(marker));
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> PutMarker(MarkerDTO markerDTO)
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

            var marker = await _userContext.Markers.FindAsync(markerDTO.Id);

            if (marker == null)
            {
                return NotFound("Marker not found");
            }

            if (marker.UserId != user.Id)
            {
                return Unauthorized("User unauthorized to make changes to this marker.");
            }

            updateMarker(marker, markerDTO);

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MarkerExists(markerDTO.Id))
            {
                return NotFound("Marker not found");
            }

            return Ok("Marker updated successfully");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarkerDTO>>> GetMarkers()
        {
            if (_userContext.Markers == null)
            {
                return StatusCode(500);
            }
            return await _userContext.Markers
              .Select(x => MarkerToDTO(x))
              .ToListAsync();
        }

        [HttpGet("Since")]
        public async Task<ActionResult<IEnumerable<MarkerDTO>>> GetMarkersUpdate(DateTime dateTime)
        {
            if (_userContext.Markers == null)
            {
                return StatusCode(500);
            }
            return await _userContext.Markers
              .Where(x => DateTime.Compare(x.DateTime, dateTime) >= 0)
              .Select(x => MarkerToDTO(x))
              .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarkerDTO>> GetMarker(int id)
        {
            if (_userContext.Markers == null)
            {
                return StatusCode(500);
            }
            var marker = await _userContext.Markers.FindAsync(id);

            if (marker == null)
            {
                return NotFound("Marker not found");
            }

            return MarkerToDTO(marker);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarker(int id)
        {
            if (_userContext.Markers == null)
            {
                return StatusCode(500);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var marker = await _userContext.Markers.FindAsync(id);

            if (marker == null)
            {
                return NotFound("Marker not found");
            }

            if (marker.UserId != user.Id)
            {
                return Unauthorized("User unauthorized to make changes to this marker.");
            }

            _userContext.Markers.Remove(marker);
            await _userContext.SaveChangesAsync();

            return Ok("Marker deleted");
        }

        [Authorize]
        [HttpPut("{id}/Report")]
        public async Task<IActionResult> ReportMarker(Report report)
        {
            var marker = await _userContext.Markers.FindAsync(report.ItemId);

            if (marker == null)
            {
                return NotFound("Marker not found");
            }

            marker.ReportCount += 1;
            if (marker.ReportCount > 10)
            {
                using (var reportContext = new ReportContext())
                {
                    reportContext.Reports
                    .RemoveRange(reportContext.Reports
                    .Where(x => x.ItemId == report.ItemId && x.Type == report.Type));

                    await reportContext.SaveChangesAsync();
                }
                _userContext.Markers.Remove(marker);
                await _userContext.SaveChangesAsync();
                return Ok("Reported marker removed");
            }

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MarkerExists(report.ItemId))
            {
                return NotFound("Marker not found");
            }
            return Ok("Marker reported");
        }

        private bool MarkerExists(int id)
        {
            return (_userContext.Markers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Converts the Marker object to MarkerDTO hiding private info
        private static MarkerDTO MarkerToDTO(Marker marker)
        => new MarkerDTO
        {
            Id = marker.Id,
            UserName = marker.UserName,
            Title = marker.Title,
            Summary = marker.Summary,
            //Not adding reportCount
            longitude = marker.longitude,
            latitude = marker.latitude,
            DateTime = marker.DateTime,
            EndTime = marker.EndTime,   
        };

        //Update the marker entry with info from markerDTO
        private static void updateMarker(Marker marker, MarkerDTO markerDTO)
        {
            marker.Title = markerDTO.Title;
            marker.Summary = markerDTO.Summary;
            marker.latitude = markerDTO.latitude;
            marker.longitude = markerDTO.longitude;
            marker.DateTime = markerDTO.DateTime;
            marker.EndTime = markerDTO.EndTime;
        }
    }
}
