using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementMVC.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    
    public int CustomerId { get; set; }

    public string OrderStatus { get; set; }
}