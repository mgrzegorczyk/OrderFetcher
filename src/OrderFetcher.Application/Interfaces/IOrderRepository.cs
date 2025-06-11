using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Application.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
    Task DeleteAsync(int id);
}