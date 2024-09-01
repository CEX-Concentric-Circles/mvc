using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Models
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(CustomerDto customer);
        Task<bool> UpdateCustomerAsync(int id, CustomerDto customer);
        Task<bool> DeleteCustomerAsync(int id);
        bool CustomerExists(int id);
    }
}