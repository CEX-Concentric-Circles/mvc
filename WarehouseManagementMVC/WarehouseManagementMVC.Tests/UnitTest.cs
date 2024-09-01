using Moq;
using Xunit;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagementTests
{
    public class WarehouseManagementTests
    {
        private readonly WmsContext _context;

        public WarehouseManagementTests()
        {
            var options = new DbContextOptionsBuilder<WmsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WmsContext(options);
        }

        [Fact]
        public async Task FullProcessTest()
        {
            var product = await CreateProduct();
            var order = await CreateOrder(product);
            await ProcessOrder(order, product);
        }

        private async Task<Product> CreateProduct()
        {
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test Description"
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var createdProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
            Assert.NotNull(createdProduct);

            return createdProduct;
        }

        private async Task<Order> CreateOrder(Product product)
        {
            var order = new Order
            {
                OrderDate = System.DateTime.Now,
                OrderStatus = "Processing",
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = product.Id, Quantity = 10 }
                }
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.NotNull(createdOrder);
            Assert.Equal("Processing", createdOrder.OrderStatus);
            Assert.Single(createdOrder.OrderProducts);

            return createdOrder;
        }

        private async Task ProcessOrder(Order order, Product product)
        {
            var orderProduct = order.OrderProducts.First();
            Assert.NotNull(orderProduct); 
        }
    }
}
