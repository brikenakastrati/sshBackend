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
    public class PastryOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PastryOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PastryOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PastryOrder>>> GetPastryOrders()
        {
            return await _context.PastryOrders.ToListAsync();
        }

        // GET: api/PastryOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PastryOrder>> GetPastryOrder(int id)
        {
            var pastryOrder = await _context.PastryOrders.FindAsync(id);

            if (pastryOrder == null)
            {
                return NotFound();
            }

            return pastryOrder;
        }

        // PUT: api/PastryOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPastryOrder(int id, PastryOrder pastryOrder)
        {
            if (id != pastryOrder.PastryOrderId)
            {
                return BadRequest();
            }

            _context.Entry(pastryOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PastryOrderExists(id))
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

        // POST: api/PastryOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PastryOrder>> PostPastryOrder(PastryOrder pastryOrder)
        {
            _context.PastryOrders.Add(pastryOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPastryOrder", new { id = pastryOrder.PastryOrderId }, pastryOrder);
        }

        // DELETE: api/PastryOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePastryOrder(int id)
        {
            var pastryOrder = await _context.PastryOrders.FindAsync(id);
            if (pastryOrder == null)
            {
                return NotFound();
            }

            _context.PastryOrders.Remove(pastryOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PastryOrderExists(int id)
        {
            return _context.PastryOrders.Any(e => e.PastryOrderId == id);
        }
    }
}
