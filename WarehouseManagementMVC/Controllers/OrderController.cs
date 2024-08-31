using Microsoft.AspNetCore.Mvc;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderGetDto>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderGetDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderPostDto>> PostOrder(OrderPostDto orderDto)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);

            if (createdOrder == null)
            {
                return BadRequest("Failed to create order: Customer or Product not found, or insufficient quantity.");
            }

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderPutDto orderDto)
        {
            var result = await _orderService.UpdateOrderAsync(id, orderDto);

            if (!result)
            {
                return BadRequest("Failed to update order: Order or Product not found, or insufficient quantity.");
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (!result)
            {
                return NotFound("Order not found.");
            }

            return NoContent();
        }
    }
}
