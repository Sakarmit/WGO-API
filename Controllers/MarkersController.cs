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

        // GET: api/Markers
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

        // GET: api/Markers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MarkerDTO>> GetMarker(long id)
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

        // PUT: api/Markers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarker(long id, MarkerDTO markerDTO)
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

        // POST: api/Markers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MarkerDTO>> PostMarker(Marker markerDTO)
        {
            var marker = new Marker
            {
                Id = markerDTO.Id,
                UserId = markerDTO.UserId,
                latitude = markerDTO.latitude,
                longitude = markerDTO.longitude,
                DateTime = markerDTO.DateTime,
                Summary = markerDTO.Summary,
                Title = markerDTO.Title
            };
            
            _context.Markers.Add(marker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMarker),
                new { id = marker.Id },
                MarkerToDTO(marker));
        }

        // DELETE: api/Markers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarker(long id)
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

        private bool MarkerExists(long id)
        {
            return (_context.Markers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Converts the Marker object to MarkerDTO hiding non-public info
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
