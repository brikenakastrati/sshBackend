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
    public class MusicProviderOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicProviderOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MusicProviderOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MusicProviderOrders.Include(m => m.Event).Include(m => m.OrderStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MusicProviderOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProviderOrder = await _context.MusicProviderOrders
                .Include(m => m.Event)
                .Include(m => m.OrderStatus)
                .FirstOrDefaultAsync(m => m.MusicProviderOrderId == id);
            if (musicProviderOrder == null)
            {
                return NotFound();
            }

            return View(musicProviderOrder);
        }

        // GET: MusicProviderOrders/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            return View();
        }

        // POST: MusicProviderOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MusicProviderOrderId,OrderName,OrderPrice,AgencyFee,Notes,MusicProviderAddress,PhoneNumber,Email,EventId,OrderStatusId,TenantId")] MusicProviderOrder musicProviderOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicProviderOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", musicProviderOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", musicProviderOrder.OrderStatusId);
            return View(musicProviderOrder);
        }

        // GET: MusicProviderOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProviderOrder = await _context.MusicProviderOrders.FindAsync(id);
            if (musicProviderOrder == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", musicProviderOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", musicProviderOrder.OrderStatusId);
            return View(musicProviderOrder);
        }

        // POST: MusicProviderOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicProviderOrderId,OrderName,OrderPrice,AgencyFee,Notes,MusicProviderAddress,PhoneNumber,Email,EventId,OrderStatusId,TenantId")] MusicProviderOrder musicProviderOrder)
        {
            if (id != musicProviderOrder.MusicProviderOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musicProviderOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicProviderOrderExists(musicProviderOrder.MusicProviderOrderId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", musicProviderOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", musicProviderOrder.OrderStatusId);
            return View(musicProviderOrder);
        }

        // GET: MusicProviderOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProviderOrder = await _context.MusicProviderOrders
                .Include(m => m.Event)
                .Include(m => m.OrderStatus)
                .FirstOrDefaultAsync(m => m.MusicProviderOrderId == id);
            if (musicProviderOrder == null)
            {
                return NotFound();
            }

            return View(musicProviderOrder);
        }

        // POST: MusicProviderOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musicProviderOrder = await _context.MusicProviderOrders.FindAsync(id);
            if (musicProviderOrder != null)
            {
                _context.MusicProviderOrders.Remove(musicProviderOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicProviderOrderExists(int id)
        {
            return _context.MusicProviderOrders.Any(e => e.MusicProviderOrderId == id);
        }
    }
}
