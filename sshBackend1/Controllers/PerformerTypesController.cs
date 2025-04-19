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
    public class PerformerTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformerTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PerformerTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PerformerTypes.ToListAsync());
        }

        // GET: PerformerTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performerType = await _context.PerformerTypes
                .FirstOrDefaultAsync(m => m.PerformerTypeId == id);
            if (performerType == null)
            {
                return NotFound();
            }

            return View(performerType);
        }

        // GET: PerformerTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PerformerTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformerTypeId,Name,TenantId")] PerformerType performerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(performerType);
        }

        // GET: PerformerTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performerType = await _context.PerformerTypes.FindAsync(id);
            if (performerType == null)
            {
                return NotFound();
            }
            return View(performerType);
        }

        // POST: PerformerTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformerTypeId,Name,TenantId")] PerformerType performerType)
        {
            if (id != performerType.PerformerTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformerTypeExists(performerType.PerformerTypeId))
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
            return View(performerType);
        }

        // GET: PerformerTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performerType = await _context.PerformerTypes
                .FirstOrDefaultAsync(m => m.PerformerTypeId == id);
            if (performerType == null)
            {
                return NotFound();
            }

            return View(performerType);
        }

        // POST: PerformerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performerType = await _context.PerformerTypes.FindAsync(id);
            if (performerType != null)
            {
                _context.PerformerTypes.Remove(performerType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformerTypeExists(int id)
        {
            return _context.PerformerTypes.Any(e => e.PerformerTypeId == id);
        }
    }
}
