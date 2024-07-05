using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementMVC.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}