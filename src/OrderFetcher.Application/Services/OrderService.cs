using System.Collections.Generic;
using System.Threading.Tasks;
using OrderFetcher.Domain.Entities;
using OrderFetcher.Infrastructure.Repositories;

namespace OrderFetcher.Application.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> AddOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
    }
    
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // TODO: OrderDto
        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        // TODO: OrderDto
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }
            return order;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            return await _orderRepository.AddAsync(order);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteAsync(orderId);
        }
    }
    
}