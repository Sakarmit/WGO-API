using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WGO_API.Models;

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
        public async Task<ActionResult<IEnumerable<Marker>>> GetMarkers()
        {
          if (_context.Markers == null)
          {
              return NotFound();
          }
            return await _context.Markers.ToListAsync();
        }

        // GET: api/Markers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Marker>> GetMarker(long id)
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

            return marker;
        }

        // PUT: api/Markers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarker(long id, Marker marker)
        {
            if (id != marker.Id)
            {
                return BadRequest();
            }

            _context.Entry(marker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkerExists(id))
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

        // POST: api/Markers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Marker>> PostMarker(Marker marker)
        {
          if (_context.Markers == null)
          {
              return Problem("Entity set 'MarkerContext.Markers'  is null.");
          }
            _context.Markers.Add(marker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMarker), new { id = marker.Id }, marker);
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
    }
}
