using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;

namespace L2CodePackagingAPI.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDto orderDto);
        Task SavePackagingResultAsync(Order order, List<PackagingResultInfo> results);
    }
}
