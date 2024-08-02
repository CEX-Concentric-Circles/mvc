using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly WmsContext _context;

        public InventoriesController(WmsContext context)
        {
            _context = context;
        }

        // GET: api/Inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            return await _context.Inventories.ToListAsync();
        }

        // GET: api/Inventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }

        // PUT: api/Inventories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, InventoryPutDto inventory)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var findInventory = await _context.Inventories.SingleOrDefaultAsync(i => i.Id == id);

            if (findInventory == null)
            {
                return NotFound();
            }
            _context.Inventories.Update(new Inventory
            {
                Id = id,
                ProductId = findInventory.ProductId,
                Quantity = inventory.Quantity
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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

        // POST: api/Inventories
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(InventoryPostDto inventory)
        {
            // Check if the product exists
            var findProduct = await _context.Products.SingleOrDefaultAsync(i => i.Id == inventory.ProductId);
    
            if (findProduct == null)
            {
                return NotFound();
            }
            
            var findInventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == inventory.ProductId);

            if (findInventory != null)
            {
                return Content($"Inventory for ProductID {inventory.ProductId} already Exist");
            }
            
            // Create a new Inventory entry
            var newInventory = new Inventory
            {
                ProductId = inventory.ProductId,
                Quantity = inventory.Quantity
            };
    
            // Add the new inventory to the context
            _context.Inventories.Add(newInventory);
            await _context.SaveChangesAsync();
    
            // Return a response with the newly created inventory
            return CreatedAtAction(nameof(GetInventory), new { id = newInventory.Id }, newInventory);
        }

        // DELETE: api/Inventories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
