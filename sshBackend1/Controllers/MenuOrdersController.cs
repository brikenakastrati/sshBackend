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
    public class MenuOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MenuOrders.Include(m => m.Event).Include(m => m.OrderStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MenuOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders
                .Include(m => m.Event)
                .Include(m => m.OrderStatus)
                .FirstOrDefaultAsync(m => m.MenuOrderId == id);
            if (menuOrder == null)
            {
                return NotFound();
            }

            return View(menuOrder);
        }

        // GET: MenuOrders/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId");
            return View();
        }

        // POST: MenuOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuOrderId,OrderName,OrderPrice,AgencyFee,Allergents,IngreedientsForbiddenByReligion,AdditionalRequests,EventId,OrderStatusId,TenantId")] MenuOrder menuOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", menuOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", menuOrder.OrderStatusId);
            return View(menuOrder);
        }

        // GET: MenuOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders.FindAsync(id);
            if (menuOrder == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", menuOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", menuOrder.OrderStatusId);
            return View(menuOrder);
        }

        // POST: MenuOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuOrderId,OrderName,OrderPrice,AgencyFee,Allergents,IngreedientsForbiddenByReligion,AdditionalRequests,EventId,OrderStatusId,TenantId")] MenuOrder menuOrder)
        {
            if (id != menuOrder.MenuOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuOrderExists(menuOrder.MenuOrderId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", menuOrder.EventId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "OrderStatusId", "OrderStatusId", menuOrder.OrderStatusId);
            return View(menuOrder);
        }

        // GET: MenuOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders
                .Include(m => m.Event)
                .Include(m => m.OrderStatus)
                .FirstOrDefaultAsync(m => m.MenuOrderId == id);
            if (menuOrder == null)
            {
                return NotFound();
            }

            return View(menuOrder);
        }

        // POST: MenuOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuOrder = await _context.MenuOrders.FindAsync(id);
            if (menuOrder != null)
            {
                _context.MenuOrders.Remove(menuOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuOrderExists(int id)
        {
            return _context.MenuOrders.Any(e => e.MenuOrderId == id);
        }
    }
}
