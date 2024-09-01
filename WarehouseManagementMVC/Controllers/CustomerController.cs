using Microsoft.AspNetCore.Mvc;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is null.");
            }
            
            var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
            
            if (createdCustomer == null)
            {
                return Conflict("Could not create the customer.");
            }
            
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is null.");
            }
            
            var result = await _customerService.UpdateCustomerAsync(id, customerDto);
            if (!result)
            {
                return NotFound("Customer not found or unable to update.");
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
            {
                return NotFound("Customer not found.");
            }

            return NoContent();
        }
    }
}
