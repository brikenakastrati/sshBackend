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
    public class VenueOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenueOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VenueOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VenueOrders.Include(v => v.Event).Include(v => v.OrderStatus).Include(v => v.Venue);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VenueOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueOrder = await _context.VenueOrders
                .Include(v => v.Event)
                .Include(v => v.OrderStatus)
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.VenueOrderId == id);
            if (venueOrder == null)
            {
                return NotFound();
            }

            return View(venueOrder);
        }

        // GET: VenueOrders/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueId");
            return View();
        }

        // POST: VenueOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueOrderId,VenueId,Name,Description,Price,Address,AgencyFee,Notes,EventId,OrderStatusId,TenantId")] VenueOrder venueOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venueOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", venueOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", venueOrder.OrderStatusId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueId", venueOrder.VenueId);
            return View(venueOrder);
        }

        // GET: VenueOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueOrder = await _context.VenueOrders.FindAsync(id);
            if (venueOrder == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", venueOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", venueOrder.OrderStatusId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueId", venueOrder.VenueId);
            return View(venueOrder);
        }

        // POST: VenueOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueOrderId,VenueId,Name,Description,Price,Address,AgencyFee,Notes,EventId,OrderStatusId,TenantId")] VenueOrder venueOrder)
        {
            if (id != venueOrder.VenueOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venueOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueOrderExists(venueOrder.VenueOrderId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", venueOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", venueOrder.OrderStatusId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueId", venueOrder.VenueId);
            return View(venueOrder);
        }

        // GET: VenueOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueOrder = await _context.VenueOrders
                .Include(v => v.Event)
                .Include(v => v.OrderStatus)
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.VenueOrderId == id);
            if (venueOrder == null)
            {
                return NotFound();
            }

            return View(venueOrder);
        }

        // POST: VenueOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venueOrder = await _context.VenueOrders.FindAsync(id);
            if (venueOrder != null)
            {
                _context.VenueOrders.Remove(venueOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueOrderExists(int id)
        {
            return _context.VenueOrders.Any(e => e.VenueOrderId == id);
        }
    }
}
