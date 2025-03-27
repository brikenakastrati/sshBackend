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
    public class VenueOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VenueOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VenueOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueOrder>>> GetVenueOrders()
        {
            return await _context.VenueOrders.ToListAsync();
        }

        // GET: api/VenueOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueOrder>> GetVenueOrder(int id)
        {
            var venueOrder = await _context.VenueOrders.FindAsync(id);

            if (venueOrder == null)
            {
                return NotFound();
            }

            return venueOrder;
        }

        // PUT: api/VenueOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenueOrder(int id, VenueOrder venueOrder)
        {
            if (id != venueOrder.VenueOrderId)
            {
                return BadRequest();
            }

            _context.Entry(venueOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueOrderExists(id))
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

        // POST: api/VenueOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VenueOrder>> PostVenueOrder(VenueOrder venueOrder)
        {
            _context.VenueOrders.Add(venueOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenueOrder", new { id = venueOrder.VenueOrderId }, venueOrder);
        }

        // DELETE: api/VenueOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenueOrder(int id)
        {
            var venueOrder = await _context.VenueOrders.FindAsync(id);
            if (venueOrder == null)
            {
                return NotFound();
            }

            _context.VenueOrders.Remove(venueOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenueOrderExists(int id)
        {
            return _context.VenueOrders.Any(e => e.VenueOrderId == id);
        }
    }
}
