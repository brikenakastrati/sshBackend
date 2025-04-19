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
    public class PastriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PastriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pastries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pastries.Include(p => p.PastryType).Include(p => p.Shop);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pastries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastries
                .Include(p => p.PastryType)
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(m => m.PastryId == id);
            if (pastry == null)
            {
                return NotFound();
            }

            return View(pastry);
        }

        // GET: Pastries/Create
        public IActionResult Create()
        {
            ViewData["PastryTypeId"] = new SelectList(_context.PastryTypes, "PastryTypeId", "PastryTypeId");
            ViewData["ShopId"] = new SelectList(_context.PastryShops, "ShopId", "ShopId");
            return View();
        }

        // POST: Pastries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PastryId,PastryName,Price,ShopId,PastryTypeId,TenantId")] Pastry pastry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PastryTypeId"] = new SelectList(_context.PastryTypes, "PastryTypeId", "PastryTypeId", pastry.PastryTypeId);
            ViewData["ShopId"] = new SelectList(_context.PastryShops, "ShopId", "ShopId", pastry.ShopId);
            return View(pastry);
        }

        // GET: Pastries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastries.FindAsync(id);
            if (pastry == null)
            {
                return NotFound();
            }
            ViewData["PastryTypeId"] = new SelectList(_context.PastryTypes, "PastryTypeId", "PastryTypeId", pastry.PastryTypeId);
            ViewData["ShopId"] = new SelectList(_context.PastryShops, "ShopId", "ShopId", pastry.ShopId);
            return View(pastry);
        }

        // POST: Pastries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PastryId,PastryName,Price,ShopId,PastryTypeId,TenantId")] Pastry pastry)
        {
            if (id != pastry.PastryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastryExists(pastry.PastryId))
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
            ViewData["PastryTypeId"] = new SelectList(_context.PastryTypes, "PastryTypeId", "PastryTypeId", pastry.PastryTypeId);
            ViewData["ShopId"] = new SelectList(_context.PastryShops, "ShopId", "ShopId", pastry.ShopId);
            return View(pastry);
        }

        // GET: Pastries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastry = await _context.Pastries
                .Include(p => p.PastryType)
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(m => m.PastryId == id);
            if (pastry == null)
            {
                return NotFound();
            }

            return View(pastry);
        }

        // POST: Pastries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastry = await _context.Pastries.FindAsync(id);
            if (pastry != null)
            {
                _context.Pastries.Remove(pastry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastryExists(int id)
        {
            return _context.Pastries.Any(e => e.PastryId == id);
        }
    }
}
