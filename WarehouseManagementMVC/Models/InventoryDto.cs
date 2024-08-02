using System.ComponentModel.DataAnnotations;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Dtos
{
    public class InventoryPutDto
    {
        [Required] public int Quantity { get; set; }
    }
    
    public class InventoryPostDto
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        [Required] public int Quantity { get; set; }
    }
}