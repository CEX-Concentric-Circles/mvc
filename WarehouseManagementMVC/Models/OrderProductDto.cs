using System.ComponentModel.DataAnnotations;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Dtos
{
    public class OrderProductPostDto
    {
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
    
    public class OrderProductGetDto
    {
        public int ProductId { get; set; }
        
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
    
    public class OrderProductPutDto
    {
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}