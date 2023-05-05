using APAssistantAPI.Data;
using APAssistantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<IEnumerable<Location>> GetLocations()
        {
            return await _context.Locations.AsNoTracking().ToListAsync();
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(Guid id)
        {
            var location = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
                return NotFound();

            return location;
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            if (await _context.Locations.AsNoTracking().AnyAsync(l => l.Id == location.Id))
                _context.Entry(location).State = EntityState.Modified;
            else
                _context.Locations.Add(location);

            await _context.SaveChangesAsync();


            return location;
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
                return NotFound();

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
