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
    public class FlowerArrangementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowerArrangementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlowerArrangements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FlowerArrangements.Include(f => f.Florist).Include(f => f.FlowerArrangementType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FlowerArrangements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangement = await _context.FlowerArrangements
                .Include(f => f.Florist)
                .Include(f => f.FlowerArrangementType)
                .FirstOrDefaultAsync(m => m.FlowerArrangementId == id);
            if (flowerArrangement == null)
            {
                return NotFound();
            }

            return View(flowerArrangement);
        }

        // GET: FlowerArrangements/Create
        public IActionResult Create()
        {
            ViewData["FloristId"] = new SelectList(_context.Florists, "FloristId", "FloristId");
            ViewData["FlowerArrangementTypeId"] = new SelectList(_context.FlowerArrangementTypes, "FlowerArrangementTypeId", "FlowerArrangementTypeId");
            return View();
        }

        // POST: FlowerArrangements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlowerArrangementId,Name,Description,Price,FloristId,FlowerArrangementTypeId,TenantId")] FlowerArrangement flowerArrangement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flowerArrangement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FloristId"] = new SelectList(_context.Florists, "FloristId", "FloristId", flowerArrangement.FloristId);
            ViewData["FlowerArrangementTypeId"] = new SelectList(_context.FlowerArrangementTypes, "FlowerArrangementTypeId", "FlowerArrangementTypeId", flowerArrangement.FlowerArrangementTypeId);
            return View(flowerArrangement);
        }

        // GET: FlowerArrangements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangement = await _context.FlowerArrangements.FindAsync(id);
            if (flowerArrangement == null)
            {
                return NotFound();
            }
            ViewData["FloristId"] = new SelectList(_context.Florists, "FloristId", "FloristId", flowerArrangement.FloristId);
            ViewData["FlowerArrangementTypeId"] = new SelectList(_context.FlowerArrangementTypes, "FlowerArrangementTypeId", "FlowerArrangementTypeId", flowerArrangement.FlowerArrangementTypeId);
            return View(flowerArrangement);
        }

        // POST: FlowerArrangements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlowerArrangementId,Name,Description,Price,FloristId,FlowerArrangementTypeId,TenantId")] FlowerArrangement flowerArrangement)
        {
            if (id != flowerArrangement.FlowerArrangementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flowerArrangement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowerArrangementExists(flowerArrangement.FlowerArrangementId))
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
            ViewData["FloristId"] = new SelectList(_context.Florists, "FloristId", "FloristId", flowerArrangement.FloristId);
            ViewData["FlowerArrangementTypeId"] = new SelectList(_context.FlowerArrangementTypes, "FlowerArrangementTypeId", "FlowerArrangementTypeId", flowerArrangement.FlowerArrangementTypeId);
            return View(flowerArrangement);
        }

        // GET: FlowerArrangements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangement = await _context.FlowerArrangements
                .Include(f => f.Florist)
                .Include(f => f.FlowerArrangementType)
                .FirstOrDefaultAsync(m => m.FlowerArrangementId == id);
            if (flowerArrangement == null)
            {
                return NotFound();
            }

            return View(flowerArrangement);
        }

        // POST: FlowerArrangements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flowerArrangement = await _context.FlowerArrangements.FindAsync(id);
            if (flowerArrangement != null)
            {
                _context.FlowerArrangements.Remove(flowerArrangement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlowerArrangementExists(int id)
        {
            return _context.FlowerArrangements.Any(e => e.FlowerArrangementId == id);
        }
    }
}
