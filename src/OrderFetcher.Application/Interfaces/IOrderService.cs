using OrderFetcher.Application.Models;
using OrderFetcher.Application.Services;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Interfaces;

public interface IOrderService
{
    Task<PagedResult<Order>> GetOrdersAsync(int page, int pageSize);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<Order> AddOrderAsync(Order order);
    Task DeleteOrderAsync(int orderId);
}