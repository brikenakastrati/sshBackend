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
    public class VenueProvidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenueProvidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VenueProviders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VenueProviders.Include(v => v.PartnerStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VenueProviders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueProvider = await _context.VenueProviders
                .Include(v => v.PartnerStatus)
                .FirstOrDefaultAsync(m => m.VenueProviderId == id);
            if (venueProvider == null)
            {
                return NotFound();
            }

            return View(venueProvider);
        }

        // GET: VenueProviders/Create
        public IActionResult Create()
        {
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId");
            return View();
        }

        // POST: VenueProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueProviderId,Name,Address,PhoneNumber,Email,AgencyFee,PartnerStatusId,TenantId")] VenueProvider venueProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venueProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", venueProvider.PartnerStatusId);
            return View(venueProvider);
        }

        // GET: VenueProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueProvider = await _context.VenueProviders.FindAsync(id);
            if (venueProvider == null)
            {
                return NotFound();
            }
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", venueProvider.PartnerStatusId);
            return View(venueProvider);
        }

        // POST: VenueProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueProviderId,Name,Address,PhoneNumber,Email,AgencyFee,PartnerStatusId,TenantId")] VenueProvider venueProvider)
        {
            if (id != venueProvider.VenueProviderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venueProvider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueProviderExists(venueProvider.VenueProviderId))
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
            ViewData["PartnerStatusId"] = new SelectList(_context.PartnerStatuses, "PartnerStatusId", "PartnerStatusId", venueProvider.PartnerStatusId);
            return View(venueProvider);
        }

        // GET: VenueProviders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venueProvider = await _context.VenueProviders
                .Include(v => v.PartnerStatus)
                .FirstOrDefaultAsync(m => m.VenueProviderId == id);
            if (venueProvider == null)
            {
                return NotFound();
            }

            return View(venueProvider);
        }

        // POST: VenueProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venueProvider = await _context.VenueProviders.FindAsync(id);
            if (venueProvider != null)
            {
                _context.VenueProviders.Remove(venueProvider);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueProviderExists(int id)
        {
            return _context.VenueProviders.Any(e => e.VenueProviderId == id);
        }
    }
}
