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
    public class MusicProvidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicProvidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MusicProviders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MusicProviders.Include(m => m.PartnerStatus).Include(m => m.PerformerType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MusicProviders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProvider = await _context.MusicProviders
                .Include(m => m.PartnerStatus)
                .Include(m => m.PerformerType)
                .FirstOrDefaultAsync(m => m.MusicProviderId == id);
            if (musicProvider == null)
            {
                return NotFound();
            }

            return View(musicProvider);
        }

        // GET: MusicProviders/Create
        public IActionResult Create()
        {
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId");
            ViewData["PerformerTypeId"] = new SelectList(_context.PerformerTypes, "PerformerTypeId", "PerformerTypeId");
            return View();
        }

        // POST: MusicProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MusicProviderId,Name,Address,PhoneNumber,Email,AgencyFee,PerformerTypeId,PartnerStatusId,BaseHourlyRate,TenantId")] MusicProvider musicProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", musicProvider.PartnerStatusId);
            ViewData["PerformerTypeId"] = new SelectList(_context.PerformerTypes, "PerformerTypeId", "PerformerTypeId", musicProvider.PerformerTypeId);
            return View(musicProvider);
        }

        // GET: MusicProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProvider = await _context.MusicProviders.FindAsync(id);
            if (musicProvider == null)
            {
                return NotFound();
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", musicProvider.PartnerStatusId);
            ViewData["PerformerTypeId"] = new SelectList(_context.PerformerTypes, "PerformerTypeId", "PerformerTypeId", musicProvider.PerformerTypeId);
            return View(musicProvider);
        }

        // POST: MusicProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicProviderId,Name,Address,PhoneNumber,Email,AgencyFee,PerformerTypeId,PartnerStatusId,BaseHourlyRate,TenantId")] MusicProvider musicProvider)
        {
            if (id != musicProvider.MusicProviderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musicProvider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicProviderExists(musicProvider.MusicProviderId))
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
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", musicProvider.PartnerStatusId);
            ViewData["PerformerTypeId"] = new SelectList(_context.PerformerTypes, "PerformerTypeId", "PerformerTypeId", musicProvider.PerformerTypeId);
            return View(musicProvider);
        }

        // GET: MusicProviders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicProvider = await _context.MusicProviders
                .Include(m => m.PartnerStatus)
                .Include(m => m.PerformerType)
                .FirstOrDefaultAsync(m => m.MusicProviderId == id);
            if (musicProvider == null)
            {
                return NotFound();
            }

            return View(musicProvider);
        }

        // POST: MusicProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musicProvider = await _context.MusicProviders.FindAsync(id);
            if (musicProvider != null)
            {
                _context.MusicProviders.Remove(musicProvider);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicProviderExists(int id)
        {
            return _context.MusicProviders.Any(e => e.MusicProviderId == id);
        }
    }
}
