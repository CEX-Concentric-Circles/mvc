using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementMVC.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; }

    [Required]
    public int Quantity { get; set; }
}