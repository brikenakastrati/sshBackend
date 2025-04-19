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
    public class PastryTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PastryTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PastryTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PastryTypes.ToListAsync());
        }

        // GET: PastryTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryType = await _context.PastryTypes
                .FirstOrDefaultAsync(m => m.PastryTypeId == id);
            if (pastryType == null)
            {
                return NotFound();
            }

            return View(pastryType);
        }

        // GET: PastryTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PastryTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PastryTypeId,TypeName,TenantId")] PastryType pastryType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastryType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pastryType);
        }

        // GET: PastryTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryType = await _context.PastryTypes.FindAsync(id);
            if (pastryType == null)
            {
                return NotFound();
            }
            return View(pastryType);
        }

        // POST: PastryTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PastryTypeId,TypeName,TenantId")] PastryType pastryType)
        {
            if (id != pastryType.PastryTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastryType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastryTypeExists(pastryType.PastryTypeId))
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
            return View(pastryType);
        }

        // GET: PastryTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryType = await _context.PastryTypes
                .FirstOrDefaultAsync(m => m.PastryTypeId == id);
            if (pastryType == null)
            {
                return NotFound();
            }

            return View(pastryType);
        }

        // POST: PastryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastryType = await _context.PastryTypes.FindAsync(id);
            if (pastryType != null)
            {
                _context.PastryTypes.Remove(pastryType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastryTypeExists(int id)
        {
            return _context.PastryTypes.Any(e => e.PastryTypeId == id);
        }
    }
}
