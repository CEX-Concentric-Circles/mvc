using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementMVC.Models;

public class Inventory
{
    public int Id { get; set; }
        
    [Required]
    public int ProductId { get; set; }
        
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
        
    [Required]
    public int Quantity { get; set; }
}