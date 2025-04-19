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
    public class PastryOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PastryOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PastryOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PastryOrders.Include(p => p.Event).Include(p => p.OrderStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PastryOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryOrder = await _context.PastryOrders
                .Include(p => p.Event)
                .Include(p => p.OrderStatus)
                .FirstOrDefaultAsync(m => m.PastryOrderId == id);
            if (pastryOrder == null)
            {
                return NotFound();
            }

            return View(pastryOrder);
        }

        // GET: PastryOrders/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            return View();
        }

        // POST: PastryOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PastryOrderId,OrderName,OrderPrice,AgencyFee,OrderDescription,Notes,EventId,OrderStatusId,TenantId")] PastryOrder pastryOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pastryOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", pastryOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", pastryOrder.OrderStatusId);
            return View(pastryOrder);
        }

        // GET: PastryOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryOrder = await _context.PastryOrders.FindAsync(id);
            if (pastryOrder == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", pastryOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", pastryOrder.OrderStatusId);
            return View(pastryOrder);
        }

        // POST: PastryOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PastryOrderId,OrderName,OrderPrice,AgencyFee,OrderDescription,Notes,EventId,OrderStatusId,TenantId")] PastryOrder pastryOrder)
        {
            if (id != pastryOrder.PastryOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pastryOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PastryOrderExists(pastryOrder.PastryOrderId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", pastryOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", pastryOrder.OrderStatusId);
            return View(pastryOrder);
        }

        // GET: PastryOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pastryOrder = await _context.PastryOrders
                .Include(p => p.Event)
                .Include(p => p.OrderStatus)
                .FirstOrDefaultAsync(m => m.PastryOrderId == id);
            if (pastryOrder == null)
            {
                return NotFound();
            }

            return View(pastryOrder);
        }

        // POST: PastryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pastryOrder = await _context.PastryOrders.FindAsync(id);
            if (pastryOrder != null)
            {
                _context.PastryOrders.Remove(pastryOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PastryOrderExists(int id)
        {
            return _context.PastryOrders.Any(e => e.PastryOrderId == id);
        }
    }
}
