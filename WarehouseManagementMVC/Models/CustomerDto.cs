using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementMVC.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}