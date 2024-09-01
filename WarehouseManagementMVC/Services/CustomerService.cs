using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly WmsContext _context;

        public CustomerService(WmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = new Customer
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                Email = customerDto.Email
            };
            
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(int id, CustomerDto customer)
        {
            var findCustomer = await _context.Customers.FindAsync(id);
            if (findCustomer == null)
            {
                return false;
            }

            findCustomer.Name = customer.Name;
            findCustomer.Email = customer.Email;

            _context.Entry(findCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
