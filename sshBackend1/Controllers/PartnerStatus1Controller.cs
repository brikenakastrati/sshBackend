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
    public class PartnerStatus1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnerStatus1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartnerStatus1
        public async Task<IActionResult> Index()
        {
            return View(await _context.PartnerStatuses.ToListAsync());
        }

        // GET: PartnerStatus1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerStatus = await _context.PartnerStatuses
                .FirstOrDefaultAsync(m => m.PartnerStatusId == id);
            if (partnerStatus == null)
            {
                return NotFound();
            }

            return View(partnerStatus);
        }

        // GET: PartnerStatus1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartnerStatus1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartnerStatusId,Name,TenantId")] PartnerStatus partnerStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partnerStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partnerStatus);
        }

        // GET: PartnerStatus1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerStatus = await _context.PartnerStatuses.FindAsync(id);
            if (partnerStatus == null)
            {
                return NotFound();
            }
            return View(partnerStatus);
        }

        // POST: PartnerStatus1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartnerStatusId,Name,TenantId")] PartnerStatus partnerStatus)
        {
            if (id != partnerStatus.PartnerStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerStatusExists(partnerStatus.PartnerStatusId))
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
            return View(partnerStatus);
        }

        // GET: PartnerStatus1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerStatus = await _context.PartnerStatuses
                .FirstOrDefaultAsync(m => m.PartnerStatusId == id);
            if (partnerStatus == null)
            {
                return NotFound();
            }

            return View(partnerStatus);
        }

        // POST: PartnerStatus1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnerStatus = await _context.PartnerStatuses.FindAsync(id);
            if (partnerStatus != null)
            {
                _context.PartnerStatuses.Remove(partnerStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerStatusExists(int id)
        {
            return _context.PartnerStatuses.Any(e => e.PartnerStatusId == id);
        }
    }
}
