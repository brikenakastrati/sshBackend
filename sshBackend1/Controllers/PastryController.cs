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
    public class PastryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PastryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pastry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pastry>>> GetPastries()
        {
            return await _context.Pastries.ToListAsync();
        }

        // GET: api/Pastry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pastry>> GetPastry(int id)
        {
            var pastry = await _context.Pastries.FindAsync(id);

            if (pastry == null)
            {
                return NotFound();
            }

            return pastry;
        }

        // PUT: api/Pastry/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPastry(int id, Pastry pastry)
        {
            if (id != pastry.PastryId)
            {
                return BadRequest();
            }

            _context.Entry(pastry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PastryExists(id))
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

        // POST: api/Pastry
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pastry>> PostPastry(Pastry pastry)
        {
            _context.Pastries.Add(pastry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPastry", new { id = pastry.PastryId }, pastry);
        }

        // DELETE: api/Pastry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePastry(int id)
        {
            var pastry = await _context.Pastries.FindAsync(id);
            if (pastry == null)
            {
                return NotFound();
            }

            _context.Pastries.Remove(pastry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PastryExists(int id)
        {
            return _context.Pastries.Any(e => e.PastryId == id);
        }
    }
}
