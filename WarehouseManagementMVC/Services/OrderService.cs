using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Dtos;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly WmsContext _context;

        public OrderService(WmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderGetDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(oc => oc.Customer)
                .ToListAsync();

            return orders.Select(o => new OrderGetDto
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
        }

        public async Task<OrderGetDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(oc => oc.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            return new OrderGetDto
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
        }

        public async Task<OrderPostDto> CreateOrderAsync(OrderPostDto orderDto)
        {
            var order = new Order
            {
                OrderDate = orderDto.OrderDate,
                OrderStatus = orderDto.OrderStatus,
                CustomerId = orderDto.CustomerId
            };

            var findCustomer = await _context.Customers.SingleOrDefaultAsync(i => i.Id == orderDto.CustomerId);
            if (findCustomer == null)
            {
                return null; // Customer not found
            }

            foreach (var opDto in orderDto.OrderProducts)
            {
                var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.ProductId == opDto.ProductId);

                if (inventory == null)
                {
                    return null; // Product not found
                }

                if (inventory.Quantity < opDto.Quantity)
                {
                    return null; // Insufficient quantity
                }

                inventory.Quantity -= opDto.Quantity;
                _context.Entry(inventory).State = EntityState.Modified;

                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = opDto.ProductId,
                    Quantity = opDto.Quantity
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderPostDto
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
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderPutDto orderDto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
            {
                return false;
            }

            _context.OrderProducts.RemoveRange(existingOrder.OrderProducts);

            foreach (var opDto in orderDto.OrderProducts)
            {
                var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.ProductId == opDto.ProductId);

                if (inventory == null)
                {
                    return false;
                }

                if (inventory.Quantity < opDto.Quantity)
                {
                    return false;
                }

                var existingQuantity = existingOrder.OrderProducts
                    .FirstOrDefault(op => op.ProductId == opDto.ProductId)?.Quantity ?? 0;

                inventory.Quantity -= opDto.Quantity - existingQuantity;
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

            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            foreach (var op in order.OrderProducts)
            {
                var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.ProductId == op.ProductId);

                if (inventory != null)
                {
                    inventory.Quantity += op.Quantity;
                    _context.Entry(inventory).State = EntityState.Modified;
                }
            }

            _context.OrderProducts.RemoveRange(order.OrderProducts);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
