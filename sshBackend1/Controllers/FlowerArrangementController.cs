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
    public class FlowerArrangementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlowerArrangementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FlowerArrangement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlowerArrangement>>> GetFlowerArrangements()
        {
            return await _context.FlowerArrangements.ToListAsync();
        }

        // GET: api/FlowerArrangement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlowerArrangement>> GetFlowerArrangement(int id)
        {
            var flowerArrangement = await _context.FlowerArrangements.FindAsync(id);

            if (flowerArrangement == null)
            {
                return NotFound();
            }

            return flowerArrangement;
        }

        // PUT: api/FlowerArrangement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlowerArrangement(int id, FlowerArrangement flowerArrangement)
        {
            if (id != flowerArrangement.FlowerArrangementId)
            {
                return BadRequest();
            }

            _context.Entry(flowerArrangement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerArrangementExists(id))
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

        // POST: api/FlowerArrangement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlowerArrangement>> PostFlowerArrangement(FlowerArrangement flowerArrangement)
        {
            _context.FlowerArrangements.Add(flowerArrangement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlowerArrangement", new { id = flowerArrangement.FlowerArrangementId }, flowerArrangement);
        }

        // DELETE: api/FlowerArrangement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlowerArrangement(int id)
        {
            var flowerArrangement = await _context.FlowerArrangements.FindAsync(id);
            if (flowerArrangement == null)
            {
                return NotFound();
            }

            _context.FlowerArrangements.Remove(flowerArrangement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlowerArrangementExists(int id)
        {
            return _context.FlowerArrangements.Any(e => e.FlowerArrangementId == id);
        }
    }
}
