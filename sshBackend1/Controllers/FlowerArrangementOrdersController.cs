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
    public class FlowerArrangementOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowerArrangementOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlowerArrangementOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FlowerArrangementOrders.Include(f => f.Event).Include(f => f.OrderStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FlowerArrangementOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementOrder = await _context.FlowerArrangementOrders
                .Include(f => f.Event)
                .Include(f => f.OrderStatus)
                .FirstOrDefaultAsync(m => m.FlowerArrangementOrderId == id);
            if (flowerArrangementOrder == null)
            {
                return NotFound();
            }

            return View(flowerArrangementOrder);
        }

        // GET: FlowerArrangementOrders/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            return View();
        }

        // POST: FlowerArrangementOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlowerArrangementOrderId,OrderName,OrderPrice,AgencyFee,OrderDescription,Notes,EventId,OrderStatusId,TenantId")] FlowerArrangementOrder flowerArrangementOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flowerArrangementOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", flowerArrangementOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", flowerArrangementOrder.OrderStatusId);
            return View(flowerArrangementOrder);
        }

        // GET: FlowerArrangementOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementOrder = await _context.FlowerArrangementOrders.FindAsync(id);
            if (flowerArrangementOrder == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", flowerArrangementOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", flowerArrangementOrder.OrderStatusId);
            return View(flowerArrangementOrder);
        }

        // POST: FlowerArrangementOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlowerArrangementOrderId,OrderName,OrderPrice,AgencyFee,OrderDescription,Notes,EventId,OrderStatusId,TenantId")] FlowerArrangementOrder flowerArrangementOrder)
        {
            if (id != flowerArrangementOrder.FlowerArrangementOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flowerArrangementOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlowerArrangementOrderExists(flowerArrangementOrder.FlowerArrangementOrderId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", flowerArrangementOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", flowerArrangementOrder.OrderStatusId);
            return View(flowerArrangementOrder);
        }

        // GET: FlowerArrangementOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerArrangementOrder = await _context.FlowerArrangementOrders
                .Include(f => f.Event)
                .Include(f => f.OrderStatus)
                .FirstOrDefaultAsync(m => m.FlowerArrangementOrderId == id);
            if (flowerArrangementOrder == null)
            {
                return NotFound();
            }

            return View(flowerArrangementOrder);
        }

        // POST: FlowerArrangementOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flowerArrangementOrder = await _context.FlowerArrangementOrders.FindAsync(id);
            if (flowerArrangementOrder != null)
            {
                _context.FlowerArrangementOrders.Remove(flowerArrangementOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlowerArrangementOrderExists(int id)
        {
            return _context.FlowerArrangementOrders.Any(e => e.FlowerArrangementOrderId == id);
        }
    }
}
