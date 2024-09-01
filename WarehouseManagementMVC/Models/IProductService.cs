
using WarehouseManagementMVC.Dtos;

namespace WarehouseManagementMVC.Models;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<bool> UpdateProductAsync(int id, ProductDto productDto);
    Task<Product> CreateProductAsync(ProductDto product);
    Task<bool> DeleteProductAsync(int id);
    bool ProductExists(int id);
}