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
    public class FlowerArrangementTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowerArrangementTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlowerArrangementTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FlowerArrangementTypes.ToListAsync());
        }

        // GET: FlowerArrangementTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementType = await _context.FlowerArrangementTypes
                .FirstOrDefaultAsync(m => m.FlowerArrangementTypeId == id);
            if (flowerArrangementType == null)
            {
                return NotFound();
            }

            return View(flowerArrangementType);
        }

        // GET: FlowerArrangementTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlowerArrangementTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlowerArrangementTypeId,Name,TenantId")] FlowerArrangementType flowerArrangementType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flowerArrangementType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flowerArrangementType);
        }

        // GET: FlowerArrangementTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementType = await _context.FlowerArrangementTypes.FindAsync(id);
            if (flowerArrangementType == null)
            {
                return NotFound();
            }
            return View(flowerArrangementType);
        }

        // POST: FlowerArrangementTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlowerArrangementTypeId,Name,TenantId")] FlowerArrangementType flowerArrangementType)
        {
            if (id != flowerArrangementType.FlowerArrangementTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flowerArrangementType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowerArrangementTypeExists(flowerArrangementType.FlowerArrangementTypeId))
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
            return View(flowerArrangementType);
        }

        // GET: FlowerArrangementTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementType = await _context.FlowerArrangementTypes
                .FirstOrDefaultAsync(m => m.FlowerArrangementTypeId == id);
            if (flowerArrangementType == null)
            {
                return NotFound();
            }

            return View(flowerArrangementType);
        }

        // POST: FlowerArrangementTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flowerArrangementType = await _context.FlowerArrangementTypes.FindAsync(id);
            if (flowerArrangementType != null)
            {
                _context.FlowerArrangementTypes.Remove(flowerArrangementType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlowerArrangementTypeExists(int id)
        {
            return _context.FlowerArrangementTypes.Any(e => e.FlowerArrangementTypeId == id);
        }
    }
}
