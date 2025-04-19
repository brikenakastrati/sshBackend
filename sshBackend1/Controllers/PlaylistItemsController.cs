using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;

namespace sshBackend1.Controllers
{
    public class PlaylistItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaylistItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlaylistItems.Include(p => p.MusicProvider);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PlaylistItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems
                .Include(p => p.MusicProvider)
                .FirstOrDefaultAsync(m => m.PlaylistItemId == id);
            if (playlistItem == null)
            {
                return NotFound();
            }

            return View(playlistItem);
        }

        // GET: PlaylistItems/Create
        public IActionResult Create()
        {
            ViewData["MusicProviderId"] = new SelectList(_context.MusicProviders, "MusicProviderId", "MusicProviderId");
            return View();
        }

        // POST: PlaylistItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistItemId,Name,GenreId,MusicProviderId,Length,TenantId")] PlaylistItem playlistItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playlistItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MusicProviderId"] = new SelectList(_context.MusicProviders, "MusicProviderId", "MusicProviderId", playlistItem.MusicProviderId);
            return View(playlistItem);
        }

        // GET: PlaylistItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems.FindAsync(id);
            if (playlistItem == null)
            {
                return NotFound();
            }
            ViewData["MusicProviderId"] = new SelectList(_context.MusicProviders, "MusicProviderId", "MusicProviderId", playlistItem.MusicProviderId);
            return View(playlistItem);
        }

        // POST: PlaylistItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaylistItemId,Name,GenreId,MusicProviderId,Length,TenantId")] PlaylistItem playlistItem)
        {
            if (id != playlistItem.PlaylistItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistItemExists(playlistItem.PlaylistItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MusicProviderId"] = new SelectList(_context.MusicProviders, "MusicProviderId", "MusicProviderId", playlistItem.MusicProviderId);
            return View(playlistItem);
        }

        // GET: PlaylistItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems
                .Include(p => p.MusicProvider)
                .FirstOrDefaultAsync(m => m.PlaylistItemId == id);
            if (playlistItem == null)
            {
                return NotFound();
            }

            return View(playlistItem);
        }

        // POST: PlaylistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlistItem = await _context.PlaylistItems.FindAsync(id);
            if (playlistItem != null)
            {
                _context.PlaylistItems.Remove(playlistItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistItemExists(int id)
        {
            return _context.PlaylistItems.Any(e => e.PlaylistItemId == id);
        }
    }
}
