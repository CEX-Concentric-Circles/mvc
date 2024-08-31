using Microsoft.AspNetCore.Mvc;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public InventoriesController(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
        }

        // GET: api/Inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            var inventories = await _inventoryService.GetInventoriesAsync();
            return Ok(inventories);
        }

        // GET: api/Inventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // PUT: api/Inventories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, InventoryPutDto inventoryDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid inventory ID.");
            }

            var result = await _inventoryService.UpdateInventoryAsync(id, inventoryDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Inventories
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(InventoryPostDto inventoryDto)
        {
            var createdInventory = await _inventoryService.CreateInventoryAsync(inventoryDto);

            if (createdInventory == null)
            {
                var productExists = _productService.ProductExists(inventoryDto.ProductId);
                if (!productExists)
                {
                    return NotFound($"Product with ID {inventoryDto.ProductId} does not exist.");
                }
                else
                {
                    return Conflict($"Inventory for ProductID {inventoryDto.ProductId} already exists.");
                }
            }

            return CreatedAtAction(nameof(GetInventory), new { id = createdInventory.Id }, createdInventory);
        }

        // DELETE: api/Inventories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var result = await _inventoryService.DeleteInventoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
