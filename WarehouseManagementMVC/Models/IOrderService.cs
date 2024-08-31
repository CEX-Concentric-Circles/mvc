using WarehouseManagementMVC.Dtos;

namespace WarehouseManagementMVC.Models
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderGetDto>> GetOrdersAsync();
        Task<OrderGetDto> GetOrderByIdAsync(int id);
        Task<OrderPostDto> CreateOrderAsync(OrderPostDto orderDto);
        Task<bool> UpdateOrderAsync(int id, OrderPutDto orderDto);
        Task<bool> DeleteOrderAsync(int id);
    }
}