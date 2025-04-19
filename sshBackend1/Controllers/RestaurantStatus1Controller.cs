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
    public class RestaurantStatus1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantStatus1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantStatus1
        public async Task<IActionResult> Index()
        {
            return View(await _context.RestaurantStatuses.ToListAsync());
        }

        // GET: RestaurantStatus1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantStatus = await _context.RestaurantStatuses
                .FirstOrDefaultAsync(m => m.RestaurantStatusId == id);
            if (restaurantStatus == null)
            {
                return NotFound();
            }

            return View(restaurantStatus);
        }

        // GET: RestaurantStatus1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantStatus1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantStatusId,Name,Description,LastUpdated,TenantId")] RestaurantStatus restaurantStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurantStatus);
        }

        // GET: RestaurantStatus1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantStatus = await _context.RestaurantStatuses.FindAsync(id);
            if (restaurantStatus == null)
            {
                return NotFound();
            }
            return View(restaurantStatus);
        }

        // POST: RestaurantStatus1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantStatusId,Name,Description,LastUpdated,TenantId")] RestaurantStatus restaurantStatus)
        {
            if (id != restaurantStatus.RestaurantStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantStatusExists(restaurantStatus.RestaurantStatusId))
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
            return View(restaurantStatus);
        }

        // GET: RestaurantStatus1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantStatus = await _context.RestaurantStatuses
                .FirstOrDefaultAsync(m => m.RestaurantStatusId == id);
            if (restaurantStatus == null)
            {
                return NotFound();
            }

            return View(restaurantStatus);
        }

        // POST: RestaurantStatus1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantStatus = await _context.RestaurantStatuses.FindAsync(id);
            if (restaurantStatus != null)
            {
                _context.RestaurantStatuses.Remove(restaurantStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantStatusExists(int id)
        {
            return _context.RestaurantStatuses.Any(e => e.RestaurantStatusId == id);
        }
    }
}
