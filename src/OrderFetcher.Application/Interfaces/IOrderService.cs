using OrderFetcher.Application.Dtos;
using OrderFetcher.Application.Models;
using OrderFetcher.Application.Services;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Interfaces;

public interface IOrderService
{
    Task<PagedResult<OrderDto>> GetOrdersAsync(int page, int pageSize);
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task<Order> AddOrderAsync(Order order);
    Task DeleteOrderAsync(int orderId);
}