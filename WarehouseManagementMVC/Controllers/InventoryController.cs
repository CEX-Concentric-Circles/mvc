using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Models;

namespace WarehouseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly WmsContext _context;

        public InventoryController(WmsContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventories.Include(i => i.Product).ToListAsync();
            return Ok(inventories);
        }

        // GET: api/Inventory/Products
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.Select(p => new { p.Id, p.Name }).ToListAsync();
            return Ok(products);
        }

        // POST: api/Inventory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = inventory.Id }, inventory);
            }
            return BadRequest(ModelState);
        }

        // GET: api/Inventory/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Select(p => new { p.Id, p.Name }).ToListAsync();
            return Ok(new { inventory, products });
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Id))
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
            return BadRequest(ModelState);
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
