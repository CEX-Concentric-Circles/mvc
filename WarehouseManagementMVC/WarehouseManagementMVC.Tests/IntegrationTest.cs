// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using WarehouseManagementMVC.Controllers;
// using WarehouseManagementMVC.Data;
// using WarehouseManagementMVC.Dtos;
// using WarehouseManagementMVC.Models;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace WarehouseManagementTests
// {
//     public class WarehouseManagementIntegrationTests
//     {
//         private readonly WmsContext _context;
//         private readonly ProductsController _productsController;
//         private readonly InventoriesController _inventoryController;
//         private readonly OrdersController _ordersController;
//
//         public WarehouseManagementIntegrationTests()
//         {
//             var options = new DbContextOptionsBuilder<WmsContext>()
//                 .UseInMemoryDatabase(databaseName: "mydatabase")
//                 .Options;
//
//             _context = new WmsContext(options);
//
//             // Ensure the database is created
//             _context.Database.EnsureCreated();
//
//             // Initialize controllers
//             _productsController = new ProductsController(_context);
//             _inventoryController = new InventoriesController(_context);
//             _ordersController = new OrdersController(_context);
//         }
//
//         [Fact]
//         public async Task CreateProduct_AddToInventory_CreateAndProcessOrder()
//         {
//             // Step 1: Create a Product
//             var product = new Product
//             {
//                 Name = "Test Product",
//                 Description = "Test Description"
//             };
//
//             var result = await _productsController.PostProduct(product) as ActionResult<Product>;
//             Assert.NotNull(result);
//             var createdProduct = result.Value as Product;
//             Assert.NotNull(createdProduct);
//             Assert.Equal("Test Product", createdProduct.Name);
//
//             // Step 2: Add it to the Inventory
//             var inventoryDto = new InventoryPostDto
//             {
//                 ProductId = createdProduct.Id,
//                 Quantity = 50
//             };
//
//             var inventoryResult = await _inventoryController.PostInventory(inventoryDto) as ActionResult<Inventory>;
//             Assert.NotNull(inventoryResult);
//             var addedInventory = inventoryResult.Value as Inventory;
//             Assert.NotNull(addedInventory);
//             Assert.Equal(50, addedInventory.Quantity);
//
//             // Step 3: Create an Order
//             var orderDto = new OrderPostDto
//             {
//                 OrderDate = DateTime.Now,
//                 OrderStatus = "Processing",
//                 OrderProducts = new List<OrderProductPostDto>
//                 {
//                     new OrderProductPostDto { ProductId = createdProduct.Id, Quantity = 20 }
//                 }
//             };
//
//             var orderResult = await _ordersController.PostOrder(orderDto) as ActionResult<OrderPostDto>;
//             Assert.NotNull(orderResult);
//             var createdOrder = orderResult.Value;
//             Assert.NotNull(createdOrder);
//             Assert.Equal("Processing", createdOrder.OrderStatus);
//             Assert.Single(createdOrder.OrderProducts);
//             Assert.Equal(20, createdOrder.OrderProducts.First().Quantity);
//
//             // Step 4: Verify Inventory Update after Order Processing
//             var updatedInventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == createdProduct.Id);
//
//             Assert.NotNull(updatedInventory);
//             Assert.Equal(30, updatedInventory.Quantity); // Original quantity (50) - Ordered quantity (20)
//         }
//     }
// }
