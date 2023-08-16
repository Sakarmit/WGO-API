using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models.MarkerModel;

namespace WGO_API.Controllers
{
    [Route("api/Markers")]
    [ApiController]
    public class MarkersController : ControllerBase
    {
        private readonly MarkerContext _context;

        public MarkersController(MarkerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarkerDTO>>> GetMarkers()
        {
            if (_context.Markers == null)
            {
                return NotFound();
            }
            return await _context.Markers
              .Select(x => MarkerToDTO(x))
              .ToListAsync();
        }

        [HttpGet("Updates")]
        public async Task<ActionResult<IEnumerable<MarkerDTO>>> GetMarkersUpdate(DateTime dateTime)
        {
            if (_context.Markers == null)
            {
                return NotFound();
            }
            return await _context.Markers
              .Where(x => DateTime.Compare(x.DateTime, dateTime) >= 0)
              .Select(x => MarkerToDTO(x))
              .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarkerDTO>> GetMarker(int id)
        {
            if (_context.Markers == null)
            {
                return NotFound();
            }
            var marker = await _context.Markers.FindAsync(id);

            if (marker == null)
            {
                return NotFound();
            }

            return MarkerToDTO(marker);
        }

        [HttpPost]
        public async Task<ActionResult<MarkerDTO>> PostMarker(MarkerDTO markerDTO)
        {
            var marker = new Marker
            {
                Id = markerDTO.Id,
                Title = markerDTO.Title,
                Summary = markerDTO.Summary,
                UserId = markerDTO.UserId,
                latitude = markerDTO.latitude,
                longitude = markerDTO.longitude,
                DateTime = markerDTO.DateTime,
                EndTime = markerDTO.EndTime,
            };

            _context.Markers.Add(marker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMarker),
                new { id = marker.Id },
                MarkerToDTO(marker));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarker(int id, MarkerDTO markerDTO)
        {
            if (id != markerDTO.Id)
            {
                return BadRequest();
            }

            var marker = await _context.Markers.FindAsync(id);
            
            if (marker == null)
            {
                return NotFound();
            }

            updateMarker(marker, markerDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MarkerExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarker(int id)
        {
            if (_context.Markers == null)
            {
                return NotFound();
            }
            var marker = await _context.Markers.FindAsync(id);
            if (marker == null)
            {
                return NotFound();
            }

            _context.Markers.Remove(marker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/Report")]
        public async Task<ActionResult<int>> ReportMarker(int id)
        {
            var marker = await _context.Markers.FindAsync(id);

            if (marker == null)
            {
                return NotFound();
            }

            marker.ReportCount += 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MarkerExists(id))
            {
                return NotFound();
            }
            return marker.ReportCount;
        }

        private bool MarkerExists(int id)
        {
            return (_context.Markers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Converts the Marker object to MarkerDTO hiding private info
        private static MarkerDTO MarkerToDTO(Marker marker)
        => new MarkerDTO
        {
            Id = marker.Id,
            UserId = marker.UserId,
            //Not adding reportCount
            latitude = marker.latitude,
            longitude = marker.longitude,
            DateTime = marker.DateTime,
            Summary = marker.Summary,
            Title = marker.Title
        };

        //Update the marker entry with info from markerDTO
        private static void updateMarker(Marker marker, MarkerDTO markerDTO)
        {
            marker.Title = markerDTO.Title;
            marker.Summary = markerDTO.Summary;
            marker.latitude = markerDTO.latitude;
            marker.longitude = markerDTO.longitude;
            marker.DateTime = markerDTO.DateTime;
        }
    }
}
