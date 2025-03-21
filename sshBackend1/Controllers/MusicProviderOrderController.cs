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
    public class MusicProviderOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusicProviderOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MusicProviderOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicProviderOrder>>> GetMusicProviderOrders()
        {
            return await _context.MusicProviderOrders.ToListAsync();
        }

        // GET: api/MusicProviderOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicProviderOrder>> GetMusicProviderOrder(int id)
        {
            var musicProviderOrder = await _context.MusicProviderOrders.FindAsync(id);

            if (musicProviderOrder == null)
            {
                return NotFound();
            }

            return musicProviderOrder;
        }

        // PUT: api/MusicProviderOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusicProviderOrder(int id, MusicProviderOrder musicProviderOrder)
        {
            if (id != musicProviderOrder.MusicProviderOrderId)
            {
                return BadRequest();
            }

            _context.Entry(musicProviderOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicProviderOrderExists(id))
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

        // POST: api/MusicProviderOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MusicProviderOrder>> PostMusicProviderOrder(MusicProviderOrder musicProviderOrder)
        {
            _context.MusicProviderOrders.Add(musicProviderOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMusicProviderOrder", new { id = musicProviderOrder.MusicProviderOrderId }, musicProviderOrder);
        }

        // DELETE: api/MusicProviderOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusicProviderOrder(int id)
        {
            var musicProviderOrder = await _context.MusicProviderOrders.FindAsync(id);
            if (musicProviderOrder == null)
            {
                return NotFound();
            }

            _context.MusicProviderOrders.Remove(musicProviderOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicProviderOrderExists(int id)
        {
            return _context.MusicProviderOrders.Any(e => e.MusicProviderOrderId == id);
        }
    }
}
