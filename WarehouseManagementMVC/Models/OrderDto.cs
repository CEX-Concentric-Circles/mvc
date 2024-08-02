using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementMVC.Dtos
{
    public class OrderGetDto
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public ICollection<OrderProductGetDto> OrderProducts { get; set; } = new List<OrderProductGetDto>();

        public string OrderStatus { get; set; }
    }
    
    public class OrderPostDto
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public ICollection<OrderProductPostDto> OrderProducts { get; set; } = new List<OrderProductPostDto>();

        public int CustomerId { get; set; }
        public string OrderStatus { get; set; }
    }
    
    public class OrderPutDto
    {
        public ICollection<OrderProductPostDto> OrderProducts { get; set; } = new List<OrderProductPostDto>();

        public string OrderStatus { get; set; }
    }
}