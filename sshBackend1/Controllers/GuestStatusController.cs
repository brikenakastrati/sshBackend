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
    public class GuestStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GuestStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.GuestStatuses.ToListAsync());
        }

        // GET: GuestStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guestStatus = await _context.GuestStatuses
                .FirstOrDefaultAsync(m => m.GuestStatusId == id);
            if (guestStatus == null)
            {
                return NotFound();
            }

            return View(guestStatus);
        }

        // GET: GuestStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GuestStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestStatusId,GuestStatusName,TenantId")] GuestStatus guestStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guestStatus);
        }

        // GET: GuestStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guestStatus = await _context.GuestStatuses.FindAsync(id);
            if (guestStatus == null)
            {
                return NotFound();
            }
            return View(guestStatus);
        }

        // POST: GuestStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestStatusId,GuestStatusName,TenantId")] GuestStatus guestStatus)
        {
            if (id != guestStatus.GuestStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guestStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestStatusExists(guestStatus.GuestStatusId))
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
            return View(guestStatus);
        }

        // GET: GuestStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guestStatus = await _context.GuestStatuses
                .FirstOrDefaultAsync(m => m.GuestStatusId == id);
            if (guestStatus == null)
            {
                return NotFound();
            }

            return View(guestStatus);
        }

        // POST: GuestStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guestStatus = await _context.GuestStatuses.FindAsync(id);
            if (guestStatus != null)
            {
                _context.GuestStatuses.Remove(guestStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestStatusExists(int id)
        {
            return _context.GuestStatuses.Any(e => e.GuestStatusId == id);
        }
    }
}
