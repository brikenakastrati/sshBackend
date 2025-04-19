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
    public class PastryShopsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PastryShopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PastryShops
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PastryShops.Include(p => p.PartnerStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PastryShops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryShop = await _context.PastryShops
                .Include(p => p.PartnerStatus)
                .FirstOrDefaultAsync(m => m.ShopId == id);
            if (pastryShop == null)
            {
                return NotFound();
            }

            return View(pastryShop);
        }

        // GET: PastryShops/Create
        public IActionResult Create()
        {
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId");
            return View();
        }

        // POST: PastryShops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopId,ShopName,Address,PhoneNumber,PartnerStatusId,TenantId")] PastryShop pastryShop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastryShop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", pastryShop.PartnerStatusId);
            return View(pastryShop);
        }

        // GET: PastryShops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryShop = await _context.PastryShops.FindAsync(id);
            if (pastryShop == null)
            {
                return NotFound();
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", pastryShop.PartnerStatusId);
            return View(pastryShop);
        }

        // POST: PastryShops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopId,ShopName,Address,PhoneNumber,PartnerStatusId,TenantId")] PastryShop pastryShop)
        {
            if (id != pastryShop.ShopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastryShop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastryShopExists(pastryShop.ShopId))
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
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", pastryShop.PartnerStatusId);
            return View(pastryShop);
        }

        // GET: PastryShops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryShop = await _context.PastryShops
                .Include(p => p.PartnerStatus)
                .FirstOrDefaultAsync(m => m.ShopId == id);
            if (pastryShop == null)
            {
                return NotFound();
            }

            return View(pastryShop);
        }

        // POST: PastryShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastryShop = await _context.PastryShops.FindAsync(id);
            if (pastryShop != null)
            {
                _context.PastryShops.Remove(pastryShop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastryShopExists(int id)
        {
            return _context.PastryShops.Any(e => e.ShopId == id);
        }
    }
}
