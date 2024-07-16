using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace WarehouseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly WmsContext _context;

        public OrderController(WmsContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Product).ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/Products
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // POST: api/Orders
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = order.Id }, order);
            }
            return BadRequest(ModelState);
        }

        // GET: api/Orders/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var products = await _context.Products.ToListAsync();
            return Ok(new { order, products });
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
