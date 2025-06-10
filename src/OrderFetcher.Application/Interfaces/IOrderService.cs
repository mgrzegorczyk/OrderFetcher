using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<Order> AddOrderAsync(Order order);
    Task DeleteOrderAsync(int orderId);
}