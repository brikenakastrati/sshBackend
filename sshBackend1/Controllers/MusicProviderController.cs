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
    public class MusicProviderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusicProviderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MusicProvider
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicProvider>>> GetMusicProviders()
        {
            return await _context.MusicProviders.ToListAsync();
        }

        // GET: api/MusicProvider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicProvider>> GetMusicProvider(int id)
        {
            var musicProvider = await _context.MusicProviders.FindAsync(id);

            if (musicProvider == null)
            {
                return NotFound();
            }

            return musicProvider;
        }

        // PUT: api/MusicProvider/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusicProvider(int id, MusicProvider musicProvider)
        {
            if (id != musicProvider.MusicProviderId)
            {
                return BadRequest();
            }

            _context.Entry(musicProvider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicProviderExists(id))
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

        // POST: api/MusicProvider
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MusicProvider>> PostMusicProvider(MusicProvider musicProvider)
        {
            _context.MusicProviders.Add(musicProvider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMusicProvider", new { id = musicProvider.MusicProviderId }, musicProvider);
        }

        // DELETE: api/MusicProvider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusicProvider(int id)
        {
            var musicProvider = await _context.MusicProviders.FindAsync(id);
            if (musicProvider == null)
            {
                return NotFound();
            }

            _context.MusicProviders.Remove(musicProvider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicProviderExists(int id)
        {
            return _context.MusicProviders.Any(e => e.MusicProviderId == id);
        }
    }
}
