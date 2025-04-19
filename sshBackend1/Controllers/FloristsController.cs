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
    public class FloristsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FloristsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Florists
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Florists.Include(f => f.PartnerStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Florists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var florist = await _context.Florists
                .Include(f => f.PartnerStatus)
                .FirstOrDefaultAsync(m => m.FloristId == id);
            if (florist == null)
            {
                return NotFound();
            }

            return View(florist);
        }

        // GET: Florists/Create
        public IActionResult Create()
        {
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId");
            return View();
        }

        // POST: Florists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FloristId,Name,Address,PhoneNumber,Email,AgencyFee,PartnerStatusId,TenantId")] Florist florist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(florist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", florist.PartnerStatusId);
            return View(florist);
        }

        // GET: Florists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var florist = await _context.Florists.FindAsync(id);
            if (florist == null)
            {
                return NotFound();
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", florist.PartnerStatusId);
            return View(florist);
        }

        // POST: Florists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FloristId,Name,Address,PhoneNumber,Email,AgencyFee,PartnerStatusId,TenantId")] Florist florist)
        {
            if (id != florist.FloristId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(florist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FloristExists(florist.FloristId))
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
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", florist.PartnerStatusId);
            return View(florist);
        }

        // GET: Florists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var florist = await _context.Florists
                .Include(f => f.PartnerStatus)
                .FirstOrDefaultAsync(m => m.FloristId == id);
            if (florist == null)
            {
                return NotFound();
            }

            return View(florist);
        }

        // POST: Florists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var florist = await _context.Florists.FindAsync(id);
            if (florist != null)
            {
                _context.Florists.Remove(florist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FloristExists(int id)
        {
            return _context.Florists.Any(e => e.FloristId == id);
        }
    }
}
