using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly WmsContext _context;

        public InventoryService(WmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventories.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> UpdateInventoryAsync(int id, InventoryPutDto inventoryDto)
        {
            var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.Id == id);
            if (inventory == null)
                return false;

            inventory.Quantity = inventoryDto.Quantity;

            _context.Inventories.Update(inventory);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
                    return false;
                else
                    throw;
            }

            return true;
        }

        public async Task<Inventory> CreateInventoryAsync(InventoryPostDto inventoryDto)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == inventoryDto.ProductId);
            if (product == null)
            {
                return null; 
            }
            
            var existingInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == inventoryDto.ProductId);
            if (existingInventory != null)
            {
                return null; 
            }

            var newInventory = new Inventory
            {
                ProductId = inventoryDto.ProductId,
                Quantity = inventoryDto.Quantity
            };

            _context.Inventories.Add(newInventory);
            await _context.SaveChangesAsync();

            return newInventory;
        }

        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
                return false;

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
