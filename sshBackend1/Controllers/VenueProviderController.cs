using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueProviderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VenueProviderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VenueProvider
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueProvider>>> GetVenueProviders()
        {
            return await _context.VenueProviders.ToListAsync();
        }

        // GET: api/VenueProvider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueProvider>> GetVenueProvider(int id)
        {
            var venueProvider = await _context.VenueProviders.FindAsync(id);

            if (venueProvider == null)
            {
                return NotFound();
            }

            return venueProvider;
        }

        // PUT: api/VenueProvider/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenueProvider(int id, VenueProvider venueProvider)
        {
            if (id != venueProvider.VenueProviderId)
            {
                return BadRequest();
            }

            _context.Entry(venueProvider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueProviderExists(id))
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

        // POST: api/VenueProvider
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VenueProvider>> PostVenueProvider(VenueProvider venueProvider)
        {
            _context.VenueProviders.Add(venueProvider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenueProvider", new { id = venueProvider.VenueProviderId }, venueProvider);
        }

        // DELETE: api/VenueProvider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenueProvider(int id)
        {
            var venueProvider = await _context.VenueProviders.FindAsync(id);
            if (venueProvider == null)
            {
                return NotFound();
            }

            _context.VenueProviders.Remove(venueProvider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenueProviderExists(int id)
        {
            return _context.VenueProviders.Any(e => e.VenueProviderId == id);
        }
    }
}
