using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MenuOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuOrder>>> GetMenuOrders()
        {
            return await _context.MenuOrders.ToListAsync();
        }

        // GET: api/MenuOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuOrder>> GetMenuOrder(int id)
        {
            var menuOrder = await _context.MenuOrders.FindAsync(id);

            if (menuOrder == null)
            {
                return NotFound();
            }

            return menuOrder;
        }

        // PUT: api/MenuOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuOrder(int id, MenuOrder menuOrder)
        {
            if (id != menuOrder.MenuOrderId)
            {
                return BadRequest();
            }

            _context.Entry(menuOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MenuOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MenuOrder>> PostMenuOrder(MenuOrder menuOrder)
        {
            _context.MenuOrders.Add(menuOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuOrder", new { id = menuOrder.MenuOrderId }, menuOrder);
        }

        // DELETE: api/MenuOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuOrder(int id)
        {
            var menuOrder = await _context.MenuOrders.FindAsync(id);
            if (menuOrder == null)
            {
                return NotFound();
            }

            _context.MenuOrders.Remove(menuOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuOrderExists(int id)
        {
            return _context.MenuOrders.Any(e => e.MenuOrderId == id);
        }
    }
}
