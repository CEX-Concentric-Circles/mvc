// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using WarehouseManagementMVC.Data;
//
// public class IntegrationTestsFixture : IDisposable
// {
//     public WmsContext Context { get; private set; }
//     public IServiceProvider ServiceProvider { get; private set; }
//
//     public IntegrationTestsFixture()
//     {
//         var options = new DbContextOptionsBuilder<WmsContext>()
//             .UseInMemoryDatabase("TestDatabase")
//             .Options;
//
//         Context = new WmsContext(options);
//
//         ServiceProvider = new ServiceCollection()
//             .AddSingleton(Context)
//             .BuildServiceProvider();
//     }
//
//     public void Dispose()
//     {
//         Context.Database.EnsureDeleted();
//         Context.Dispose();
//     }
// }