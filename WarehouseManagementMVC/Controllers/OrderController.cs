using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Models;
using WarehouseManagementMVC.Dtos;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly WmsContext _context;

        public OrdersController(WmsContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderGetDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(oc => oc.Customer)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderGetDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                OrderProducts = o.OrderProducts.Select(op => new OrderProductGetDto
                {
                    ProductId = op.ProductId,
                    Product = new Product
                    {
                        Id = op.Product.Id,
                        Name = op.Product.Name,
                        Description = op.Product.Description
                    },
                    Quantity = op.Quantity
                }).ToList()
            }).ToList();

            return orderDtos;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderGetDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(oc => oc.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderGetDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductGetDto
                {
                    ProductId = op.ProductId,
                    Product = new Product
                    {
                        Id = op.Product.Id,
                        Name = op.Product.Name,
                        Description = op.Product.Description
                    },
                    Quantity = op.Quantity
                }).ToList()
            };

            return orderDto;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderPostDto>> PostOrder(OrderPostDto orderDto)
        {
            var order = new Order
            {
                OrderDate = orderDto.OrderDate,
                OrderStatus = orderDto.OrderStatus
            };
            var findCustomer = await _context.Customers.SingleOrDefaultAsync(i => i.Id == orderDto.CustomerId);
            if (findCustomer == null)
            {
                return NotFound("Customer not found.");
            }
            
            foreach (var opDto in orderDto.OrderProducts)
            {
                var inventory = await _context.Inventories
                    .SingleOrDefaultAsync(i => i.ProductId == opDto.ProductId);

                if (inventory == null)
                {
                    return NotFound($"Product with ID {opDto.ProductId} not found");
                }

                if (inventory.Quantity < opDto.Quantity)
                {
                    return BadRequest($"Insufficient quantity for product ID {opDto.ProductId}");
                }

                inventory.Quantity -= opDto.Quantity;
                _context.Entry(inventory).State = EntityState.Modified;

                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = opDto.ProductId,
                    Quantity = opDto.Quantity
                });

                order.CustomerId = findCustomer.Id;
            }
            Console.WriteLine(findCustomer.Id);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
           
            var createdOrderDto = new OrderPostDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductPostDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList(),
                CustomerId = findCustomer.Id
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrderDto);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderPutDto orderDto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            _context.OrderProducts.RemoveRange(existingOrder.OrderProducts);

            foreach (var opDto in orderDto.OrderProducts)
            {
                var inventory = await _context.Inventories
                    .SingleOrDefaultAsync(i => i.ProductId == opDto.ProductId);

                if (inventory == null)
                {
                    return NotFound($"Product with ID {opDto.ProductId} not found");
                }

                if (inventory.Quantity < opDto.Quantity)
                {
                    return BadRequest($"Insufficient quantity for product ID {opDto.ProductId}");
                }

                var exisQuantity = existingOrder.OrderProducts.ToList().Find(e => e.ProductId == opDto.ProductId);
                
                inventory.Quantity -= opDto.Quantity - exisQuantity.Quantity;
                _context.Entry(inventory).State = EntityState.Modified;

                _context.OrderProducts.Add(new OrderProduct
                {
                    OrderId = id,
                    ProductId = opDto.ProductId,
                    Quantity = opDto.Quantity
                });
            }
            
            existingOrder.OrderStatus = orderDto.OrderStatus;
            _context.Entry(existingOrder).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            foreach (var op in order.OrderProducts)
            {
                var inventory = await _context.Inventories
                    .SingleOrDefaultAsync(i => i.ProductId == op.ProductId);

                if (inventory != null)
                {
                    inventory.Quantity += op.Quantity;
                    _context.Entry(inventory).State = EntityState.Modified;
                }
            }

            _context.OrderProducts.RemoveRange(order.OrderProducts);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
