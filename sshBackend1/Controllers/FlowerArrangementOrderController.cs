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
    public class FlowerArrangementOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlowerArrangementOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FlowerArrangementOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlowerArrangementOrder>>> GetFlowerArrangementOrders()
        {
            return await _context.FlowerArrangementOrders.ToListAsync();
        }

        // GET: api/FlowerArrangementOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlowerArrangementOrder>> GetFlowerArrangementOrder(int id)
        {
            var flowerArrangementOrder = await _context.FlowerArrangementOrders.FindAsync(id);

            if (flowerArrangementOrder == null)
            {
                return NotFound();
            }

            return flowerArrangementOrder;
        }

        // PUT: api/FlowerArrangementOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlowerArrangementOrder(int id, FlowerArrangementOrder flowerArrangementOrder)
        {
            if (id != flowerArrangementOrder.FlowerArrangementOrderId)
            {
                return BadRequest();
            }

            _context.Entry(flowerArrangementOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerArrangementOrderExists(id))
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

        // POST: api/FlowerArrangementOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlowerArrangementOrder>> PostFlowerArrangementOrder(FlowerArrangementOrder flowerArrangementOrder)
        {
            _context.FlowerArrangementOrders.Add(flowerArrangementOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlowerArrangementOrder", new { id = flowerArrangementOrder.FlowerArrangementOrderId }, flowerArrangementOrder);
        }

        // DELETE: api/FlowerArrangementOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlowerArrangementOrder(int id)
        {
            var flowerArrangementOrder = await _context.FlowerArrangementOrders.FindAsync(id);
            if (flowerArrangementOrder == null)
            {
                return NotFound();
            }

            _context.FlowerArrangementOrders.Remove(flowerArrangementOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlowerArrangementOrderExists(int id)
        {
            return _context.FlowerArrangementOrders.Any(e => e.FlowerArrangementOrderId == id);
        }
    }
}
