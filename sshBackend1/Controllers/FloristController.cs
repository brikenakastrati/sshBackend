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
    public class FloristController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FloristController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Florist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Florist>>> GetFlorists()
        {
            return await _context.Florists.ToListAsync();
        }

        // GET: api/Florist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Florist>> GetFlorist(int id)
        {
            var florist = await _context.Florists.FindAsync(id);

            if (florist == null)
            {
                return NotFound();
            }

            return florist;
        }

        // PUT: api/Florist/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlorist(int id, Florist florist)
        {
            if (id != florist.FloristId)
            {
                return BadRequest();
            }

            _context.Entry(florist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloristExists(id))
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

        // POST: api/Florist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Florist>> PostFlorist(Florist florist)
        {
            _context.Florists.Add(florist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlorist", new { id = florist.FloristId }, florist);
        }

        // DELETE: api/Florist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlorist(int id)
        {
            var florist = await _context.Florists.FindAsync(id);
            if (florist == null)
            {
                return NotFound();
            }

            _context.Florists.Remove(florist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FloristExists(int id)
        {
            return _context.Florists.Any(e => e.FloristId == id);
        }
    }
}
