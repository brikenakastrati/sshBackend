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
    public class MenuTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuTypes.ToListAsync());
        }

        // GET: MenuTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuType = await _context.MenuTypes
                .FirstOrDefaultAsync(m => m.MenuTypeId == id);
            if (menuType == null)
            {
                return NotFound();
            }

            return View(menuType);
        }

        // GET: MenuTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuTypeId,TypeName,TenantId")] MenuType menuType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuType);
        }

        // GET: MenuTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuType = await _context.MenuTypes.FindAsync(id);
            if (menuType == null)
            {
                return NotFound();
            }
            return View(menuType);
        }

        // POST: MenuTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuTypeId,TypeName,TenantId")] MenuType menuType)
        {
            if (id != menuType.MenuTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuTypeExists(menuType.MenuTypeId))
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
            return View(menuType);
        }

        // GET: MenuTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuType = await _context.MenuTypes
                .FirstOrDefaultAsync(m => m.MenuTypeId == id);
            if (menuType == null)
            {
                return NotFound();
            }

            return View(menuType);
        }

        // POST: MenuTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuType = await _context.MenuTypes.FindAsync(id);
            if (menuType != null)
            {
                _context.MenuTypes.Remove(menuType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuTypeExists(int id)
        {
            return _context.MenuTypes.Any(e => e.MenuTypeId == id);
        }
    }
}
