using WarehouseManagementMVC.Dtos;

namespace WarehouseManagementMVC.Models
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int id);
        Task<bool> UpdateInventoryAsync(int id, InventoryPutDto inventoryDto);
        Task<Inventory> CreateInventoryAsync(InventoryPostDto inventoryDto);
        Task<bool> DeleteInventoryAsync(int id);
        bool InventoryExists(int id);
    }
}